using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour {

    public static MenuScript menuScript;

    private void Awake()
    {
        if(menuScript == null)
        {
            menuScript = this;
        }
        else if (menuScript != this)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {
        StartCoroutine(WaitForAssigns());
    }
    IEnumerator WaitForAssigns() {
        yield return new WaitForSeconds(.5f);
        gameObject.SetActive(false);
	}
	
	
}
