using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStatShowerScript : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        gameObject.transform.parent.parent.gameObject.GetComponent<StatShowerScript>().WeaponStatShower = gameObject;
	}
	
}
