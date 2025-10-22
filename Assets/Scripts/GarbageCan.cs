using UnityEngine;

public class GarbageCan : MonoBehaviour
{
    public WinLoseManager manager;
    [Header("Object Manager Reference")]
    public ObjectManager objectManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BadPickup") || other.CompareTag("NeutralPickup"))
        {
            if (manager != null && other.CompareTag("BadPickup"))
            {
                manager.ReportBadPickupRemoved();
                if (objectManager != null)
                    objectManager.AddBad();
            }
            else if (manager != null && other.CompareTag("NeutralPickup"))
            {
                if (objectManager != null)
                    objectManager.AddNeutral();
            }

                Destroy(other.gameObject);
        }
    }
}
