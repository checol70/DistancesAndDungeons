using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalExitScript : MonoBehaviour {

    private int
        PlayerInCount;
    public static GameObject
        Exit;
    public int SceneToBeLoaded;
    public bool
        //PIA Player In Area
        PIA;
    private bool
        //PSM Player Sees Me
        PSM;
    // Use this for initialization
    public IList<Renderer> rend = new List<Renderer>();
    void Awake()
    {
        Exit = gameObject;
        rend = GetComponentsInChildren<Renderer>();
        foreach(Renderer r in rend)
        {
            r.enabled = false;
        }
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
        DungeonScript.CurrentDungeon.CurrentLevelIndex++;

        if (DungeonScript.CurrentDungeon.CurrentLevelIndex >= DungeonScript.CurrentDungeon.GeneratedLevelIndexes)
        {
            DungeonScript.CurrentDungeon.CreateNewLevel();
            foreach (Renderer r in rend)
            {
                r.enabled = false;
            }
        }
        else
        {
            DungeonScript.CurrentDungeon.gameObject.transform.GetChild(DungeonScript.CurrentDungeon.CurrentLevelIndex).gameObject.GetComponent<LevelScript>().ReCreate();
        }

    }
    
    private void Update()
    {
        if(!LoadingScript.loadScreen.activeInHierarchy && !PSM && PIA)
        {
            RaycastHit rH;
            Physics.Raycast(gameObject.transform.position, CameraScript.GameController.ActivePlayer.transform.position - gameObject.transform.position, out rH, 20f, 1<<11);
            if(rH.collider != null)
            {
                if(rH.collider.gameObject.transform.root.gameObject.CompareTag("Player"))
                {
                    PSM = true;
                    foreach(Renderer r in rend)
                    {
                        r.enabled = true;
                    }
                }
            }
        }
    }

    public void LoadLevelCheck()
    {
        
        if (CheckForPlayerDistances())
        {
            LoadingScript.loadScreen.SetActive(true);
            SpecificCharacterScript.returned = false;
            StartCoroutine(LoadingNewLevel());
        }
    }
    public bool CheckForPlayerDistances()
    {
        Debug.Log("Checking");
        PlayerInCount = 0;
        foreach (GameObject player in CameraScript.GameController.Players)
        {
            if (Vector3.Distance(player.transform.position, gameObject.transform.position) <= 2f)
            {
                PlayerInCount++;
                if (PlayerInCount >= CameraScript.GameController.Players.Length)
                {
                    return true;
                }
            }
            else return false;
        }
        return false;
    }
}
