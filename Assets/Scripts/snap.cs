using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snap : MonoBehaviour
{
    public bool click;

    public string etiqueta;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == etiqueta)
        {
            if (click == false)
            {
               // transform.position = other.gameObject.transform.position;
                this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == etiqueta)
        {
            this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
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