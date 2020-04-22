using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornedIndicatorScript : MonoBehaviour
{
    void Awake()
    {
        gameObject.transform.root.gameObject.GetComponent<HealthScript>().AddIndicator(Indicators.thorned, gameObject);
        gameObject.SetActive(false);
    }
}
