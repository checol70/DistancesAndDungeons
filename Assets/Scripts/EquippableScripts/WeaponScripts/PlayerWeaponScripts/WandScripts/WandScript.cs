using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WandScript : WeaponScript {

    protected override void SetUpBaseClass()
    {
        
    }

    public abstract void Effect(GameObject target);
}
