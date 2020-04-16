using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitObjectScript : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        StartMenuScript.startMenu.ExitObject = gameObject;
        gameObject.SetActive(false);
	}
	
	
}
