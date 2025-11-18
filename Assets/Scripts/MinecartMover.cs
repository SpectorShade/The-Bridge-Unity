using UnityEngine;

public class MinecartMover : MonoBehaviour
{
    public float speed = 2f;
    public Transform goal;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // kinematic for controlled motion
    }

    void FixedUpdate() {
        if (!goal) return;
        
        Vector3 newPos = Vector3.MoveTowards(transform.position, goal.position, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
    }
}
