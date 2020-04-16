using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaPenaltyShowerScript : MonoBehaviour {

    void Awake()
    {
        gameObject.transform.parent.parent.gameObject.GetComponent<StatShowerScript>().ManaPenaltyObject = gameObject.GetComponent<Text>();
    }
}
