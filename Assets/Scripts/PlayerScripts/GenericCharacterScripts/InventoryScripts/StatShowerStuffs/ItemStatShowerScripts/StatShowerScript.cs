using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class StatShowerScript : MonoBehaviour {

    public GameObject
        ItemToShow;
    public Text
        TitleObject,
        RangeObject,
        AverageDamageObject,
        AverageCostObject,
        RulesObject,
        AdvancedTooltipObject,
        ManaPenaltyObject,
        ManaCostObject;
    public GameObject
        WeaponStatShower;
    public Vector3[]
        corners;
    public Vector3
        pos;
    public float 
        width, 
        height,
        distPastX,
        distPastY;
    List<SpecialRulesEnum> rulesCovered = new List<SpecialRulesEnum>();

    private void Awake()
    {
        gameObject.transform.position = Input.mousePosition + new Vector3(95, -85, 0);
    }
    private void Update()
    {
        {
            pos = Input.mousePosition;

            var corners = gameObject.GetComponent<RectTransform>().rect;
            width = corners.xMax - corners.xMin;
            height = corners.yMax - corners.yMin;

            distPastX = pos.x + width - Screen.width;
            if (distPastX > 0)
                pos = new Vector3(pos.x - distPastX, pos.y, pos.z);
            distPastY = pos.y - height;
            if (distPastY < 0)
                pos = new Vector3(pos.x, pos.y - distPastY, pos.z);
            pos.x += (width / 2);
            pos.y -= (height / 2);
            transform.position = pos;
        }
    }

    public IEnumerator ShowStats(GameObject ItemToShow)
    {
        yield return new WaitWhile(() => TitleObject == null);
        if (ItemToShow != null)
        {

            if (ItemToShow.GetComponent<WeaponScript>() != null)
            {
                if (ItemToShow.GetComponent<MagicScript>())
                {
                    ItemToShow.GetComponent<MagicScript>().UpdateTip();
                }

                var weaponScript = ItemToShow.GetComponent<WeaponScript>();
                if (String.IsNullOrEmpty(weaponScript.WeaponVariation))
                    TitleObject.text = weaponScript.WeaponCategory.ToString();
                else TitleObject.text = weaponScript.WeaponVariation;
                if(weaponScript.BaseRange !=0)
                    RangeObject.text = weaponScript.BaseRange.ToString();
                else
                {
                    RangeObject.gameObject.transform.parent.gameObject.SetActive(false);
                }
                var characterScript = gameObject.transform.root.GetComponent<CharacterScript>();
                if(weaponScript.BaseDamage != 0)
                {
                    
                        AverageDamageObject.text = characterScript.CalculateWeaponDamage(weaponScript).ToString();
                    
                }
                else
                {
                    AverageDamageObject.gameObject.transform.parent.gameObject.SetActive(false);
                }
                if (weaponScript.BaseCost != 0)
                {
                    AverageCostObject.text = weaponScript.BaseCost.ToString();
                }
                else
                {
                    AverageCostObject.gameObject.transform.parent.gameObject.SetActive(false);
                }
                if (weaponScript.SpecialRules.Count >= 1)
                {
                    for (int i = 0; i < weaponScript.SpecialRules.Count; i++)
                    {
                        if (weaponScript.SpecialRules[i] != SpecialRulesEnum.None)
                        {
                            if (!rulesCovered.Contains(weaponScript.SpecialRules[i]))
                            {
                                rulesCovered.Add(weaponScript.SpecialRules[i]);

                                if (i > 0)
                                {

                                    GameObject rule = Instantiate(RulesObject.gameObject, gameObject.transform);
                                    if (weaponScript.CalcRules(weaponScript.SpecialRules[i]) == 1)
                                    {
                                        rule.GetComponent<Text>().text = weaponScript.SpecialRules[i].ToString();
                                    }
                                    else
                                    {
                                        rule.GetComponent<Text>().text = weaponScript.SpecialRules[i].ToString() + " x" + weaponScript.CalcRules(weaponScript.SpecialRules[i]);
                                    }

                                    rule.transform.SetParent(gameObject.transform);
                                    rule.transform.SetSiblingIndex(gameObject.transform.childCount - 2);
                                }
                                else
                                {
                                    if (weaponScript.CalcRules(weaponScript.SpecialRules[i]) == 1)
                                    {
                                        RulesObject.GetComponent<Text>().text = weaponScript.SpecialRules[i].ToString();
                                    }
                                    else
                                    {
                                        RulesObject.GetComponent<Text>().text = weaponScript.SpecialRules[i].ToString() + " x" + weaponScript.CalcRules(weaponScript.SpecialRules[i]);
                                    }
                                }
                            }
                        }
                    }
                }
                if (weaponScript.Modifiers.Count > 1)
                {
                    for (int i = 0; i < weaponScript.Modifiers.Count; i++)
                    {
                        if (weaponScript.Modifiers[i] != SpecialRulesEnum.None && !weaponScript.SpecialRules.Contains(weaponScript.Modifiers[i]))
                        {
                            GameObject rule = Instantiate(RulesObject.gameObject, gameObject.transform);

                            rule.transform.GetComponent<Text>().text = weaponScript.SpecialRules.ToString();
                            rule.transform.SetSiblingIndex(gameObject.transform.childCount - 2);
                        }
                    }
                }
                if(weaponScript.SpecialRules.Count == 0 && weaponScript.Modifiers.Count == 0)
                {
                    RulesObject.gameObject.transform.parent.gameObject.SetActive(false);
                }
                weaponScript.CalculateManaPenalty();
                if(weaponScript.ManaPenalty > 0)
                {
                    ManaPenaltyObject.text = weaponScript.ManaPenalty.ToString();
                    

                    if ((characterScript.WeaponHeld != null && weaponScript.ManaPenalty > characterScript.ModifiedMaxMana + characterScript.WeaponHeld.GetComponent<WeaponScript>().ManaPenalty)
                        || characterScript.WeaponHeld == null && weaponScript.ManaPenalty > characterScript.ModifiedMaxMana)
                    {
                        ManaPenaltyObject.color = Color.red;
                    }
                }
                else
                {
                    ManaPenaltyObject.gameObject.transform.parent.gameObject.SetActive(false);
                }
                weaponScript.CalculateManaCost();
                if (weaponScript.ManaCost != 0)
                {
                    ManaCostObject.text = weaponScript.ManaCost.ToString();
                    if (weaponScript.ManaCost < 0)
                    {
                        ManaCostObject.color = Color.green;
                    }
                }
                else ManaCostObject.gameObject.transform.parent.gameObject.SetActive(false);
                if(weaponScript.enchantments.Count != 0)
                {
                    foreach (WeaponEnchantmentScript enchantment in weaponScript.enchantments)
                    {
                        GameObject enchantShower = Instantiate(Resources.Load("StatShower/EnchantmentsObject") as GameObject);
                        if (enchantShower.transform.childCount > 1)
                        {
                            enchantShower.transform.GetChild(1).GetComponent<EnchantmentShowerScript>().Show(enchantment);
                        }
                        else
                        {
                            enchantShower.transform.GetChild(0).GetComponent<EnchantmentShowerScript>().Show(enchantment);

                        }
                        enchantShower.transform.SetParent(gameObject.transform);
                        enchantShower.transform.SetAsLastSibling();
                    }
                }
                if (!string.IsNullOrEmpty(weaponScript.AdvancedTooltip))
                {
                    AdvancedTooltipObject.text = weaponScript.AdvancedTooltip;
                    AdvancedTooltipObject.transform.SetAsLastSibling();
                }
                else
                {
                    AdvancedTooltipObject.gameObject.SetActive(false);
                }
            }
            else if (ItemToShow.GetComponent<ConsumableScript>())
            {
                var consumableScript = ItemToShow.GetComponent<ConsumableScript>();
                TitleObject.text = consumableScript.Type.ToString();
                if (consumableScript.range == 0.0f)
                {
                    RangeObject.gameObject.transform.parent.gameObject.SetActive(false);
                }
                else
                {
                    RangeObject.text = consumableScript.range.ToString();
                }
                if (consumableScript.cost != 0)
                {
                    AverageCostObject.text = consumableScript.cost.ToString();
                }
                else
                {
                    AverageCostObject.gameObject.transform.parent.gameObject.SetActive(false);
                }
                if (consumableScript.damage == 0.0f)
                {
                    AverageDamageObject.gameObject.transform.parent.gameObject.SetActive(false);
                }
                else
                {
                    AverageDamageObject.text = consumableScript.damage.ToString();
                }
                

                RulesObject.gameObject.transform.parent.gameObject.SetActive(false);

                AdvancedTooltipObject.text = "Right click to consume. " + consumableScript.Explanation;

            }
            else if (ItemToShow.GetComponent<ArmorScript>())
            {

            }
        }
        else { 
            Destroy(gameObject);
        }
    }

	
}
