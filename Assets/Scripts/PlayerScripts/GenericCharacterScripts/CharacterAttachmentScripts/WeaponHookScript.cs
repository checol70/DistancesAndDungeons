using UnityEngine;
using System.Collections;

public class WeaponHookScript : MonoBehaviour {

	// Use this for initialization
	void Awake ()
    {
        gameObject.transform.root.gameObject.GetComponent<CharacterScript>().WeaponHooks.Add(gameObject.transform);
    }
}
