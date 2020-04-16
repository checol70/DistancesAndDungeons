using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScutumScript : SwordAndShieldScript
{

    protected override void SetUpSpecifics()
    {
        BaseCost = 7f;
        SpecialRules.Add(SpecialRulesEnum.Block);

        WeaponVariation = "Gladius & Scutum";
        BaseRange = 2f;

        swordType = "Gladius";
        shieldType = "Scutum";
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
