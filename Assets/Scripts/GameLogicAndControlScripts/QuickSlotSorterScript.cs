using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotSorterScript : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        gameObject.transform.parent.gameObject.GetComponent<CharacterSelectButtonScript>().QuickSlotSorter = gameObject;
		
	}
	
	
}
