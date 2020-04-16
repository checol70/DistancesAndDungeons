using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class StringOkButtonScript : MonoBehaviour {

    private void Start()
    {
        StartMenuScript.startMenu.StringEnterObject = gameObject.transform.parent.parent.gameObject;
        gameObject.GetComponent<Button>().onClick.AddListener(Pressed);
        gameObject.transform.parent.parent.gameObject.SetActive(false);
    }
    public void Pressed()
    {
        if (!String.IsNullOrEmpty(gameObject.transform.parent.GetChild(0).GetChild(0).GetChild(2).gameObject.GetComponent<Text>().text))
        {
            SaveFileScript.SaveSelected.saveName = gameObject.transform.parent.GetChild(0).GetChild(0).GetChild(2).gameObject.GetComponent<Text>().text;

            if (Directory.Exists(Application.persistentDataPath + "/" + SaveFileScript.CurrentSaveFile.ToString()))
            {
                DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath + "/" + SaveFileScript.CurrentSaveFile.ToString());

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    foreach (FileInfo file in dir.GetFiles())
                    {
                        file.Delete();
                    }
                }
            }
            else
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/" + SaveFileScript.CurrentSaveFile.ToString());
            }

            SceneManager.LoadScene("PlayScene");
        }
    }
}
