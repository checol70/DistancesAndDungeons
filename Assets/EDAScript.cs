using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EDAScript : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == CameraScript.GameController.ActivePlayer)
        {
            gameObject.transform.root.gameObject.GetComponent<LocalExitScript>().PIA = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject == CameraScript.GameController.ActivePlayer)
        {
            gameObject.transform.root.gameObject.GetComponent<LocalExitScript>().PIA = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == CameraScript.GameController.ActivePlayer)
        {
            gameObject.transform.root.gameObject.GetComponent<LocalExitScript>().PIA = false;
        }
    }
}
