using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunnedIndicatorScript : MonoBehaviour
{
    void Awake()
    {
        gameObject.transform.root.gameObject.GetComponent<HealthScript>().AddIndicator(Indicators.stunned, gameObject);
        gameObject.SetActive(false);
    }
}
