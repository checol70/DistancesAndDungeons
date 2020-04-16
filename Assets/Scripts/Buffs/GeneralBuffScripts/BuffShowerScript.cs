using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffShowerScript : MonoBehaviour {

    private void Start()
    {
        gameObject.transform.root.gameObject.GetComponent<BuffReferenceScript>().BuffShower = gameObject.transform;
    }
}
