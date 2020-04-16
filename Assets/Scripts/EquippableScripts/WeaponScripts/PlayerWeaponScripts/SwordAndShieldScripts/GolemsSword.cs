using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemsSword : SwordAndShieldScript {

    protected override void SetUpSpecifics()
    {
        BaseCost = 4f;
        SpecialRules.Add(SpecialRulesEnum.IncreasedCrits);

        WeaponVariation = "Golem's Sword";
        BaseRange = 2.5f;

        swordType = "Scimitar";
        shieldType = "KiteShield";

        
    }
    

}
