using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterWeaponHook : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        gameObject.transform.root.gameObject.GetComponent<MonsterScript>().WeaponHook = gameObject.transform;
	}
	
}
