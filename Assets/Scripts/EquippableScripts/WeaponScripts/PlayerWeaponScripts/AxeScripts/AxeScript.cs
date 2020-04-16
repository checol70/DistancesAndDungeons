using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AxeScript : WeaponScript {
    public string axeType;

    protected override void SetUpBaseClass()
    {
        WeaponCategory = WeaponType.GreatAxe;
        BaseDamage = 5;

        IsMelee = true;
        HandsRequired = ItemType.TwoHandedWeapon;

        gameObject.GetComponent<DragAndDropScript>().itemType = ItemType.TwoHandedWeapon;

        SpecialRules.Add(SpecialRulesEnum.BetterCrits);
        SpecialRules.Add(SpecialRulesEnum.ExtraDamage);

        WeaponStat = TypeOfStats.Strength;
    }
}
