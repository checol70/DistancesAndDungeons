using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAndExitObjectScript : MonoBehaviour {

    public static GameObject saveAndExitObject;

	// Use this for initialization
	void Start () {
		if(saveAndExitObject == null)
        {
            saveAndExitObject = gameObject;
        }
        else if (saveAndExitObject != gameObject)
        {
            Destroy(gameObject);
        }
        gameObject.SetActive(false);
	}
	
	
}
