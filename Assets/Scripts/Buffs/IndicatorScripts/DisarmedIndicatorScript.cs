using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisarmedIndicatorScript : MonoBehaviour
{
    void Awake()
    {
        gameObject.transform.root.gameObject.GetComponent<HealthScript>().AddIndicator(Indicators.disarmed, gameObject);
        gameObject.SetActive(false);
    }
}
