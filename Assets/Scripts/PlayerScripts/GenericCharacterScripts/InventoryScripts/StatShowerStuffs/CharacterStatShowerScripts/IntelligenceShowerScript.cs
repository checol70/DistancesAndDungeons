using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntelligenceShowerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<Text>().text = gameObject.transform.root.gameObject.GetComponent<SpecificCharacterScript>().BaseIntelligence.ToString();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
