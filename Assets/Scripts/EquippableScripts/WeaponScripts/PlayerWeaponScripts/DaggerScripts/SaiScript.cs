using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaiScript : DaggerScript {

    protected override void SetUpSpecifics()
    {
        SpecialRules.Add(SpecialRulesEnum.DisarmingCrits);

        WeaponVariation = "Sais";

        BaseCost = 4.5f;
    }
}
