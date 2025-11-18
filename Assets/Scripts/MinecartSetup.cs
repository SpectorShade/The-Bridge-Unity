using UnityEngine;

public class MinecartSetup : MonoBehaviour
{
    [SerializeField] private Transform[] respawnPoints;

    private void Start()
    {
        PickupRespawner[] pickups = GetComponentsInChildren<PickupRespawner>();

        for (int i = 0; i < pickups.Length && i < respawnPoints.Length; i++)
        {
            pickups[i].SetRespawnPoint(respawnPoints[i]);
        }
    }
}
