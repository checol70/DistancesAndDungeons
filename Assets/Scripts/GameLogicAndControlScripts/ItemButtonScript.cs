using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemButtonScript : MonoBehaviour {

    void Start()
    {
        if (CameraScript.GameController.InventoryButton == null)
        {
            CameraScript.GameController.InventoryButton = this.gameObject.GetComponent<Button>();
            gameObject.GetComponent<Button>().onClick.AddListener(CameraScript.GameController.InventoryOpened);
        }
    }
}
