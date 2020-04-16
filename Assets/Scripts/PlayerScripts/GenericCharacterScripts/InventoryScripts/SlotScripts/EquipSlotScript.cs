using UnityEngine;
using System.Collections;

public abstract class EquipSlotScript : ItemSlotScript {
    protected GameObject
        PreviousItem;
    public GameObject
        Player;
    protected int
        previousManaPenalty,
        manaPenalty;
    public abstract void EquipItem();

    public bool HasEnoughMana(GameObject equippingItem)
    {
        manaPenalty = equippingItem.GetComponent<WeaponScript>().ManaPenalty;
        if (Player.GetComponent<CharacterScript>().ModifiedMaxMana + previousManaPenalty >= manaPenalty)
        {
            previousManaPenalty = manaPenalty;
            return true;
        }
        else
        return false;
    }
}
