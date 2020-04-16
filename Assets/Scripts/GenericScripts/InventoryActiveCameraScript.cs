using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryActiveCameraScript : MonoBehaviour {

    public static Camera InventoryCamera;

    private void Awake()
    {
        if(InventoryCamera == null)
        {
            InventoryCamera = gameObject.GetComponent<Camera>();
        }
        if (gameObject.GetComponent<Camera>() != InventoryCamera)
        {
            Destroy(gameObject);
        }
        gameObject.SetActive(false);
    }
}
