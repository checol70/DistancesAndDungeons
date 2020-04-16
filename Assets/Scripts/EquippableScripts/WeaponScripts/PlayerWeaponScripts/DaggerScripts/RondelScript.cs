using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RondelScript : DaggerScript {

    protected override void SetUpSpecifics()
    {
        BaseCost = 3f;

        SpecialRules.Add(SpecialRulesEnum.IncreasedCrits);

        WeaponVariation = "Rondels";
    }
}
