using UnityEngine;
using System.Collections;
using System;

public abstract class SwordAndShieldScript : WeaponScript {
    public String 
        swordType,
        shieldType;

    protected override void SetUpBaseClass() {
        WeaponCategory = WeaponType.SwordAndShield;
        BaseDamage = 10;

        IsMelee = true;
        HandsRequired = ItemType.OneHandedWeapon;

        gameObject.GetComponent<DragAndDropScript>().itemType = ItemType.OneHandedWeapon;
        SpecialRules.Add(SpecialRulesEnum.Block);

        WeaponStat = TypeOfStats.Strength;
    }
    
}
