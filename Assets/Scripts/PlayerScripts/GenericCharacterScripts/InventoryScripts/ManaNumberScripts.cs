﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ManaNumberScripts : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.transform.root.gameObject.GetComponent<CharacterScript>().InventoryManaText = gameObject.GetComponent<Text>();	
	}
}
