using UnityEngine;

public class snap : MonoBehaviour
{
    public bool click;

    public string etiqueta;

    private Rigidbody rb;
    
    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(etiqueta))
        {
            if (click == false)
            {
               // transform.position = other.gameObject.transform.position;
                rb.isKinematic = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(etiqueta))
        {
            rb.isKinematic = false;
        }
    }
    private void OnMouseDown()
    {
        click = true;
    }
    private void OnMouseUp()
    {
        click = false;
    }
}