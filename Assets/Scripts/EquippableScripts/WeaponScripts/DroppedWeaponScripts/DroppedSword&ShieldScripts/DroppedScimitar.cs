using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedScimitar : DroppedWeaponScript {

	// Use this for initialization
	void Start () {
        WeaponVariation = "Scimitar&KiteShield";
        WeaponCategory = WeaponType.SwordAndShield;
	}
}
