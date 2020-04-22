using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatteredScript : BuffScript {

    public override void SetUp(int strong, int time)
    {
        strength += strong;
        duration += time;
        gameObject.GetComponent<HealthScript>().ShowBuff(Indicators.defenseShattered);
    }

    public override void Remove()
    {
        gameObject.GetComponent<HealthScript>().HideBuff(Indicators.defenseShattered);
        Destroy(this);
    }
    protected override void Decrease()
    {

    }
}
