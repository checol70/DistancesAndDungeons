using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronGolemScript : BossScript {
    
    protected override void Setup()
    {
        {
            BaseMaxHealth = 500;
            MaxMovement = 5.0f;
            Range = 2f;
            gameObject.GetComponent<HealthScript>().MaxHp = BaseMaxHealth;
            gameObject.GetComponent<HealthScript>().IsReady();
            MaxMovement = 5;
            AttackCost = 1.5f;
            AverageDamage = 5;
            bossWeapon = "GolemsSword";
            Defense = 2;
            RangeOffset = 1;
        }
    }
    protected override void ChooseWeapon(WeaponType wt)
    {
        
    }
    public override void DropLoot()
    {
        GameObject dW = Instantiate(Resources.Load("DroppedWeapons/SwordAndShield/Scimitar&KiteShield") as GameObject, gameObject.transform.position, gameObject.transform.rotation);
        dW.GetComponent<DroppedWeaponScript>().rarity = WeaponRarityEnum.relic;
        dW.GetComponent<DroppedWeaponScript>().GenerateRules();
    }
}
