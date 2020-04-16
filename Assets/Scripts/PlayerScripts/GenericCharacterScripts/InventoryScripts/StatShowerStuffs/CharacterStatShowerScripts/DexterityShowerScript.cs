using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DexterityShowerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<Text>().text = gameObject.transform.root.gameObject.GetComponent<SpecificCharacterScript>().BaseDexterity.ToString();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
