using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StrengthShowerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<Text>().text = gameObject.transform.root.gameObject.GetComponent<SpecificCharacterScript>().BaseStrength.ToString();
    }
	
	
}
