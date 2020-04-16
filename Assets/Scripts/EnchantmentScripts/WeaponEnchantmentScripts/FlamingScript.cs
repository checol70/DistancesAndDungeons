using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamingScript : WeaponEnchantmentScript {

    public override void OnHit(int damage, GameObject target)
    {
        if (damage * 6 * Strength / 100 >= 1)
        {
            if (target != null)
            {
                if (character.CurrentMana >= ManaCost)
                {
                    target.GetComponent<HealthScript>().Damaged(damage * 12 / 100, DamageType.Fire);
                    character.SpendMana(ManaCost);
                }
            }
        }
        else if(damage * 6 * Strength / 100 < 1)
        {
            if (target != null)
            {
                if(character.CurrentMana >= ManaCost)
                {
                    target.GetComponent<HealthScript>().Damaged(1, DamageType.Fire);
                    character.SpendMana(ManaCost);
                }
            }
        }
    }


    public override IEnumerator Ready()
    {
        yield return new WaitWhile(() => Strength == 0);
        ManaCost = Strength;
        damageItDeals = DamageType.Fire;
        EnchantType = EnchantmentType.Flaming;
        ManaPenalty = Strength * 2;
    }
}
