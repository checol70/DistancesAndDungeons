using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHealthScript : HealthScript
{

    public int
        TurnsTillRespawn;
    public bool
        stillFunctioning = true;

    private void Respawn()
    {

    }

    public override IEnumerator Begin()
    {
        yield return new WaitWhile(() => HealthIndicator == null);
        HealthIndicator.maxValue = MaxHp;
        CurrentHP = MaxHp;
        HealthIndicator.value = CurrentHP;
    }
    public override void Healed(int amount)
    {
        Debug.Log(amount.ToString() + " Healed");
        //first make sure we don't overheal
        if (CurrentHP + amount >= MaxHp)
        {
            amount = CurrentHP - MaxHp;
        }
        //heal
        CurrentHP += amount;
        //show we healed
        ShowDamageTaken(amount.ToString(), DamageType.Positive);
    }
    protected override void Hit(int DamageTaken, GameObject Attacker, DamageType damageType)
    {
        if (gameObject.GetComponent<ShatteredScript>())
        {
            TotalDamageTaken = DamageTaken * gameObject.GetComponent<ShatteredScript>().strength + 2 / 2;

        }
        else TotalDamageTaken = DamageTaken;
        if (gameObject.GetComponent<MonsterScript>().Unarmed || gameObject.GetComponent<MonsterScript>().WeaponHeld == null)
        {
            Damaged(TotalDamageTaken, damageType);
            Attacker.GetComponent<CharacterScript>().IHitAMonster(DamageTaken, gameObject);
        }
    }
    public override void Damaged(int DamageTaken, DamageType damageType)
    {
        if (stillFunctioning)
        {
            if (DamageTaken > CurrentHP)
                DamageTaken = CurrentHP;
            CurrentHP = CurrentHP - DamageTaken;
            HealthIndicator.value = CurrentHP;

            ShowDamageTaken(DamageTaken.ToString(), damageType);
            if (CurrentHP <= 0)
            {

                // this is where the respawn timer will go
                if (gameObject != LevelScript.currentLevel.GetComponent<LevelScript>().boss)
                {
                    if (!DungeonScript.CurrentDungeon.BossDeceased)
                    {
                        foreach (BuffScript buff in gameObject.GetComponents<BuffScript>())
                        {
                            buff.Remove();
                        }
                        RespawningBuffScript res = gameObject.AddComponent<RespawningBuffScript>();
                        res.SetUp(1, 5);
                        stillFunctioning = false;
                    }
                    else
                    {
                        Die();
                    }
                }
                else
                {
                    DungeonScript.CurrentDungeon.BossDeceased = true;
                    Die();
                }
            }
        }
    }
    private IEnumerator Killed()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    
    public void Die()
    {
        gameObject.GetComponent<Collider>().enabled = false;
        //here is where we will put loot spawning/actually killing this monster.
        StartCoroutine(Killed());
        //loot gen goes here. lol, tricked myself

        gameObject.GetComponent<MonsterScript>().DropLoot();

        gameObject.GetComponent<Rigidbody>().isKinematic = false;
    }
}
