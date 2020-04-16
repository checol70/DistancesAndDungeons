using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveAndExitButtonScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<Button>().onClick.AddListener(SaveAndExit);
	}

    public void SaveAndExit()
    {
        SaveAndExitObjectScript.saveAndExitObject.SetActive(true);
    }
	
	
}
