using UnityEngine;
using System.Collections;

public class InventoryScript : MonoBehaviour {

    void Awake()
    {
        gameObject.transform.root.gameObject.GetComponent<CharacterScript>().InventoryManager = gameObject;
        
    }
    private void Start()
    {
        gameObject.GetComponent<Canvas>().worldCamera = InventoryActiveCameraScript.InventoryCamera;
    }
}
