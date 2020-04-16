using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedKnightsShieldScript : DroppedWeaponScript
{

	// Use this for initialization
	void Start () {
        WeaponVariation = "KnightlySword&Shield";
        WeaponCategory = WeaponType.SwordAndShield;
    }
}
