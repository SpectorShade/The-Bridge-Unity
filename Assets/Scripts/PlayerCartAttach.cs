using UnityEngine;

public class PlayerCartAttach : MonoBehaviour
{
    public Transform minecart; // assign in inspector

    void Start()
    {
        transform.SetParent(minecart);
        transform.localPosition = Vector3.zero;  // seat position
        transform.localRotation = Quaternion.identity;
    }
}