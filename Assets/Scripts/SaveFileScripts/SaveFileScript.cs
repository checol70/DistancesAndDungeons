using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class SaveFileScript : MonoBehaviour {

    public string 
        saveName;
    public static DungeonEnum
        StartingActiveDungeon;
    //this is for waiting the program out while everything saves
    public static IDictionary<GameObject,bool> 
        FinishedSaving = new Dictionary<GameObject,bool>();
    // for saving at the end of a turn
    public static SaveFileEnum
        CurrentSaveFile;
    // For singleton
    public static SaveFileScript
        SaveSelected;
    private SaveFileData
        saveLoading;
    GameObject[] Players;
    List<string> 
        CharactersToLoad = new List<string>();
    public delegate void Saving();
    public static event Saving StartSave;
    public delegate void Loading();
    public static event Loading StartLoad;
    public GameObject OverWorldObject;
    public static bool loading;

    private void Awake()
    {
        if (SaveSelected == null)
        {
            SaveSelected = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(SaveSelected!= this)
        {
            Destroy(gameObject);
        }
        if(CurrentSaveFile == SaveFileEnum.SaveFileError)
        {
            CurrentSaveFile = SaveFileEnum.TestFile;
        }
    }

    public void FirstLoad()
    {
        StartCoroutine(Load());
    }

    
    public IEnumerator Load()
    {
        
        if (File.Exists(Application.persistentDataPath + "/" + CurrentSaveFile + "/SaveFile.dat"))
        {
            loading = true;
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + CurrentSaveFile + "/SaveFile.dat", FileMode.Open);
            FileStream nameFile = File.Open(Application.persistentDataPath + "/" + CurrentSaveFile.ToString() + "name.tic", FileMode.Open);

            saveLoading = (SaveFileData)bf.Deserialize(file);
            saveName = (string)bf.Deserialize(nameFile);

            file.Close();
            nameFile.Close();
            
            // Making the right Dungeon Active
            StartingActiveDungeon = saveLoading.ActiveDungeon;


            if (StartingActiveDungeon != DungeonEnum.StarterDungeon)
            {
                SceneManager.LoadScene("PlayScene");
            }
            else
            {
                SceneManager.LoadScene("PlayScene");
            }
            for (int i = 0; i < 12; i++)
            {
                yield return new WaitForEndOfFrame();
            }

            CameraScript.GameController.transform.position = new Vector3(saveLoading.cameraPosition[0], saveLoading.cameraPosition[1], saveLoading.cameraPosition[2]);

            StartLoad();
            Players = GameObject.FindGameObjectsWithTag("Player");
            foreach (string name in saveLoading.CurrentLivingCharacters)
            {
                CharactersToLoad.Add(name);
            }
            foreach (GameObject player in Players)
            {
                if (!CharactersToLoad.Contains(player.name))
                {
                    player.SetActive(false);
                }




            }
        }
        else Debug.Log("NoFileFound");
    }

    public void OverWriteSaveFile1()
    {
        SetCurrentSaveFile(SaveFileEnum.SaveFile1);
        
        StartMenuScript.startMenu.FileNumberChosen();
        
    }

    public void OverWriteSaveFile2()
    {
        SetCurrentSaveFile(SaveFileEnum.SaveFile2);
        
        StartMenuScript.startMenu.FileNumberChosen();

    }
    public void OverWriteSaveFile3()
    {
        SetCurrentSaveFile(SaveFileEnum.SaveFile3);
        
        StartMenuScript.startMenu.FileNumberChosen();
    }

        public void Save()
    {
        if (!FinishedSaving.ContainsKey(gameObject))
        {
            FinishedSaving.Add(gameObject, false);
        }
        FinishedSaving[gameObject] = false;
        List<GameObject> Keys = new List<GameObject>(FinishedSaving.Keys);

        foreach (GameObject key in Keys)
        {
            FinishedSaving[key] = false;
        }
        SaveScreenScript.SaveScreen.SetActive(true);
        if (!Directory.Exists(Application.persistentDataPath + "/" + CurrentSaveFile.ToString()))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/" + CurrentSaveFile.ToString());
        }
        if (StartSave != null)
        {
            StartSave();
        }


        GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");
        List<string> PlayerNames = new List<string>();

        foreach (GameObject player in Players)
        {
            if (player.activeInHierarchy)
            {
                PlayerNames.Add(player.name);
            }
        }

        Debug.Log("StartSave");
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + CurrentSaveFile.ToString() + "/SaveFile.dat");
        FileStream nameFile = File.Create(Application.persistentDataPath + "/" + CurrentSaveFile.ToString() + "name.tic");

        bf.Serialize(nameFile, saveName);

        GameObject cam = CameraScript.GameController.gameObject;
        Vector3 cPos = CameraScript.GameController.transform.position;

        SaveFileData mySaveData = new SaveFileData()
        {
            orthoSize = cam.GetComponent<Camera>().orthographicSize,

            cameraPosition = new float[3]{cPos.x,cPos.y,cPos.z},

            ActiveDungeon = DungeonScript.CurrentDungeon.DungeonName,

            CurrentLivingCharacters = PlayerNames.ToArray()
        };
        bf.Serialize(file, mySaveData);
        file.Close();
        Debug.Log("FinishedSave");
        FinishedSaving[gameObject] = true;

    }
    public void SetFile1()
    {
        SetCurrentSaveFile(SaveFileEnum.SaveFile1);
        if (Directory.Exists(Application.persistentDataPath + "/" + CurrentSaveFile.ToString()))
        {
            
            FirstLoad();
        }
        else
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/" + CurrentSaveFile.ToString());
            SceneManager.LoadScene("PlayScene");
        }
        
        
    }
    public void SetFile2()
    {
        SetCurrentSaveFile(SaveFileEnum.SaveFile2);
        if (Directory.Exists(Application.persistentDataPath + "/" + CurrentSaveFile.ToString()))
        {
            
            FirstLoad();
        }
        else
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/" + CurrentSaveFile.ToString());
            SceneManager.LoadScene("PlayScene");
        }
    }
    public void SetFile3()
    {
        SetCurrentSaveFile(SaveFileEnum.SaveFile3);
        if (Directory.Exists(Application.persistentDataPath + "/" + CurrentSaveFile.ToString()))
        {
            
            FirstLoad();
        }
        else
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/" + CurrentSaveFile.ToString());
            SceneManager.LoadScene("PlayScene");
        }
    }

    private void SetCurrentSaveFile(SaveFileEnum SaveFileChosen)
    {
        CurrentSaveFile = SaveFileChosen;
    }
}
public enum SaveFileEnum
{
    SaveFileError = 0,
    SaveFile1,
    SaveFile2,
    SaveFile3,
    TestFile
}
[Serializable]
class SaveFileData
{
    public float orthoSize;
    public float[] cameraPosition; 
    public DungeonEnum 
        ActiveDungeon;
    public string[]
        CurrentLivingCharacters;
}
