using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBuffScript : BuffScript {
    
    protected override void Decrease()
    {
        
        
        
    }
    public void IncreaseDamage(int damage, GameObject target,GameObject player, DamageType dT)
    {
        duration--;
        
        target.GetComponent<HealthScript>().InitHit(damage + strength, gameObject, dT);
        if (duration <= 0)
            Remove();
    }
    public override void Remove()
    {
        gameObject.GetComponent<CharacterScript>().HideBuff(Indicators.attackBuff);
        Destroy(this);
    }
    public override void SetUp(int strong, int time)
    {
        strength = strong;
        duration = time;
        gameObject.GetComponent<CharacterScript>().ShowBuff(Indicators.attackBuff);
    }
}
