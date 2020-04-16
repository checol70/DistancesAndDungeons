using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleGrabberScript : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Stepped On");
            other.gameObject.transform.SetParent(gameObject.transform);
        if (other.gameObject.CompareTag("Player")|| other.gameObject.CompareTag("Monster"))
        {
            if(other.gameObject.transform.parent.gameObject.GetComponent<TopGrabberScript>()== null)
                other.gameObject.transform.SetParent( gameObject.transform);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Stepped Off");
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Monster"))
        {
            if (other.gameObject.transform.parent.gameObject.GetComponent<TopGrabberScript>() == null)
                other.gameObject.transform.parent = null;
        }
    }
}
