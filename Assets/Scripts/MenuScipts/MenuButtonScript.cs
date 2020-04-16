using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonScript : MonoBehaviour {

    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(OpenMenu);
    }
    public void OpenMenu()
    {
        GameObject menu = MenuScript.menuScript.gameObject;
        if (menu.activeInHierarchy == false)
        {
            menu.SetActive(true);
            CameraScript.InventoryOpen = true;
        }
        else if (menu.activeInHierarchy == true)
        {
            menu.SetActive(false);
            CameraScript.InventoryOpen = false;
        }
    }
}
