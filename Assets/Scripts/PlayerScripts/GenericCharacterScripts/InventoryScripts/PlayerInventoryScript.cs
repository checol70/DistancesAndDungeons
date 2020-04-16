using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerInventoryScript : MonoBehaviour {

void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(CameraScript.GameController.InventoryClosed);
    }
}
