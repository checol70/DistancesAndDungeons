﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHealthCanvasScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.transform.root.gameObject.GetComponent<MonsterScript>().HealthCanvas = gameObject;
	}
}
