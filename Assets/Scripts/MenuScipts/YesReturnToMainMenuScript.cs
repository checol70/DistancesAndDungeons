using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class YesReturnToMainMenuScript : MonoBehaviour {



	// Use this for initialization
	void Start () {
        gameObject.GetComponent<Button>().onClick.AddListener(ReturnToMainMenu);
	}
	
	// Update is called once per frame
	public void ReturnToMainMenu () {
        SceneManager.LoadScene(0);
	}
}
