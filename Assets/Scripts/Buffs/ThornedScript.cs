using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornedScript : BuffScript {

    protected override void Decrease()
    {
        gameObject.GetComponent<HealthScript>().HideBuff(Indicators.thorned);
        duration--;
        if(duration <= 0)
        {
            Remove();
        }
    }
    public override void Remove()
    {
        Destroy(this);
    }
    public override void SetUp(int strong, int time)
    {
        strength = strong;
        duration = time;
        gameObject.GetComponent<HealthScript>().ShowBuff(Indicators.thorned);
    }
}
