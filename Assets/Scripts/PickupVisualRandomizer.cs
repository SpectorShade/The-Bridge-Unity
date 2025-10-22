using UnityEngine;

public class PickupVisualRandomizer : MonoBehaviour
{
    void Awake()
    {
        // Get all child models
        Transform[] models = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
            models[i] = transform.GetChild(i);

        // Disable all models
        foreach (Transform t in models)
            t.gameObject.SetActive(false);

        // Pick one randomly
        if (models.Length > 0)
        {
            int index = Random.Range(0, models.Length);
            models[index].gameObject.SetActive(true);
        }
    }
}
