using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnchantmentScript : MonoBehaviour {
    public int
            ManaPenalty,
            ManaCost,
            Strength;
    public EnchantmentType
        EnchantType;
    public DamageType
        damageItDeals;

}
