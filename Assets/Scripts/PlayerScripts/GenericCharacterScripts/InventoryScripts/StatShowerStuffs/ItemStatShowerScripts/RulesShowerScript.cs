using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RulesShowerScript : MonoBehaviour {

    // Use this for initialization
    void Awake()
    {
        if (gameObject.transform.parent.parent.gameObject.GetComponent<StatShowerScript>() != null)
        {
            gameObject.transform.parent.parent.gameObject.GetComponent<StatShowerScript>().RulesObject = gameObject.GetComponent<Text>();
        }
    }
	// Update is called once per frame
	void Update () {
		
	}
}
