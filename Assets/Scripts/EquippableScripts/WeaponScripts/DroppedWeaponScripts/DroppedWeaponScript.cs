using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public abstract class DroppedWeaponScript : DroppedShinyScript
{
    public string WeaponVariation;
    public WeaponType
        WeaponCategory;
    public WeaponRarityEnum
        rarity;
    private List<SpecialRulesEnum> myRules = new List<SpecialRulesEnum>();
    int none,heavy;
    

    public void GenerateRules()
    {
        Material mat = gameObject.GetComponent<Renderer>().material;
        switch (rarity)
        {
            case WeaponRarityEnum.common:
                mat.color = new Color32(255, 255, 255, 255);
                break;
            case WeaponRarityEnum.uncommon:
                mat.color = new Color32(0, 255, 0, 255);
                break;
            case WeaponRarityEnum.rare:
                mat.color = new Color32(0, 0, 255, 255);
                break;
            case WeaponRarityEnum.legendary:
                mat.color = new Color32(128, 0, 128, 255);
                break;
            case WeaponRarityEnum.epic:
                mat.color = new Color32(255, 92, 0, 255);
                break;
            case WeaponRarityEnum.relic:
                mat.color = new Color32(255, 255, 0, 255);
                break;
            default:
                Debug.Log("Unhandled rarity at line 42 of dropped weaponscript");
                break;
        }
        for (int i = 0; i < (int)rarity; i++)
        {
            AssignStat();
        }
    }

    public override void PickedUp()
    {
        ActivePlayer = CameraScript.GameController.ActivePlayer;
        if (Vector3.Distance(gameObject.transform.position, ActivePlayer.transform.position) <= 1.5f)
        {
            ActiveBackpack = ActivePlayer.GetComponent<CharacterScript>().Backpack;
            if (WeaponVariation != null)
            {
                Debug.Log("WeaponSprites/" + WeaponCategory.ToString() + "/" + WeaponVariation + "Sprite");
                CurrentItem = Instantiate(Resources.Load("WeaponSprites/" + WeaponCategory.ToString() + "/" + WeaponVariation + "Sprite") as GameObject);
                Image img = CurrentItem.GetComponent<Image>();
                Debug.Log(img);
                switch (rarity)
                {
                    case WeaponRarityEnum.common:
                        break;
                    case WeaponRarityEnum.uncommon:
                        img.color = new Color32(0,255,0,125);
                        break;
                    case WeaponRarityEnum.rare:
                        img.color = new Color32(0,0,255,125);
                        break;
                    case WeaponRarityEnum.legendary:
                        img.color = new Color32(128, 0, 128, 125);
                        break;
                    case WeaponRarityEnum.epic:
                        img.color = new Color32(255, 92, 0, 125);
                        break;
                    case WeaponRarityEnum.relic:
                        img.color = new Color32(255, 255, 0, 125);
                        break;
                    default:
                        img.color = Color.red;
                        break;
                }
            }
            if (ActivePlayer.GetComponent<CharacterScript>().WeaponSlot.transform.childCount == 0)
            {
                CurrentItem.transform.SetParent(ActivePlayer.GetComponent<CharacterScript>().WeaponSlot.transform, false);
                ActivePlayer.GetComponent<CharacterScript>().EquipNewWeapon();
                ActivePlayer.GetComponent<CharacterScript>().WeaponSlot.GetComponent<WeaponSlotScript>().EquipItem();
                ActivePlayer.GetComponent<CharacterScript>().MoveTo = new RaycastHit();
                Destroy(gameObject);
            }
            else
            {
                for (int i = 0; i < ActiveBackpack.transform.childCount; i++)
                {
                    if (ActiveBackpack.transform.GetChild(i) != null)
                    {
                        CurrentItemSlot = ActiveBackpack.transform.GetChild(i).gameObject;
                        if (CurrentItemSlot.transform.childCount == 0)
                        {
                            CurrentItem.transform.SetParent(CurrentItemSlot.transform);
                            CurrentItem.transform.localPosition = Vector3.zero;
                            Destroy(gameObject);
                            break;
                        }
                    }
                    else
                    {
                        ActivePlayer.GetComponent<HealthScript>().ShowDamageTaken("Inventory Full", DamageType.Physical);
                        break;
                    }
                }
            }
            CurrentItem.transform.localScale = Vector3.one;
            CurrentItem.GetComponent<WeaponScript>().AddSpecialRules(myRules.ToArray());
        }
        else ActivePlayer.GetComponent<HealthScript>().ShowDamageTaken("Too Far Away", DamageType.Physical);

    }


    // this puts the stat in the list if it meets our criteria
    protected void AssignStat()
    {
        // start by getting a random stat.
        SpecialRulesEnum pot = RollStat();

        // then test it against stuff.
        switch (pot)
        {
            case SpecialRulesEnum.None:
                if (none > 0)
                    AssignStat();
                else none++;
                break;
            case SpecialRulesEnum.Counter:
                if (WeaponCategory == WeaponType.Magic)
                {
                    AssignStat();
                    break;
                }
                else
                {
                    myRules.Add(pot);
                    break;
                }
            case SpecialRulesEnum.Brace:
                if (WeaponCategory == WeaponType.Magic)
                {
                    AssignStat();
                    break;
                }
                else
                {
                    myRules.Add(pot);
                    break;
                }
            case SpecialRulesEnum.Flexible:

                AssignStat();
                break;


            case SpecialRulesEnum.Block:
                if (WeaponCategory == WeaponType.Magic)
                {
                    AssignStat();
                    break;
                }
                else
                {
                    myRules.Add(pot);
                    break;
                }
            case SpecialRulesEnum.DisarmingCrits:
                if (WeaponCategory == WeaponType.Magic)
                {
                    AssignStat();
                    break;
                }
                else
                {
                    myRules.Add(pot);
                    break;
                }
            case SpecialRulesEnum.ShatteringCrits:
                if (WeaponCategory == WeaponType.Magic)
                {
                    AssignStat();
                    break;
                }
                else
                {
                    myRules.Add(pot);
                    break;
                }
            case SpecialRulesEnum.IncreasedCrits:
                if (WeaponCategory == WeaponType.Magic)
                {
                    AssignStat();
                    break;
                }
                else
                {
                    myRules.Add(pot);
                    break;
                }
            case SpecialRulesEnum.StunningCrits:
                if (WeaponCategory == WeaponType.Magic)
                {
                    AssignStat();
                    break;
                }
                else
                {
                    myRules.Add(pot);
                    break;
                }
            case SpecialRulesEnum.BetterCrits:
                if (WeaponCategory == WeaponType.Magic)
                {
                    AssignStat();
                    break;
                }
                else
                {
                    myRules.Add(pot);
                    break;
                }
            case SpecialRulesEnum.Lingering:
                if (WeaponCategory != WeaponType.Magic)
                {
                    AssignStat();
                    break;
                }
                else
                {
                    myRules.Add(pot);
                    break;
                }
            case SpecialRulesEnum.Prolonged:
                if (WeaponCategory != WeaponType.Magic)
                    AssignStat();
                else myRules.Add(pot);
                break;
            case SpecialRulesEnum.Light:
                if (!myRules.Contains(SpecialRulesEnum.Heavy) && !myRules.Contains(SpecialRulesEnum.Light))
                {
                    myRules.Add(pot);
                }
                else
                {
                    AssignStat();
                }

                break;
            case SpecialRulesEnum.Heavy:
                if (!myRules.Contains(SpecialRulesEnum.Light) && !myRules.Contains(SpecialRulesEnum.ExtraDamage) && heavy > 3)
                {
                    myRules.Add(SpecialRulesEnum.ExtraDamage);
                    myRules.Add(pot);
                }
                else
                {
                    heavy++;
                    AssignStat();
                }
                break;
            default:
                AssignStat();
                break;
        }
    }

    // this returns a special rule enum so that we can use it later.
    protected SpecialRulesEnum RollStat()
    {
        SpecialRulesEnum[] enumValues = SpecialRulesEnum.GetValues(typeof(SpecialRulesEnum)) as SpecialRulesEnum[];
        int e = UnityEngine.Random.Range(0, enumValues.Length);
        SpecialRulesEnum rule = enumValues[e];
        return rule;
    }
}
