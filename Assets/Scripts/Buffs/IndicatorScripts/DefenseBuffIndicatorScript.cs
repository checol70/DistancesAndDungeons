using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseBuffIndicatorScript : MonoBehaviour
{
    void Awake()
    {
        gameObject.transform.root.gameObject.GetComponent<HealthScript>().AddIndicator(Indicators.defense, gameObject);
        gameObject.SetActive(false);
    }
}
