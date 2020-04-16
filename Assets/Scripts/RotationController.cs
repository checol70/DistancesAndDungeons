using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationController : MonoBehaviour {

	
	
	// Update is called once per frame
	void LateUpdate () {
        gameObject.transform.eulerAngles = Vector3.zero;
	}
}
