using UnityEngine;
using System.Collections;

public class WeaponSlotScript : EquipSlotScript
{

    private GameObject
        weaponContained,
        WeaponSpawned;
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
            weaponContained = null;
            PreviousItem = null;
            previousManaPenalty = 0;
            WeaponSpawned = Resources.Load("WeaponMeshes/Fists") as GameObject;
        }
        else
        {
            weaponContained = gameObject.transform.GetChild(0).gameObject;
            if (weaponContained.GetComponent<WeaponScript>())
            {
                weaponType = weaponContained.GetComponent<WeaponScript>().WeaponCategory;
                weaponContained.GetComponent<WeaponScript>().CalculateManaPenalty();
                manaPenalty = weaponContained.GetComponent<WeaponScript>().ManaPenalty;

            }
            else
                Debug.Log("You forgot to put WeaponScript on this Item");
        }
        if (weaponType == WeaponType.Dagger || weaponType == WeaponType.GreatAxe || weaponType == WeaponType.SwordAndShield)
        {
            if (weaponContained.GetComponent<SwordAndShieldScript>())
            {
                string swordType = weaponContained.GetComponent<SwordAndShieldScript>().swordType;
                string shieldType = weaponContained.GetComponent<SwordAndShieldScript>().shieldType;
                WeaponSpawned = Resources.Load("WeaponMeshes/Sword/" + swordType) as GameObject;
            }
            else
            {
                string axeType = weaponContained.GetComponent<AxeScript>().axeType;
                WeaponSpawned = Resources.Load("WeaponMeshes/GreatAxe/" + axeType) as GameObject;
            }
        }
        else if (weaponType == WeaponType.Ranged)
        {
            WeaponSpawned = Resources.Load("WeaponMeshes/Ranged/" + weaponContained.GetComponent<WeaponScript>().WeaponVariation) as GameObject;
        }
        else if (weaponType == WeaponType.Magic)
        {
            Debug.Log(weaponContained.GetComponent<MagicScript>().DMagicType);
            WeaponSpawned = Resources.Load("WeaponMeshes/Magic/" + (weaponContained.GetComponent<MagicScript>().DMagicType)) as GameObject;
        }
        else Debug.Log("forgot to add mesh case");



        //WeaponSpawned = Instantiate(Resources.Load("WeaponMeshes/" + weaponType +"/") as GameObject);
        //WeaponSpawned.transform.SetParent(Player.GetComponent<CharacterScript>().WeaponHook.transform, false);
        Player.GetComponent<CharacterScript>().EquipNewWeapon();
    }

}
