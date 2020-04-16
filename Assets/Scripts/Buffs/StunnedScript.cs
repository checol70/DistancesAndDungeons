using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunnedScript : BuffScript {
    public float originalMovement;
    public override void SetUp(int strong, int time)
    {
        if (!gameObject.GetComponent<SlowedScript>())
        {
            if (gameObject.GetComponent<MonsterScript>())
            {
                MonsterScript mon = gameObject.GetComponent<MonsterScript>();
                originalMovement = mon.MaxMovement;
                mon.MaxMovement = 0;
            }
            else if (gameObject.GetComponent<CharacterScript>())
            {
                CharacterScript chara = gameObject.GetComponent<CharacterScript>();
                originalMovement = chara.modifiedMaxMovement;
                chara.modifiedMaxMovement = 0;
            }
        }

        else
        {
            originalMovement = gameObject.GetComponent<SlowedScript>().origMovement;
            if (gameObject.GetComponent<MonsterScript>())
            {
                MonsterScript mon = gameObject.GetComponent<MonsterScript>();
                mon.MaxMovement = 0;
            }
            else if (gameObject.GetComponent<CharacterScript>())
            {
                CharacterScript chara = gameObject.GetComponent<CharacterScript>();
                chara.modifiedMaxMovement = 0;
            }
        }
    }
    public override void Remove()
    {
        if (!gameObject.GetComponent<SlowedScript>())
        {
            if (gameObject.GetComponent<MonsterScript>())
            {
                MonsterScript mon = gameObject.GetComponent<MonsterScript>();
                mon.MaxMovement = originalMovement;
            }
            else if (gameObject.GetComponent<CharacterScript>())
            {
                CharacterScript chara = gameObject.GetComponent<CharacterScript>();
                chara.modifiedMaxMovement = originalMovement;
            }
        }
        else
        {
            SlowedScript slow = gameObject.GetComponent<SlowedScript>();
            slow.SetUp(slow.strength, slow.duration);
        }
    }
    protected override void Decrease()
    {
        duration--;
        if (duration < 0)
            Remove();
    }
}
