using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public abstract class ItemSlotScript : MonoBehaviour, IDropHandler
{
    public ItemType AllowedItemType;
    public ItemType SecondaryAllowedItemType;


    public void OnDrop(PointerEventData eventData)
    {
        if (DragAndDropScript.DraggedItem != null)
        {
            if (CameraScript.InventoryOpen)
            {
                GameObject otherSlot = DragAndDropScript.DraggedItem.GetComponent<DragAndDropScript>().PreviousSlot.gameObject;
                if (IsCorrectItemType(DragAndDropScript.DraggedItem))
                {
                    if (gameObject.transform.childCount == 0)
                    {
                        DragAndDropScript.DraggedItem.transform.SetParent(gameObject.transform);
                        DragAndDropScript.DraggedItem.transform.localPosition = Vector3.zero;
                    }

                    if (gameObject.transform.childCount != 0)
                    {
                        GameObject CurrentItem = gameObject.transform.GetChild(0).gameObject;
                        ItemType CurrentItemType = gameObject.GetComponentInChildren<DragAndDropScript>().itemType;
                        if (otherSlot.GetComponent<ItemSlotScript>().IsCorrectItemType(CurrentItem))
                        {
                            CurrentItem.transform.SetParent(otherSlot.transform);
                            DragAndDropScript.DraggedItem.transform.SetParent(gameObject.transform);
                            CurrentItem.transform.localPosition = Vector3.zero;
                            DragAndDropScript.DraggedItem.transform.localPosition = Vector3.zero;
                        }
                    }

                    if (gameObject.GetComponent<EquipSlotScript>() != null)
                        gameObject.GetComponent<EquipSlotScript>().EquipItem();
                    if (otherSlot.GetComponent<EquipSlotScript>() != null)
                        otherSlot.GetComponent<EquipSlotScript>().EquipItem();
                    DragAndDropScript.DraggedItem = null;
                }
            }
        }
    }
    public abstract bool IsCorrectItemType(GameObject item);
    /*{
        var type = item.GetComponent<DragAndDropScript>().itemType;
        if (AllowedItemType == ItemType.Any || AllowedItemType == type || SecondaryAllowedItemType == type)
            return true;
        else
            return false;
    }*/
}