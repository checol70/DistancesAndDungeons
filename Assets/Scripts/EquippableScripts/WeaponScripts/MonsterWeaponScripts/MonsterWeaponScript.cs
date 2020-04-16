using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterWeaponScript : WeaponScript {
    private MonsterScript
        monster;
	// Use this for initialization
	void Start ()
    {
        monster = gameObject.transform.root.gameObject.GetComponent<MonsterScript>();
        SettingUpInitialDamage();
        SettingUpInitialCost();
        SettingUpInitialCounterChance();
        SettingUpInitialBlockChance();
        SettingUpInitialWeaponVariation();
        SettingUpInitialSubstance();
        SettingUpInitialRange();
        SettingUpInitialWeaponType();
    }

    protected abstract void SettingUpInitialDamage();
    protected abstract void SettingUpInitialCost();
    protected abstract void SettingUpInitialCounterChance();
    protected abstract void SettingUpInitialBlockChance();
    protected abstract void SettingUpInitialWeaponVariation();
    protected abstract void SettingUpInitialSubstance();
    protected abstract void SettingUpInitialRange();
    protected abstract void SettingUpInitialWeaponType();



}
