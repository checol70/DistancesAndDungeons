using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeardedAxeScript : AxeScript {

    protected override void SetUpSpecifics()
    {
        axeType = "BeardedAxe";

        BaseCost = 4.5f;
        SpecialRules.Add(SpecialRulesEnum.ShatteringCrits);

        

        WeaponVariation = "Bearded Axe";
        BaseRange = 2.5f;
    }
}
