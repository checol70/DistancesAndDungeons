using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlot1Script : QuickSlotScript {
    protected override void Ready()
    {
        gameObject.transform.root.gameObject.GetComponent<CharacterScript>().QuickSlot1 = gameObject;
    }

}
