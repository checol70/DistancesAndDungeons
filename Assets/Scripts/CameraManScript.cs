using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManScript : MonoBehaviour {

	
	
	// Update is called once per frame
	void Update () {
		if(CameraScript.GameController.ActivePlayer != null)
        {
            gameObject.transform.SetParent (CameraScript.GameController.ActivePlayer.transform,false);
        }
	}
}
