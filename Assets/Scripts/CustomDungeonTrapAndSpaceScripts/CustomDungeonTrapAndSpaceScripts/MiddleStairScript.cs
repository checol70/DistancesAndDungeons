using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleStairScript : MonoBehaviour {
    public GameObject HeightTarget;
	// Use this for initialization
	void Start () {
        gameObject.transform.parent.gameObject.GetComponent<StairHolderScript>().middleStairObject = gameObject;
        
    }
    private void FixedUpdate()
    {
        if (HeightTarget != null)
            gameObject.GetComponent<Rigidbody>().MovePosition(new Vector3(gameObject.transform.position.x, HeightTarget.transform.position.y, gameObject.transform.position.z));
    }
}
