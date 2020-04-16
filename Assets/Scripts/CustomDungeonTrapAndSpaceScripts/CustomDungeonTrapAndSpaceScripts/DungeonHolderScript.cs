using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DungeonHolderScript : MonoBehaviour {

    public static DungeonHolderScript holder;


    //public static IDictionary<GameObject,bool> stairs = new Dictionary<GameObject,bool>();
    private void Awake()
    {
        if(holder == null)
        {
            holder = this;
        }if(holder != this)
        {
            Destroy(gameObject);
        }
    }
    //private void Start()
    //{
    //    //StartCoroutine(FirstNavMeshBuild());        
    //}
    private void OnEnable()
    {
        CameraScript.EndingPlayerTurn += StartBuildNavMesh;
    }
    private void OnDisable()
    {
        CameraScript.EndingPlayerTurn -= StartBuildNavMesh;
    }

    IEnumerator FirstNavMeshBuild()
    {
        yield return null;
        StartBuildNavMesh();
    }

    public void StartBuildNavMesh()
    {

        gameObject.GetComponent<NavMeshSurface>().BuildNavMesh();
    }
}
