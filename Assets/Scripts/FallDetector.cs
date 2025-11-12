using UnityEngine;

public class FallDetector : MonoBehaviour
{
    public StrikeManager strikeManager;
    private void OnTriggerEnter(Collider other)
    {
        PickupRespawner respawner = other.GetComponent<PickupRespawner>();
        if (respawner != null)
        {
            respawner.Respawn();
        }

        // Only count pickups
        if (other.CompareTag("WinPickup") || other.CompareTag("BadPickup") || other.CompareTag("NeutralPickup"))
        {
            // Optionally, respawn the object
            other.GetComponent<PickupRespawner>()?.Respawn();

            // Register strike
            if (strikeManager)
                strikeManager.RegisterStrike();
        }
    }
}
