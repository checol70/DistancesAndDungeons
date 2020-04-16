using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossScript : MonsterScript {
    protected string bossWeapon;
	public void OnDeath()
    {
        DungeonScript.CurrentDungeon.BossDeceased = true;
        Instantiate(Resources.Load("DroppedWeapons/" +bossWeapon ) as GameObject);
        
    }
}
