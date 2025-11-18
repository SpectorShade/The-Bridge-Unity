using UnityEngine;

public class SnapTray : MonoBehaviour
{
    public int requiredCount = 3; // number of green pickups needed
    private int currentCount = 0;

    public WinLoseManager winLoseManager;

    [Header("Object Manager Reference")] public ObjectManager objectManager;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("WinPickup")) {
            SnapObject(other.gameObject);
        }
    }

    public void SnapObject(GameObject obj) {
        
        if (obj.TryGetComponent(out Rigidbody rb)) {
            
            if (obj.GetComponent<SnappedFlag>() != null) return;

            rb.isKinematic = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            obj.transform.position = transform.position; // lock to tray
            obj.transform.SetParent(transform); // parent to tray
            currentCount++;
            FindFirstObjectByType<VignetteFlash>().FlashGood();

            if (winLoseManager != null)
                winLoseManager.ReportGoodPickupPlaced();

            obj.gameObject.AddComponent<SnappedFlag>();

            if (objectManager != null)
                objectManager.AddGood();
        }
    }

    public bool IsComplete() => currentCount >= requiredCount;
}