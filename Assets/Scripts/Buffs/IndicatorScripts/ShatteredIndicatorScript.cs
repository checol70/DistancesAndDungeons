using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatteredIndicatorScript : MonoBehaviour
{
    void Awake()
    {
        gameObject.transform.root.gameObject.GetComponent<HealthScript>().AddIndicator(Indicators.defenseShattered, gameObject);
        gameObject.SetActive(false);
    }
}
