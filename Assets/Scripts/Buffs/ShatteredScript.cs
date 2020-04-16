using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatteredScript : BuffScript {

    public override void SetUp(int strong, int time)
    {
        strength += strong;
        duration += time;
        if (indicator == null)
        {
            indicator = Instantiate(Resources.Load("ShatteredIndicator") as GameObject, gameObject.transform.root.gameObject.GetComponent<BuffReferenceScript>().BuffShower);
        }
    }

    public override void Remove()
    {
        Destroy(indicator);
        Destroy(this);
    }
    protected override void Decrease()
    {

    }
}
