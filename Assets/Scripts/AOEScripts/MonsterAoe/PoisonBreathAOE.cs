using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoisonBreathAOE : MonsterAOE {

    protected override void Effect()
    {
        foreach(GameObject target in TargetsAffected)
        {
            var targetHealth = target.GetComponent<HealthScript>();
            if(targetHealth != null)
            {
                targetHealth.Damaged(Damage, DamageType.Acid);
            }
        }
    }
    protected override bool IsOpposedElement(DamageType test)
    {
        if (test == DamageType.Electric)
        {
            return true;
        }
        else return false;
    }
}
