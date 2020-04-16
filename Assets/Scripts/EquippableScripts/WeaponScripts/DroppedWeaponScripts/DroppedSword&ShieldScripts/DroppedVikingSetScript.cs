using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedVikingSetScript : DroppedWeaponScript
{

	// Use this for initialization
	void Start () {
        WeaponVariation = "VikingSword&RoundShield";
        WeaponCategory = WeaponType.SwordAndShield;
    }
}
