using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitObjectScript : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        if (QuitButtonScript.QuitObject == null)
        {
            QuitButtonScript.QuitObject = gameObject;
        }
        else if (QuitButtonScript.QuitObject != gameObject)
        {
            Destroy(gameObject);
        }
        gameObject.SetActive(false);
    }
}
