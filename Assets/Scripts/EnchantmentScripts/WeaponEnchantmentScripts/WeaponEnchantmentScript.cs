using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponEnchantmentScript : EnchantmentScript {

    protected CharacterScript
        character;
    
    // Use this for initialization
    void Start () {
        Debug.Log(gameObject);
        Debug.Log(gameObject.GetComponent<WeaponScript>());
        if (!gameObject.GetComponent<WeaponScript>().enchantments.Contains(this))
        { 
            gameObject.GetComponent<WeaponScript>().enchantments.Add(this);
        }
        gameObject.GetComponent<WeaponScript>().CalculateManaPenalty();
        character = gameObject.transform.root.gameObject.GetComponent<CharacterScript>();
        
        StartCoroutine(Ready());
	}
    public abstract IEnumerator Ready();

    public abstract void OnHit(int Damage,GameObject Target);
    
}
