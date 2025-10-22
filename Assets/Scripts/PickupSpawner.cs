using System.Collections;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    public GameObject pickupPrefab;   // prefab to spawn
    public int spawnCount = 5;        // how many to spawn
    public float spawnRadius = 1.5f;  // how far from spawner we can place pickups
    public float checkRadius = 0.3f;  // size of overlap check
    public int maxAttempts = 10;      // max tries to find safe spot

    public Transform respawnPoint; 

    void Start()
    {
        SpawnPickups();
    }

    void SpawnPickups()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 spawnPos = GetSafeSpawnPosition(transform.position, spawnRadius);

            GameObject pickup = Instantiate(pickupPrefab, spawnPos, Quaternion.identity);

            // Assign respawn point if available
            PickupRespawner respawner = pickup.GetComponent<PickupRespawner>();
            if (respawner != null && respawnPoint != null)
            {
                respawner.SetRespawnPoint(respawnPoint);
            }
        }
    }


    Vector3 GetSafeSpawnPosition(Vector3 basePos, float radius)
    {
        for (int i = 0; i < maxAttempts; i++)
        {
            // pick a random point in a circle around the spawner
            Vector3 candidate = basePos + Random.insideUnitSphere * radius;
            candidate.y = basePos.y; // keep on same ground level

            // if no collider overlaps this point, we can use it
            if (!Physics.CheckSphere(candidate, checkRadius))
            {
                return candidate;
            }
        }

        // fallback: just return base position if no safe spot found
        return basePos;
    }

    // optional: draw gizmos in editor for clarity
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
