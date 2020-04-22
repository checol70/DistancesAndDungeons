using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegeneratingScript : BuffScript {

	// Use this for initialization
	protected override void Decrease () {
        gameObject.GetComponent<HealthScript>().Healed(strength);
        duration--;
        if(duration <= 0)
        {
            Remove();
        }
	}
    public override void SetUp(int strong, int time)
    {
        strength = strong;
        duration = time;
        gameObject.GetComponent<HealthScript>().ShowBuff(Indicators.regeneration);
    }

    public override void Remove()
    {
        gameObject.GetComponent<HealthScript>().HideBuff(Indicators.regeneration);
        Destroy(this);
    }
}
