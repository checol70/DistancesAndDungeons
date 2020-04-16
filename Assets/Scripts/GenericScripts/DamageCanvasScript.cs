using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCanvasScript : MonoBehaviour {
    private Quaternion
        rotation;
	// Use this for initialization
	void Start () {
        gameObject.transform.root.gameObject.GetComponent<HealthScript>().DamageCanvas = gameObject;
        rotation = gameObject.transform.rotation;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        gameObject.transform.rotation = rotation;
	}
}
