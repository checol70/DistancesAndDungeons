using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseScript : BuffScript
{

    public override void SetUp(int strong, int time)
    {
        strength = strong;
        duration = time;
    }
    public override void Remove()
    {
        Destroy(this);
    }
    protected override void Decrease()
    {
        duration--;
        if(duration <= 0)
        {
            Remove();
        }
    }
    public int ReduceDamage()
    {
        Decrease();
        return strength;
    }
}
