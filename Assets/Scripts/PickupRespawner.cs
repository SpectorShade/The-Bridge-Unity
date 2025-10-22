using UnityEngine;

public class PickupRespawner : MonoBehaviour
{
    public Transform respawnPoint;

    public void SetRespawnPoint(Transform point)
    {
        respawnPoint = point;
    }

    public void Respawn()
    {
        if (respawnPoint == null) return;

        transform.position = respawnPoint.position;
        transform.rotation = respawnPoint.rotation;

        // Reset physics so it doesn't fly off
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        Debug.Log($"{gameObject.name} is respawning");
    }
}
