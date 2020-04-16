using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventoryCurrentManaBarScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.transform.root.gameObject.GetComponent<CharacterScript>().InventoryCurrentManaSlider = gameObject.GetComponent<Slider>();
	}
	
}
