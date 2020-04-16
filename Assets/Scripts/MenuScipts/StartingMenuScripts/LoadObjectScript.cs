using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadObjectScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartMenuScript.startMenu.LoadFileObject = gameObject;
        gameObject.SetActive(false);
	}
	
	
}
