using UnityEngine;
using System.Collections;

public abstract class DroppedShinyScript : MonoBehaviour {

    protected GameObject ActivePlayer, ActiveBackpack,CurrentItemSlot, CurrentItem;
    
    // so that we can have dropped weapons, shields, and armor we are doing abstract
    public abstract void PickedUp();
	
}
