using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScript : MonoBehaviour {
	
	void Start ()
    {
        gameObject.transform.parent.gameObject.GetComponent<StatShowerScript>().TitleObject = gameObject.GetComponent<Text>();
        gameObject.GetComponent<Text>().text = null;
        
        
	}


}
