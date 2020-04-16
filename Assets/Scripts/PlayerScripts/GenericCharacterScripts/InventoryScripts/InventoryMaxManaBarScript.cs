using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMaxManaBarScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.transform.root.gameObject.GetComponent<CharacterScript>().InventoryMaxManaSlider = gameObject.GetComponent<Slider>();
	}
	
	
}
