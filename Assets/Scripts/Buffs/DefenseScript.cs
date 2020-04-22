using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseScript : BuffScript
{

    public override void Remove()
    {
        gameObject.GetComponent<HealthScript>().HideBuff(Indicators.defense);
        Destroy(this);
    }
    public override void SetUp(int strong, int time)
    {
        strength = strong;
        duration = time;
        gameObject.GetComponent<HealthScript>().ShowBuff(Indicators.defense);
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
