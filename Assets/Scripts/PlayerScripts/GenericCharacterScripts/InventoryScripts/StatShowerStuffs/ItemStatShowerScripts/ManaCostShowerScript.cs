using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaCostShowerScript : MonoBehaviour {

    void Awake()
    {
        gameObject.transform.parent.parent.gameObject.GetComponent<StatShowerScript>().ManaCostObject = gameObject.GetComponent<Text>();
    }
}
