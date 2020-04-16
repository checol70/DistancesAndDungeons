using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalEntranceScript : MonoBehaviour {

    public int
     SceneToBeLoaded;
    public static GameObject
        Entrance;
    private int
        PlayerInCount;
    public GameObject[]
        Torches;
    // Use this for initialization
    void Awake()
    {
        Entrance = gameObject;
    }

    IEnumerator LoadingNewLevel()
    {
        foreach (GameObject player in CameraScript.GameController.Players)
        {
            player.GetComponent<CharacterScript>().LoadingNewLevel();
        }
        SaveFileScript.loading = false;
        LevelScript.currentLevel.GetComponent<LevelScript>().Unload();
        yield return null;
        yield return null;
        DungeonScript.CurrentDungeon.CurrentLevelIndex--;
        if (DungeonScript.CurrentDungeon.CurrentLevelIndex < 0)
        {
            DungeonScript.CurrentDungeon = DungeonScript.DungeonDictionary[DungeonEnum.Overworld].GetComponent<DungeonScript>();
        }

        else DungeonScript.CurrentDungeon.gameObject.transform.GetChild(DungeonScript.CurrentDungeon.CurrentLevelIndex).gameObject.GetComponent<LevelScript>().ReCreate();

    }

    public void LoadLevelCheck()
    {
        
        if (CheckForPlayerDistances())
        {
            SpecificCharacterScript.returned = true;
            StartCoroutine(LoadingNewLevel());
            LoadingScript.loadScreen.SetActive(true);
        }
    }
    
    public bool CheckForPlayerDistances()
    {
        PlayerInCount = 0;
        foreach (GameObject player in CameraScript.GameController.Players)
        {
            if (Vector3.Distance(player.transform.position, gameObject.transform.position) <= 2f)
            {
                PlayerInCount++;
                if (PlayerInCount >= CameraScript.GameController.Players.Length)
                {
                    Debug.Log("Players Within Range");
                    SpecificCharacterScript.returned = true;
                    return true;

                }
            }
            else
            {
                Debug.Log("Players Out of Range");
                return false;
            }
        }
        Debug.Log("Loop Failed");
        return false;
    }
}
