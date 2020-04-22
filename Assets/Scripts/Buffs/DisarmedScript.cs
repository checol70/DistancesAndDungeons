using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisarmedScript : BuffScript {

    public int originalDamage;
    
    public override void SetUp(int strong, int time)
    {
        gameObject.GetComponent<HealthScript>().ShowBuff(Indicators.disarmed);
        strength += strong;
        duration += time;
        if (gameObject.GetComponent<MonsterScript>())
        {
            MonsterScript target = gameObject.GetComponent<MonsterScript>();

            if (originalDamage != 0)
            {
                originalDamage = target.AverageDamage;
            }
            target.AverageDamage = target.AverageDamage / strength + 1;
        }
        if (gameObject.GetComponent<CharacterScript>())
        {
            CharacterScript character = gameObject.GetComponent<CharacterScript>();
            if (character.EquippedWeapon != null)
            {
                character.AverageDamage = character.CalculateWeaponDamage(character.EquippedWeapon.GetComponent<WeaponScript>());

            }
        }
    }
    protected override void Decrease()
    {
        
    }

    public override void Remove()
    {
        if (gameObject.GetComponent<MonsterScript>())
        {
            MonsterScript target = gameObject.GetComponent<MonsterScript>();

            target.AverageDamage = originalDamage;

        }
        gameObject.GetComponent<HealthScript>().HideBuff(Indicators.attackBuff);
        Destroy(this);
    }
}
