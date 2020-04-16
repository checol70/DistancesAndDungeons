using UnityEngine;
using System.Collections;

public class AlertAreaScript : MonoBehaviour {
    public GameObject
        TrespassingPlayer,
        MonsterAttachedTo;
    public GameObject 
        MainCamera;

    void Start()
    {
        MonsterAttachedTo = gameObject.transform.parent.gameObject;
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

	
    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject == MainCamera.GetComponent<CameraScript>().ActivePlayer)
    //    {
    //        MonsterAttachedTo.GetComponent<MonsterScript>().PlayerIsWithinAlertArea = true;
            
    //        TrespassingPlayer = MainCamera.GetComponent<CameraScript>().ActivePlayer;
    //    }
    //}
    //void OnTriggerStay(Collider other)
    //{
    //    if (other.gameObject == MainCamera.GetComponent<CameraScript>().ActivePlayer)
    //    {
    //        MonsterAttachedTo.GetComponent<MonsterScript>().PlayerIsWithinAlertArea = true;
      
    //        TrespassingPlayer = MainCamera.GetComponent<CameraScript>().ActivePlayer;
    //    }
    //}
    //void OnTriggerExit(Collider other)
    //{
    //    MonsterAttachedTo.GetComponent<MonsterScript>().PlayerIsWithinAlertArea = false;

    //}
}
