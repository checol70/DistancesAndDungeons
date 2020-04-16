using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnchantTitleShowerScript : MonoBehaviour {

    public static EnchantTitleShowerScript
        Enchant;

    private void Awake()
    {
        if (Enchant == null)
        {
            Enchant = this;
        }
        else if (Enchant != this)
        {
            Destroy(gameObject);
        }
    }
}
