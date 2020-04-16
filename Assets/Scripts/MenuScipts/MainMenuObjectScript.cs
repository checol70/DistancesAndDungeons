using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuObjectScript : MonoBehaviour {

    public static GameObject mainMenuObject;

	// Use this for initialization
	void Awake () {
		if(mainMenuObject == null)
        {
            mainMenuObject = gameObject;
        }
        gameObject.SetActive(false);
	}
	
	
}
