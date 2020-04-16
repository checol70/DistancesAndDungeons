using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ReturnToMainMenuButtonScript : MonoBehaviour {

    private void Awake()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(OpenReturnMenu);
    }
    public void OpenReturnMenu()
    {
        MainMenuObjectScript.mainMenuObject.SetActive(true);
    }
}
