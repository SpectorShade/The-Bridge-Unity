using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    // Interaction settings
    public Transform playerCamera;
    public float pickupRange = 3f;
    public float holdDistance = 2f;
    public float moveSpeed = 15f;
    public float throwForce = 10f;

    // Crosshair
    public Image crosshairImage;       
    public Color normalColor = Color.white;
    public Color highlightColor = Color.green;
    public float normalScale = 1f;
    public float highlightScale = 1.3f;
    public float scaleSpeed = 10f;

    private GameObject heldObject;
    private Rigidbody heldObjectRb;

    //previous settings
    private CollisionDetectionMode prevCollisionMode = CollisionDetectionMode.Discrete;
    private RigidbodyInterpolation prevInterpolation = RigidbodyInterpolation.None;

    public LineRenderer trajectoryLine;
    public int linePoints = 30;
    public float timeStep = 0.05f;

    public Transform landingMarker;

    private bool isLookingAtPickup = false;


    void Update()
    {
        HandlePickupInput();
        UpdateCrosshair();
        UpdateTrajectory();
    }

    private void FixedUpdate()
    {
        if (heldObject != null && heldObjectRb != null)
            MoveObject();
    }

    void HandlePickupInput()
    {
        // Left click to pick up
        if (Input.GetMouseButtonDown(0) && heldObject == null)
        {
            TryPickUpObject();
        }

        // Release to drop
        if (Input.GetMouseButtonUp(0) && heldObject != null)
        {
            DropObject();
        }

        // Right click to throw
        if (Input.GetMouseButtonDown(1) && heldObject != null)
        {
            ThrowObject();
        }
    }

    void TryPickUpObject()
    {
        if (playerCamera == null) return;

        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        int layerMask = LayerMask.GetMask("Objects");
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange, layerMask))
        {
            if (hit.collider.CompareTag("WinPickup") || hit.collider.CompareTag("BadPickup") || hit.collider.CompareTag("NeutralPickup"))
            {
                heldObject = hit.collider.gameObject;
                heldObjectRb = heldObject.GetComponent<Rigidbody>();

                if (heldObjectRb != null)
                {

                    // store previous physics settings so we can restore later
                    prevCollisionMode = heldObjectRb.collisionDetectionMode;
                    prevInterpolation = heldObjectRb.interpolation;

                    // prepare while held
                    heldObjectRb.useGravity = false;
                    heldObjectRb.isKinematic = true;
                    heldObjectRb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                    heldObjectRb.interpolation = RigidbodyInterpolation.Interpolate;

                    // stop any current motion so it doesn't drift away
                    heldObjectRb.linearVelocity = Vector3.zero;
                    heldObjectRb.angularVelocity = Vector3.zero;
                }
                else
                {
                    // no rigidbody — just parent it (fallback)
                    heldObject.transform.SetParent(playerCamera);
                }
            }
        }
    }

    void DropObject()
    {
        if (heldObjectRb != null)
        {
            // restore physics settings
            heldObjectRb.useGravity = true;
            heldObjectRb.isKinematic = false;
            heldObjectRb.collisionDetectionMode = prevCollisionMode;
            heldObjectRb.interpolation = prevInterpolation;
        }
        else if (heldObject != null)
        {
            // if we parented it as fallback, unparent
            heldObject.transform.SetParent(null);
        }

        heldObject = null;
        heldObjectRb = null;

        // Hide trajectory visuals
        if (trajectoryLine != null) trajectoryLine.enabled = false;
        if (landingMarker != null) landingMarker.gameObject.SetActive(false);
    }

    void ThrowObject()
    {
        if (heldObjectRb != null)
        {
            // restore physics and apply impulse
            heldObjectRb.useGravity = true;
            heldObjectRb.isKinematic = false;
            heldObjectRb.collisionDetectionMode = prevCollisionMode;
            heldObjectRb.interpolation = prevInterpolation;

            heldObjectRb.AddForce(playerCamera.forward * throwForce, ForceMode.Impulse);
        }
        else if (heldObject != null)
        {
            // fallback: unparent and add basic forward movement
            heldObject.transform.SetParent(null);
            Rigidbody rb = heldObject.GetComponent<Rigidbody>();
            if (rb != null) rb.AddForce(playerCamera.forward * throwForce, ForceMode.Impulse);
        }

        heldObject = null;
        heldObjectRb = null;

        // Hide trajectory visuals
        if (trajectoryLine != null) trajectoryLine.enabled = false;
        if (landingMarker != null) landingMarker.gameObject.SetActive(false);
    }

    void MoveObject()
    {
        // target in front of camera
        Vector3 targetPos = playerCamera.position + playerCamera.forward * holdDistance;

        // move directly to the target (no lag)
        heldObjectRb.MovePosition(targetPos);
    }

    void UpdateCrosshair()
    {
        if (crosshairImage == null) return;

        Ray ray = new Ray(playerCamera.position, playerCamera.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
        {
            string t = hit.collider.tag;
            if (t == "WinPickup" || t == "BadPickup" || t == "NeutralPickup")
            {
                crosshairImage.color = highlightColor;
                isLookingAtPickup = true;
            }
            else
            {
                crosshairImage.color = normalColor;
                isLookingAtPickup = false;
            }
        }

        else
        {
            crosshairImage.color = normalColor;
            isLookingAtPickup = false;
        }

        // Animate scale
        float targetScale = isLookingAtPickup ? highlightScale : normalScale;
        crosshairImage.rectTransform.localScale = Vector3.Lerp(
            crosshairImage.rectTransform.localScale,
            Vector3.one * targetScale,
            Time.deltaTime * scaleSpeed
        );
    }

    void UpdateTrajectory()
    {
        if (trajectoryLine == null) return;

        if (heldObject != null && Input.GetMouseButton(0))
        {
            trajectoryLine.enabled = true;
            SimulateTrajectory();
        }
        else
        {
            trajectoryLine.enabled = false;
            if (landingMarker != null) landingMarker.gameObject.SetActive(false);
        }
    }


    void SimulateTrajectory()
    {
        Vector3[] points = new Vector3[linePoints];

        Vector3 startPos = heldObject.transform.position;
        Vector3 startVel = playerCamera.forward * throwForce;

        Vector3 currentPos = startPos;
        Vector3 currentVel = startVel;

        points[0] = currentPos;
        int pointIndex = 1;

        bool hitSomething = false;
        Vector3 hitPoint = Vector3.zero;

        for (int i = 1; i < linePoints; i++)
        {
            float stepTime = timeStep;

            Vector3 nextPos = currentPos + currentVel * stepTime + 0.5f * Physics.gravity * stepTime * stepTime;
            Vector3 nextVel = currentVel + Physics.gravity * stepTime;

            // Check collision between current and next
            if (Physics.Raycast(currentPos, nextPos - currentPos, out RaycastHit hit, (nextPos - currentPos).magnitude))
            {
                points[pointIndex] = hit.point;
                hitSomething = true;
                hitPoint = hit.point;
                pointIndex++;
                break;
            }

            points[pointIndex] = nextPos;
            pointIndex++;

            currentPos = nextPos;
            currentVel = nextVel;
        }

        trajectoryLine.positionCount = pointIndex;
        trajectoryLine.SetPositions(points);

        // Update landing marker
        if (landingMarker != null)
        {
            if (hitSomething)
            {
                landingMarker.position = hitPoint;
                landingMarker.gameObject.SetActive(true);
            }
            else
            {
                landingMarker.gameObject.SetActive(false);
            }
        }
    }

}
