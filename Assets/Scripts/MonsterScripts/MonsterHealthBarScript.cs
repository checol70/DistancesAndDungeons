using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MonsterHealthBarScript : MonoBehaviour {

	void Awake()
    {
        gameObject.transform.root.gameObject.GetComponent<HealthScript>().HealthIndicator = gameObject.GetComponent<Slider>();
    }
}
