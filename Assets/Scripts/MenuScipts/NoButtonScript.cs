using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoButtonScript : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        gameObject.GetComponent<Button>().onClick.AddListener(Close);
	}
	
	// Update is called once per frame
	void Close () {
        gameObject.transform.parent.parent.parent.gameObject.SetActive(false);
	}
}
