using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuScript : MonoBehaviour {

    // This script is for controlling the start menu in the first scene of the game.  
    // for singleton and ease of access
    public static StartMenuScript
        startMenu;
    public GameObject
        NewFileObject,
        LoadFileObject,
        ExitObject,
        MainMenu,
        StringEnterObject;
    private void Awake()
    {
        startMenu = this;
    }
    public void FileNumberChosen()
    {
        StringEnterObject.SetActive(true);
    }
    public void NewFileChosen()
    {

        NewFileObject.SetActive(true);
    }
    public void LoadFileChosen()
    {

        LoadFileObject.SetActive(true);
    }
    public void ExitChosen()
    {
        ExitObject.SetActive(true);

    }
    public void ReturnToMainMenu()
    {
        if (NewFileObject.activeInHierarchy)
        {
            NewFileObject.SetActive(false);
        }
        if (LoadFileObject.activeInHierarchy)
        {
            LoadFileObject.SetActive(false);
        }
        if (ExitObject.activeInHierarchy)
        {
            ExitObject.SetActive(false);
        }
        if (StringEnterObject.activeInHierarchy)
        {
            StringEnterObject.SetActive(false);
        }
    }
}
