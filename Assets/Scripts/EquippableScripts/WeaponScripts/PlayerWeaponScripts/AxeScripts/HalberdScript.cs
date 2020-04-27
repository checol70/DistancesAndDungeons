using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalberdScript : AxeScript {

    protected override void SetUpSpecifics()
    {
        axeType = "Halberd";

        BaseCost = 7;
        SpecialRules.Add(SpecialRulesEnum.Brace);
        SpecialRules.Add(SpecialRulesEnum.Brace);

        WeaponVariation = "Halberd";
        BaseRange = 4.5f;
        
    }
}
