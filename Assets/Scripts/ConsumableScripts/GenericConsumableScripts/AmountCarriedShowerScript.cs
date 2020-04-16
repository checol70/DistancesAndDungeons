using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmountCarriedShowerScript : MonoBehaviour {

	// Use this for initialization
	void Awake ()
    {
        gameObject.transform.parent.gameObject.GetComponent<ConsumableScript>().AmountShower = gameObject.GetComponent<Text>();
	}
	

}
