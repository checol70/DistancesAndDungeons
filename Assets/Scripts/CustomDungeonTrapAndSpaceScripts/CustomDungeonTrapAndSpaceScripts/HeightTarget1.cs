﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightTarget1 : MonoBehaviour {

	// Use this for initialization
	void Start () {

        gameObject.transform.parent.parent.parent.gameObject.GetComponent<StairTrapScript>().HeightTarget1 = gameObject;
    }
	
	
}
