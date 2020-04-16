using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSliderScript : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        gameObject.transform.root.gameObject.GetComponent<HealthScript>().HealthIndicator = gameObject.GetComponent<Slider>();
		
	}

}	
	
