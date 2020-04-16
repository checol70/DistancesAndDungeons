using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedConsumableScript : DroppedShinyScript {

    public ConsumableEnum ConsumableType;

    public int amount;

    public override void PickedUp()
    {
        //make it easy to reference the active player
        ActivePlayer = CameraScript.GameController.ActivePlayer;
        // make it easy to reference the backpack
        ActiveBackpack = ActivePlayer.GetComponent<CharacterScript>().Backpack;
        //if we are close enough to pick it up
        if (Vector3.Distance(gameObject.transform.position, ActivePlayer.transform.position) <= 1.5f)
        {
            //get the backpacks next item slot
            for (int a = 0; a < ActiveBackpack.transform.childCount; a++)
            {
                //if the itemslotexists
                if (ActiveBackpack.transform.GetChild(a) != null)
                {
                    //make it easy to reference the item slot
                    CurrentItemSlot = ActiveBackpack.transform.GetChild(a).gameObject;
                    //if the itemslot is filled
                    if (CurrentItemSlot.transform.childCount != 0)
                    {
                        //if the itemslot has our consumable
                        if (CurrentItemSlot.transform.GetChild(0).CompareTag(ConsumableType.ToString()))
                        {
                            //add the consumable to the stack in the inventory
                            CurrentItemSlot.transform.GetChild(0).GetComponent<ConsumableScript>().PickedUpConsumable();
                            Destroy(gameObject);
                            return;
                        }
                    }
                }

            }
            //if we don't find any already Existing consumables we create a new one
            CurrentItem = Instantiate(Resources.Load("ConsumableSprites/" + ConsumableType + "Sprite") as GameObject);
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
                            CurrentItem.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
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
        }
        else ActivePlayer.GetComponent<HealthScript>().ShowDamageTaken("Too Far Away", DamageType.Physical);

    }

}
