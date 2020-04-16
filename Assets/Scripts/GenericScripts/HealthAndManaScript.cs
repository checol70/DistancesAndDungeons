using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthAndManaScript : MonoBehaviour {
    private Quaternion
        Rotation;
	// Use this for initialization
	void Start () {
        Rotation = gameObject.transform.rotation;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        gameObject.transform.rotation = Rotation;
	}
}
