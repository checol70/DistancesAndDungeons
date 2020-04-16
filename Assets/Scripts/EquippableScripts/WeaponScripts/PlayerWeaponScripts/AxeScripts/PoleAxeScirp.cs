using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoleAxeScirp : AxeScript {

    protected override void SetUpSpecifics()
    {
        axeType = "PoleAxe";
        BaseCost = 4.5f;

        WeaponVariation = "Pole Axe";
        BaseRange = 3f;
    }
}
