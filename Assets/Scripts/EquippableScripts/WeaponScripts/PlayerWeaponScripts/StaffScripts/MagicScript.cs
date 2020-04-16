using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MagicScript : WeaponScript {

    protected int 
        strength;
    protected int
        cost;
    protected CharacterScript
        character;
    public string 
        // d for debug Magic type so that we can load from resources.
        DMagicType;


    protected override void SetUpBaseClass()
    {
        WeaponCategory = WeaponType.Magic;

        IsMelee = false;

        HandsRequired = ItemType.TwoHandedWeapon;

        gameObject.GetComponent<DragAndDropScript>().itemType = ItemType.TwoHandedWeapon;

        WeaponStat = TypeOfStats.Intelligence;

        character = gameObject.transform.root.gameObject.GetComponent<CharacterScript>();
    }
    public abstract void UpdateTip();

    public abstract void Effect(GameObject target);
}
