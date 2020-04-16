using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingScript : RangeScript {

    protected override void SetUpSpecifics()
    {
        BaseCost = 4.5f;
        SpecialRules.Add(SpecialRulesEnum.DisarmingCrits);
        
        WeaponVariation = "Sling";
        BaseRange = 20;
    }
}
