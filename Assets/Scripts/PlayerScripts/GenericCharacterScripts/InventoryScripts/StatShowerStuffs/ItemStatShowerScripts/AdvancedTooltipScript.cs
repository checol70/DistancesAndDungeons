using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdvancedTooltipScript : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        gameObject.transform.parent.gameObject.GetComponent<StatShowerScript>().AdvancedTooltipObject = gameObject.GetComponent<Text>();
	}
	
	
}
