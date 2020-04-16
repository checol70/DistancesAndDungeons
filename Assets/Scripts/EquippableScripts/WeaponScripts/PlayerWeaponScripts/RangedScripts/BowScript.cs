using UnityEngine;
using System.Collections;
using System;

public class BowScript : RangeScript {


    protected override void SetUpSpecifics()
    {
        BaseCost = 3;
        WeaponVariation = "Bow";
        BaseRange = 30;
    }
}
