using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossbowScript : RangeScript {

    protected override void SetUpSpecifics()
    {
        SpecialRules.Add(SpecialRulesEnum.ExtraDamage);
        SpecialRules.Add(SpecialRulesEnum.IncreasedCrits);

        BaseCost = 7;
        WeaponVariation = "Crossbow";
        BaseRange = 40;
    }
}
