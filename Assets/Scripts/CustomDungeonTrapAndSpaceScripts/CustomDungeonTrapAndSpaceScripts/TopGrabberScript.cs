using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopGrabberScript : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Stepped On");
        if (other.gameObject.CompareTag("Player")|| other.gameObject.CompareTag("Monster"))
        {
            other.gameObject.transform.parent = gameObject.transform;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Stepped Off");
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Monster"))
        {
                other.gameObject.transform.parent = null;
        }
    }
}
