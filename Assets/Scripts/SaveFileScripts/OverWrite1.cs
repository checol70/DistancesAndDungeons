using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverWrite1 : OverWriteScript {

    protected override void SetSave()
    {
        saveOverwrite = SaveFileEnum.SaveFile1;
    }
}
