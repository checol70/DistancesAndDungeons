using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapHider : MonoBehaviour {

    private void OnTriggerStay(Collider other)
    {
        if (!CameraScript.GameController.MonsterTurn && !CameraScript.GameController.PlayerTurn)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                gameObject.transform.parent.GetComponent<LowPolyLavaStoneTrap>().revealed = true;

                gameObject.transform.parent.GetChild(0).gameObject.SetActive(true);
                Destroy(gameObject);
            }
        }
    }
}
