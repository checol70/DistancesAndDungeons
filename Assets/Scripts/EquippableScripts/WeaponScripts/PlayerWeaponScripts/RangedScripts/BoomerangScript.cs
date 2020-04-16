using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangScript : RangeScript {

    protected override void SetUpSpecifics()
    {
        BaseCost = 4.5f;

        SpecialRules.Add(SpecialRulesEnum.StunningCrits);

        BaseRange = 20;

        WeaponVariation = "Boomerang";
    }
}
