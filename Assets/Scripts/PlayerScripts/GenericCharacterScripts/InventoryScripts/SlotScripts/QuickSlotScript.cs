using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuickSlotScript : ItemSlotScript {

	// Use this for initialization
	void Start () {
        Ready();
	}
    protected abstract void Ready();
    public override bool IsCorrectItemType(GameObject item)
    {
        if (item.GetComponent<DragAndDropScript>().itemType == ItemType.Consumable)
        {
            return true;
        }
        else return false;
    }
}
