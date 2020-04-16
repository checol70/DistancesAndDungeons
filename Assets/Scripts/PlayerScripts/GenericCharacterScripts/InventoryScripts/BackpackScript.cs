using UnityEngine;
using System.Collections;

public class BackpackScript : MonoBehaviour {

	// Use this for initialization
	void Awake ()
    {
        gameObject.transform.root.gameObject.GetComponent<CharacterScript>().Backpack = gameObject;
	}
}
