using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfViewFilterScript : MonoBehaviour {

    private void Awake()
    {
        gameObject.transform.root.gameObject.GetComponent<FieldOfView>().viewMeshFilter = gameObject.GetComponent<MeshFilter>();
    }
}
