using UnityEngine;
using System.Collections;

public class TemporaryCollisionIgnore : MonoBehaviour
{
    [SerializeField] private float disableDuration = 2f; // how long to ignore collisions
    private int objectLayer;

    void Start()
    {
        // Get the layer index for "Objects"
        objectLayer = LayerMask.NameToLayer("Objects");

        // Disable collisions within that layer
        Physics.IgnoreLayerCollision(objectLayer, objectLayer, true);

        // Re-enable them after a delay
        StartCoroutine(ReenableCollisions());
    }

    private IEnumerator ReenableCollisions()
    {
        yield return new WaitForSeconds(disableDuration);

        // Re-enable collisions between objects
        Physics.IgnoreLayerCollision(objectLayer, objectLayer, false);
    }
}
