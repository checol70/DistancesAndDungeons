using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class RespawningBuffScript : BuffScript {

    private Text timer;
    private void Update()
    {
        Debug.Log(duration.ToString());
    }
    protected override void Decrease()
    {
        if (DungeonScript.CurrentDungeon.BossDeceased)
        {
            gameObject.GetComponent<MonsterHealthScript>().Die();
        }
        timer.text = duration.ToString();
    }
    public override void Remove()
    {
        //here is where it will get back up and start fighting on the next turn.
            gameObject.GetComponent<MonsterScript>().timer.text = duration.ToString();
            timer.gameObject.SetActive(false);
            gameObject.GetComponent<Collider>().enabled = true;
            gameObject.GetComponent<NavMeshObstacle>().enabled = true;
            Collider[] colliders = gameObject.GetComponentsInChildren<Collider>();
            foreach (Collider col in colliders)
            {
                col.enabled = true;
            }
            Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer ren in renderers)
            {
                ren.enabled = true;
            }
            gameObject.GetComponent<MonsterScript>().HealthCanvas.SetActive(true);
            gameObject.GetComponent<MonsterScript>().enabled = true;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
        timer.gameObject.SetActive(false);
        LevelUp();
        MonsterHealthScript myHealth = gameObject.GetComponent<MonsterHealthScript>();
        myHealth.CurrentHP = myHealth.MaxHp;
        myHealth.stillFunctioning = true;
        myHealth.ShowDamageTaken(myHealth.MaxHp.ToString(), DamageType.Positive);
        myHealth.HealthIndicator.value = myHealth.CurrentHP;

        Destroy(this);
    }
    private void LevelUp()
    {
        int kind = UnityEngine.Random.Range(0, 3);
        switch (kind)
        {
            case 0:
                gameObject.GetComponent<MonsterHealthScript>().MaxHp += 5;
                break;
            case 1:
                gameObject.GetComponent<MonsterScript>().AverageDamage += 5;
                break;
            default:
                gameObject.GetComponent<MonsterScript>().AverageDamage += 5;
                gameObject.GetComponent<MonsterHealthScript>().MaxHp += 5;
                break;
        }
    }
    public override void SetUp(int strong, int time)
    {
        duration = time;
        timer = gameObject.GetComponent<MonsterScript>().timer;
        timer.text = duration.ToString();
        timer.gameObject.SetActive(true);
        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.GetComponent<NavMeshObstacle>().enabled = false;
        gameObject.GetComponent<Rigidbody>().isKinematic = true ;
        gameObject.GetComponent<MonsterScript>().enabled = false;
        Collider[] colliders = gameObject.GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
        foreach(Renderer ren in renderers)
        {
            ren.enabled = false;
        }
        gameObject.GetComponent<MonsterScript>().HealthCanvas.SetActive(false);
    }

}
