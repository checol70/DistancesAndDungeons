using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlot3Script : QuickSlotScript
{
    protected override void Ready()
    {
        gameObject.transform.root.gameObject.GetComponent<CharacterScript>().QuickSlot3 = gameObject;
    }
}
