using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitNoScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Button myButton = gameObject.GetComponent<Button>();
        if (myButton.onClick == null)
        {
            myButton.onClick.AddListener(StartMenuScript.startMenu.ReturnToMainMenu);
        }
    }
	
	
}
