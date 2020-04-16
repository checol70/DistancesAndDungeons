using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedObjectScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.transform.parent.gameObject.GetComponent<StairHolderScript>().animatedObject = gameObject;

    }
}
