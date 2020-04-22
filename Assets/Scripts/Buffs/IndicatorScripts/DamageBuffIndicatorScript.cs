using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBuffIndicatorScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        gameObject.transform.root.gameObject.GetComponent<HealthScript>().AddIndicator(Indicators.attackBuff, gameObject);
        gameObject.SetActive(false);
    }
}
