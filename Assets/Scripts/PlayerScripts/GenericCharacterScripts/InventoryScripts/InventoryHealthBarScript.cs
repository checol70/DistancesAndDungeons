using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventoryHealthBarScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.transform.root.gameObject.GetComponent<CharacterScript>().InventoryHealthSlider = gameObject.GetComponent<Slider>();
	}
	
}
