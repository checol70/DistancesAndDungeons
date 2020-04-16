using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class DungeonScript : MonoBehaviour {

    // this is my custom dungeon script for managing levels and saving the dungeon, as well as managing whether something can be killed

    public GameObject[]
        MonsterPrefabs;
    public float spawnChance;

    public static IDictionary<DungeonEnum, GameObject>
        DungeonDictionary = new Dictionary<DungeonEnum,GameObject>();
    public static DungeonScript
        CurrentDungeon;
    public GameObject
        CurrentDungeonGameObject,
        Boss;
    public DungeonEnum
        DungeonName;
    public bool
        BossDeceased;
    public int
        CurrentLevelIndex,
        MaxLevelIndex,
        GeneratedLevelIndexes;
    private IList<LevelScript> Levels = new List<LevelScript>();

    
    private void Start()
    {
        if (DungeonDictionary.Keys.Contains(DungeonEnum.Overworld) && gameObject == DungeonDictionary[DungeonEnum.Overworld])
        {
            SetAsCurrentDungeon();
        }
        else if (!DungeonDictionary.ContainsKey(DungeonEnum.Overworld))
        {
            SetAsCurrentDungeon();
        }

        StartCoroutine(TestForLoad());
    }
    IEnumerator TestForLoad()
    {
        yield return new WaitForSeconds(1f);
        if (GeneratedLevelIndexes == 0)
        {
            CreateNewLevel();
            LevelScript.currentLevel = gameObject.transform.GetChild(0).gameObject;
        }
    }



    public void SetAsCurrentDungeon()
    {
        //StaticDungeonScript.dungeon.dungeonThemes.Clear();
        //StaticDungeonScript.dungeon.dungeonThemes.Add(DungeonTheme);
        CurrentDungeon = gameObject.GetComponent<DungeonScript>();
    }

    public void CreateNewLevel()
    {

        GameObject level = Instantiate(Resources.Load("Level") as GameObject);
        level.transform.SetParent(gameObject.transform);

        level.GetComponent<LevelScript>().Create();

        GeneratedLevelIndexes++;
        
        if (CurrentLevelIndex == MaxLevelIndex)
        {
            LocalExitScript.Exit.SetActive(false);
        }
    }

    private void OnEnable()
    {
        SaveFileScript.StartLoad += Load;
        SaveFileScript.StartSave += Save;
        SaveFileScript.FinishedSaving.Add(gameObject, true);
        DungeonDictionary.Add(DungeonName, gameObject);
    }
    private void OnDisable()
    {
        SaveFileScript.StartLoad -= Load;
        SaveFileScript.StartSave -= Save;
        SaveFileScript.FinishedSaving.Remove(gameObject);
        DungeonDictionary.Remove(DungeonName);
    }

    void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/" + SaveFileScript.CurrentSaveFile.ToString() + "/" + DungeonName.ToString() + "/BaseDungeon.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + SaveFileScript.CurrentSaveFile.ToString() + "/" + DungeonName.ToString() + "/BaseDungeon.dat", FileMode.Open);
            DungeonInfo dungeon = (DungeonInfo)bf.Deserialize(file);
            file.Close();

            GeneratedLevelIndexes = dungeon.GeneratedLevels;
            MaxLevelIndex = dungeon.MaxLevels;
            BossDeceased = dungeon.bossDeceased;
            CurrentLevelIndex = dungeon.CurrentLevel;


            if (dungeon.isCurrentDungeon)
            {
                CurrentDungeon = this;
            }




            List<GameObject> children = new List<GameObject>();

            // make sure we clear out all the old levels if any
            foreach (Transform child in gameObject.transform)
            {
                children.Add(child.gameObject);
            }
            foreach (GameObject child in children) Destroy(child);

            //recreate all the levels that we had
            for (int i = 0; i < GeneratedLevelIndexes; i++)
            {
                GameObject level = Instantiate(Resources.Load("Level") as GameObject);
                level.transform.SetParent(gameObject.transform);

                level.transform.SetSiblingIndex(i);
                
                level.GetComponent<LevelScript>().Load();
                
                

                Levels.Add(level.GetComponent<LevelScript>());
            }




        }

    }
    void Save()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/" + SaveFileScript.CurrentSaveFile.ToString() + "/" + DungeonName.ToString()))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/" + SaveFileScript.CurrentSaveFile.ToString() + "/" + DungeonName.ToString());
        }
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + SaveFileScript.CurrentSaveFile.ToString() + "/" + DungeonName.ToString() + "/BaseDungeon.dat");

        DungeonInfo dungeonDat = new DungeonInfo()
        {
            MaxLevels = MaxLevelIndex,
            GeneratedLevels = GeneratedLevelIndexes,
            CurrentLevel = CurrentLevelIndex,
            bossDeceased = BossDeceased,
            isCurrentDungeon = this == CurrentDungeon
        };
        bf.Serialize(file, dungeonDat);
        SaveFileScript.FinishedSaving[gameObject] = true;
    }
}
[Serializable]
class DungeonInfo
{
    public int
        MaxLevels,
        GeneratedLevels,
        CurrentLevel;
    public bool
        bossDeceased,
        isCurrentDungeon;
}