using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RangeScript : WeaponScript{

	protected override void SetUpBaseClass()
    {
        IsMelee = false;
        WeaponCategory = WeaponType.Ranged;

        BaseDamage = 5;
        HandsRequired = ItemType.TwoHandedWeapon;

        gameObject.GetComponent<DragAndDropScript>().itemType = ItemType.TwoHandedWeapon;
        WeaponStat = TypeOfStats.Dexterity;
    }
}
