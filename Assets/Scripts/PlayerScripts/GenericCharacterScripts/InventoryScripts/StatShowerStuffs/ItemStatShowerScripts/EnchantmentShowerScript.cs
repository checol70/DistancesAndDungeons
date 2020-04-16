using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnchantmentShowerScript : MonoBehaviour
{

    public void Show(EnchantmentScript enchantment)
    {
        gameObject.GetComponent<Text>().text = enchantment.EnchantType + " " + enchantment.Strength.ToString();
        DamageType damageType = enchantment.damageItDeals;
        Color32 Type;
        switch (damageType)
        {
            case DamageType.Physical:
                {
                    Type = Color.white;
                    break;
                }
            case DamageType.Fire:
                {
                    Type = Color.red;
                    break;
                }
            case DamageType.Acid:
                {
                    Type = new Color32(128, 255, 0, 255);
                    break;
                }
            case DamageType.Ice:
                {
                    Type = new Color32(153, 255, 255, 255);
                    break;
                }
            case DamageType.Bleed:
                {
                    Type = new Color32(122, 0, 0, 255);
                    break;
                }
            case DamageType.Electric:
                {
                    Type = new Color32(255, 255, 51, 255);
                    break;
                }
            case DamageType.Positive:
                {
                    Type = new Color32(0, 255, 122, 255);
                    break;
                }
            case DamageType.Negative:
                {
                    Type = new Color32(155, 0, 155, 255);
                    break;
                }
            default:
                {
                    Type = Color.black;
                    Debug.Log("damageType not recognized");
                    break;
                }
        }

        gameObject.GetComponent<Text>().color = Type;
    }
}
