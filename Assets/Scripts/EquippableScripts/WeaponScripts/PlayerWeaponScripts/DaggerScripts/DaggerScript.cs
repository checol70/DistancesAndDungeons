using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DaggerScript : WeaponScript {

	// Use this for initialization
	protected override void SetUpBaseClass () {
        WeaponCategory = WeaponType.Dagger;

        SpecialRules.Add(SpecialRulesEnum.IncreasedCrits);
        SpecialRules.Add(SpecialRulesEnum.BetterCrits);

        BaseDamage = 5;
        BaseRange = 1.1f;

        IsMelee = true;
        HandsRequired = ItemType.OneHandedWeapon;

        gameObject.GetComponent<DragAndDropScript>().itemType = ItemType.OneHandedWeapon;
        
    }
	
	
}
