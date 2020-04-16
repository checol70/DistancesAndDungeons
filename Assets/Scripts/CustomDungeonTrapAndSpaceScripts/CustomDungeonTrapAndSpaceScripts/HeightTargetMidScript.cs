using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightTargetMidScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.transform.parent.parent.parent.gameObject.GetComponent<StairTrapScript>().HeightTargetMid = gameObject;
	}
	
	
}
