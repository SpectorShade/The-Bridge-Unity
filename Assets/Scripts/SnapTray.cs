using UnityEngine;

public class SnapTray : MonoBehaviour
{
    public int requiredCount = 3; // number of green pickups needed
    private int currentCount = 0;

    public WinLoseManager winLoseManager;

    [Header("Object Manager Reference")]
    public ObjectManager objectManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WinPickup"))
        {
            SnapObject(other.gameObject);
        }
    }

    public void SnapObject(GameObject obj)
    {
        if (obj.GetComponent<Rigidbody>() != null)
        {
            // Only snap if not already snapped
            if (obj.GetComponent<SnappedFlag>() != null) return;

            Rigidbody rb = obj.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            obj.transform.position = transform.position; // lock to tray
            obj.transform.SetParent(transform);          // parent to tray
            currentCount++;

            if (winLoseManager != null)
                winLoseManager.ReportGoodPickupPlaced();

            obj.gameObject.AddComponent<SnappedFlag>();

            if (objectManager != null)
                objectManager.AddGood();
        }

    }

    public bool IsComplete() => currentCount >= requiredCount;
}
