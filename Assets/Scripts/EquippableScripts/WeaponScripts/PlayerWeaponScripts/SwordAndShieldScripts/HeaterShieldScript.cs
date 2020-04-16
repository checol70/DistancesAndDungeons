using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeaterShieldScript : SwordAndShieldScript
{
    

	protected override void SetUpSpecifics ()
    {
        BaseCost = 3f;
        WeaponVariation = "Knightly Sword & Shield";
        BaseRange = 2.5f;

        swordType = "ArmingSword";
        shieldType = "HeaterShield";
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
