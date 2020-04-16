using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TimerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.transform.root.gameObject.GetComponent<MonsterScript>().timer = gameObject.GetComponent<Text>();
        gameObject.SetActive(false);
	}
}
