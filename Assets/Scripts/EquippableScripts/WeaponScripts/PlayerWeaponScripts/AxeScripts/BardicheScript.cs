using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BardicheScript : AxeScript {

    protected override void SetUpSpecifics()
    {
        axeType = "Bardiche";

        BaseCost = 4.5f;
        SpecialRules.Add(SpecialRulesEnum.IncreasedCrits);

        WeaponVariation = "Bardiche";
        BaseRange = 3f;
    }
}
