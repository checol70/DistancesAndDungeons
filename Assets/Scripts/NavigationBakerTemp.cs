using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationBakerTemp : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<NavMeshSurface>().BuildNavMesh();
	}
}
