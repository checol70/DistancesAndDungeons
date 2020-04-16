using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampiricScript : WeaponEnchantmentScript {

    
    private int
        HealedAmount;
    public override void OnHit(int DamageDealt, GameObject Target)
    {
        

        HealedAmount = DamageDealt * Strength * 2 / 100;
        Debug.Log(DamageDealt.ToString());

        if (character.gameObject.GetComponent<CharacterHealthScript>().CurrentHP < gameObject.transform.root.gameObject.GetComponent<CharacterHealthScript>().MaxHp && character.CurrentMana >= ManaCost)
        {
            Debug.Log("Heal Successful");
            character.SpendMana(ManaCost);

            character.gameObject.GetComponent<CharacterHealthScript>().Healed(HealedAmount);
        }
        else { Debug.Log("Already At full Health"); }
        Debug.Log("VampiricScript Works" + HealedAmount.ToString());

    }
    public override IEnumerator Ready()
    {
        yield return new WaitWhile(() => Strength == 0);
        ManaCost = Strength;
        damageItDeals = DamageType.Positive;
        EnchantType = EnchantmentType.Vampiric;
        ManaPenalty = Strength * 2;
    }

}
