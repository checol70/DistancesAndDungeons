using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OverWriteScript : MonoBehaviour {

    protected SaveFileEnum saveOverwrite;

	// Use this for initialization
	void Start () {
        SetSave();
	}

    protected abstract void SetSave();
}
