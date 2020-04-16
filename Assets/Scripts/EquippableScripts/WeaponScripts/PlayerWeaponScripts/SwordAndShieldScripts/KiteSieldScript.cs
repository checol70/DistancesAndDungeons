using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KiteSieldScript : SwordAndShieldScript
{

    protected override void SetUpSpecifics()
    {
        BaseCost = 4.5f;
        SpecialRules.Add(SpecialRulesEnum.IncreasedCrits);

        WeaponVariation = "Scimitar & Kite Shield";
        BaseRange = 2.5f;

        swordType = "Scimitar";
        shieldType = "KiteShield";
    }
}
