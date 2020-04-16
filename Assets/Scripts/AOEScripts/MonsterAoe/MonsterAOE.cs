using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterAOE : AOEScript {
    private void Start()
    {
        gameObject.layer = 16;
        gameObject.tag = "AOE";
        if(gameObject.GetComponent<SphereCollider>() == null)
        {
            gameObject.AddComponent<SphereCollider>();
        }
        gameObject.GetComponent<SphereCollider>().isTrigger = true;
        if(gameObject.GetComponent<Rigidbody>() == null)
        {
            gameObject.AddComponent<Rigidbody>();
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            gameObject.GetComponent<Rigidbody>().useGravity = false;
        }
    }
    protected override void StartingPlayerTurn()
    {
        
    }
    protected override void EndingPlayerTurn()
    {
        Effect();
        TurnsRemaining--;
        TargetsAffected.Clear();
        if(TurnsRemaining <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            TargetsAffected.Add(other.gameObject);
        }
        else if (other.gameObject.CompareTag("AOE"))
        {
            if (IsOpposedElement(other.gameObject.GetComponent<AOEScript>().damageType))
            {
                Destroy(gameObject);
            }
        }
    }
}
