using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndTurnButtonScript : MonoBehaviour {

    public static GameObject EndTurnButton;
	
    
	void Start () {
	
            CameraScript.GameController.EndTurnButton = gameObject.GetComponent<Button>();
            gameObject.GetComponent<Button>().onClick.AddListener(CameraScript.GameController.UpdatinNavMesh);
       
	}
	
}
