using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveScreenScript : MonoBehaviour {

    public static GameObject SaveScreen;

    private void Awake()
    {
        if (SaveScreen == null)
        { SaveScreen = gameObject; }
        else if (SaveScreen != gameObject)
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        StartCoroutine(Saving());
    }

    // Use this for initialization
    void Start () {
        gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	IEnumerator Saving () {
        yield return new WaitUntil(() => !SaveFileScript.FinishedSaving.Values.Contains(false) || SaveFileScript.FinishedSaving.Count == 0);
        gameObject.SetActive(false);

    }
}
