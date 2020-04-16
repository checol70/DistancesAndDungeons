using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverWrite2 : OverWriteScript {

    protected override void SetSave()
    {
        saveOverwrite = SaveFileEnum.SaveFile2;
    }
}
