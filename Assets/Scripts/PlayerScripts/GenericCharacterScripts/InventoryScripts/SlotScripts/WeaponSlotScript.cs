using UnityEngine;
using System.Collections;

public class WeaponSlotScript : EquipSlotScript
{

    private GameObject
        WeaponContained,
        WeaponSpawned,
        OffhandSpawned;
    private WeaponType
        weaponType;
    void Awake()
    {
        Player = gameObject.transform.root.gameObject;
        Player.GetComponent<CharacterScript>().WeaponSlot = gameObject;
        AllowedItemType = ItemType.OneHandedWeapon;
        SecondaryAllowedItemType = ItemType.TwoHandedWeapon;
    }
    void Start()
    {
        AllowedItemType = ItemType.OneHandedWeapon;
        SecondaryAllowedItemType = ItemType.TwoHandedWeapon;
    }

    public override bool IsCorrectItemType(GameObject item)
    {
        var type = item.GetComponent<DragAndDropScript>().itemType;
        if (
                (
                    AllowedItemType == ItemType.Any
                    || AllowedItemType == type
                    || SecondaryAllowedItemType == type
                )
                && HasEnoughMana(item)
            )
        {
            return true;
        }
        else
            return false;
    }
    public override void EquipItem()
    {
        if (WeaponSpawned != null)
            Destroy(WeaponSpawned);
        if (gameObject.transform.childCount == 0)
        {
            weaponType = WeaponType.Fisticuffs;
        }
        else
        {
            WeaponContained = gameObject.transform.GetChild(0).gameObject;
            if (WeaponContained.GetComponent<WeaponScript>())
            {
                weaponType = WeaponContained.GetComponent<WeaponScript>().WeaponCategory;
                WeaponContained.GetComponent<WeaponScript>().CalculateManaPenalty();
                manaPenalty = WeaponContained.GetComponent<WeaponScript>().ManaPenalty;

            }
            else
                Debug.Log("You forgot to put WeaponScript on this Item");
        }
        switch (weaponType)
        {
            case WeaponType.Fisticuffs:
                weaponType = WeaponType.Fisticuffs;
                WeaponContained = null;
                PreviousItem = null;
                previousManaPenalty = 0;
                WeaponSpawned = Resources.Load("WeaponMeshes/Fists") as GameObject;
                break;
            case WeaponType.SwordAndShield:
                string swordType = WeaponContained.GetComponent<SwordAndShieldScript>().swordType;
                string shieldType = WeaponContained.GetComponent<SwordAndShieldScript>().shieldType;
                WeaponSpawned = Resources.Load("WeaponMeshes/Sword/" + swordType) as GameObject;
                OffhandSpawned = Resources.Load("WeaponMeshes/Shield/" + shieldType) as GameObject;
                break;
            case WeaponType.GreatAxe:
                string axeType = WeaponContained.GetComponent<AxeScript>().axeType;
                WeaponSpawned = Resources.Load("WeaponMeshes/GreatAxe/" + axeType) as GameObject;
                break;
            case WeaponType.Ranged:
                WeaponSpawned = Resources.Load("WeaponMeshes/Ranged/" + WeaponContained.GetComponent<WeaponScript>().WeaponVariation) as GameObject;
                break;
            case WeaponType.Magic:
                Debug.Log(WeaponContained.GetComponent<WeaponScript>().WeaponVariation);
                WeaponSpawned = Resources.Load("WeaponMeshes/Staff/" + (WeaponContained.GetComponent<WeaponScript>().WeaponVariation)) as GameObject;
                break;
            case WeaponType.Dagger:
                string daggertype = WeaponContained.GetComponent<DaggerScript>().WeaponVariation;
                WeaponSpawned = Resources.Load("WeaponMeshes/Dagger/" + daggertype) as GameObject;
                break;
            default:
                Debug.Log("forgot to add mesh case");
                break;
        }




        //WeaponSpawned = Instantiate(Resources.Load("WeaponMeshes/" + weaponType +"/") as GameObject);
        //WeaponSpawned.transform.SetParent(Player.GetComponent<CharacterScript>().WeaponHook.transform, false);
        if(OffhandSpawned != null)
        {
            Player.GetComponent<CharacterScript>().ShowWeapons(WeaponSpawned, OffhandSpawned);
        }
        else
        {
            Player.GetComponent<CharacterScript>().ShowWeapons(WeaponSpawned);
        }
        Player.GetComponent<CharacterScript>().EquipNewWeapon();
    }

}
