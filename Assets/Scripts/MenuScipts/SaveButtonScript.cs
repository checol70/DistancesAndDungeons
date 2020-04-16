using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveButtonScript : MonoBehaviour {

    public bool HasBeenAssigned;


	// Use this for initialization
	void Start () {
        if (SaveFileScript.SaveSelected != null)
        {
            gameObject.GetComponent<Button>().onClick.AddListener(SaveFileScript.SaveSelected.Save);
            HasBeenAssigned = true;
        }
        else StartCoroutine(WaitForSaveFile()); 
	}
	
	IEnumerator WaitForSaveFile()
    {
        yield return new WaitWhile(() => SaveFileScript.SaveSelected == null);
        gameObject.GetComponent<Button>().onClick.AddListener(SaveFileScript.SaveSelected.Save);
        HasBeenAssigned = true;
    }
}
