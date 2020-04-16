using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VikingRoundShieldScript : SwordAndShieldScript {

	protected override void SetUpSpecifics ()
    {
        BaseCost = 4.5f;
        SpecialRules.Add(SpecialRulesEnum.Counter);

        WeaponVariation = "Viking Sword & Round Shield";
        BaseRange = 2.25f;

        swordType = "VikingSword";
        shieldType = "VikingShield";
	}
	
	
}
