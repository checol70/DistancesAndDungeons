using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnCyclerScript : MonoBehaviour {

    private void Start()
    {
        StartCoroutine(CycleTurns());
    }
    IEnumerator CycleTurns()
    {
        do {

            yield return new WaitForSecondsRealtime(2.5f);
            CameraScript.GameController.UpdatinNavMesh();
            yield return new WaitForSecondsRealtime(2.5f);
            CameraScript.GameController.StartingThePlayerTurn();

        } while (true);
    }
    
}
