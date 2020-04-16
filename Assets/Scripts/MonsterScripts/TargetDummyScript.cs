using UnityEngine;
using System.Collections;
using System;

public class TargetDummyScript : MonsterScript
{
    private float
        maxMovement;
    public int 
        MaxHealth;

    protected override void Setup()
    {
        MaxHealth = 50 + MonsterLevel - 1;
        maxMovement = 9f;
        MaxMovement = maxMovement;
        gameObject.GetComponent<HealthScript>().MaxHp = MaxHealth;
        gameObject.GetComponent<HealthScript>().IsReady();
        monsterType = MonsterTypes.TargetDummy;
    }
    protected override void ChooseWeapon(WeaponType wt)
    {
        switch (wt)
        {
            case WeaponType.Dagger:
                {
                    Instantiate(Resources.Load("MonsterWeapons/TargetDummy/Dagger") as GameObject, WeaponHook);

                    AttackCost = 3f;
                    AverageDamage = 5 + MonsterLevel -1;
                    Range = 1.3f;

                    break;
                }
            case WeaponType.GreatAxe:
                {
                    Instantiate(Resources.Load("MonsterWeapons/TargetDummy/Axe") as GameObject, WeaponHook);

                    AttackCost = 4.5f;
                    AverageDamage = 10 + 2*(MonsterLevel -1);
                    Range = 3f;

                    break;
                }
            case WeaponType.Ranged:
                {
                    Instantiate(Resources.Load("MonsterWeapons/TargetDummy/Ranged") as GameObject, WeaponHook);

                    AttackCost = 7f;
                    AverageDamage = 5 + MonsterLevel -1;
                    Range = 40f;

                    break;
                }
            case WeaponType.SwordAndShield:
                {
                    Instantiate(Resources.Load("MonsterWeapons/TargetDummy/Sword") as GameObject, WeaponHook);

                    AttackCost = 3f;
                    AverageDamage = 5 + MonsterLevel-1;

                    Range = 2f;
                    dR = MonsterLevel;

                    break;
                }
            default:
                {
                    Instantiate(Resources.Load("MonsterWeapons/TargetDummy/Sword") as GameObject, WeaponHook);

                    AttackCost = 3f;
                    AverageDamage = 5 + MonsterLevel - 1;

                    dR = MonsterLevel;
                    Range = 2f;

                    break;
                }
        }
    }
    public override void DropLoot()
    {
        
    }
}