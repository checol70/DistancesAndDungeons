using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewFileButtonScript : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(StartMenuScript.startMenu.NewFileChosen);
    }

}
