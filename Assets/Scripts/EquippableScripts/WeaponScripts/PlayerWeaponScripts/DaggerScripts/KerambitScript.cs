using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KerambitScript : DaggerScript {

    protected override void SetUpSpecifics()
    {
        BaseCost = 3f;

        WeaponVariation = "Kerambits";

        SpecialRules.Add(SpecialRulesEnum.Counter);
    }
}
