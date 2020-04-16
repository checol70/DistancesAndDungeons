using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ReturnButtonScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<Button>().onClick.AddListener(CloseMenu);
	}
	
	// Update is called once per frame
	public void CloseMenu () {
        gameObject.transform.parent.gameObject.SetActive(false);
        CameraScript.InventoryOpen = false;
	}
}
