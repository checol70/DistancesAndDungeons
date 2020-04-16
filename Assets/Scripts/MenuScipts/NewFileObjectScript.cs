using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewFileObjectScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartMenuScript.startMenu.NewFileObject = gameObject;
        gameObject.SetActive(false);
	}
	
	
}
