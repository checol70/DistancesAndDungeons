using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OPTestingAxe : AxeScript {

    protected override void SetUpSpecifics()
    {
        axeType = "Bardiche";

        BaseCost = 1f;
        for (int i = 0; i < 10; i++)
        {
            SpecialRules.Add(SpecialRulesEnum.IncreasedCrits);
            SpecialRules.Add(SpecialRulesEnum.ExtraDamage);
        }
        WeaponVariation = "Bardiche";
        BaseRange = 20f;
    }
}
