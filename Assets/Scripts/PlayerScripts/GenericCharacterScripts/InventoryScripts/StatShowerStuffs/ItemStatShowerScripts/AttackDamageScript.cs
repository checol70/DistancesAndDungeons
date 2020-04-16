using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackDamageScript : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        gameObject.transform.parent.parent.gameObject.GetComponent<StatShowerScript>().AverageDamageObject = gameObject.GetComponent<Text>();
    }
	
	
}
