﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningGemScript : MonoBehaviour {

	
	
	// Update is called once per frame
	void FixedUpdate () {
        gameObject.transform.Rotate(Vector3.forward);
	}
}
