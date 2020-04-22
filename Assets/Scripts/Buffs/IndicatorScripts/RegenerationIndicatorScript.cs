using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenerationIndicatorScript : MonoBehaviour
{
    void Awake()
    {
        gameObject.transform.root.gameObject.GetComponent<HealthScript>().AddIndicator(Indicators.regeneration, gameObject);
        gameObject.SetActive(false);
    }
}
