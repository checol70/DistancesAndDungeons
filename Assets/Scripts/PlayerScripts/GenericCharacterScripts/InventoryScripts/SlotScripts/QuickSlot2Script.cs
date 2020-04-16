using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlot2Script : QuickSlotScript
{
    protected override void Ready()
    {
        gameObject.transform.root.gameObject.GetComponent<CharacterScript>().QuickSlot2 = gameObject;
    }
}
