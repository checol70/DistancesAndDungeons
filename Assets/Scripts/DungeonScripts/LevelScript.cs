using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.AI;


public class LevelScript : MonoBehaviour
{
    public static GameObject
        currentLevel;
    public Vector3
        StartPosition,
        EndPosition;
    public bool exitFound;

    public GameObject boss;

    public LevelContents
        myContents;

    public int
        IntSeed;
    public uint
        Seed;
    private List<MonsterData>
        MonsterList = new List<MonsterData>();
    IDictionary<BuffScript, Buffs> BuffsContained = new Dictionary<BuffScript, Buffs>();
    Map map;

    public void Create()
    {
        StartCoroutine(BuildLevel());

        currentLevel = gameObject;
    }
    IEnumerator BuildLevel()
    {

        yield return null;

        LevelGeneratorScript lgs = LevelGeneratorScript.LevelGenerator;
        map = lgs.CreateFullMap(0,null);
        yield return null;

        LocalEntranceScript.Entrance.gameObject.transform.position = map.start- new Vector3(0,.5f,0);
        LocalExitScript.Exit.gameObject.transform.position = map.end - new Vector3(0, .5f, 0);

        yield return new WaitForSeconds(2f);
        DungeonHolderScript.holder.StartBuildNavMesh();

        foreach (GameObject player in CameraScript.GameController.Players)
        {
            player.GetComponent<SpecificCharacterScript>().NewLevel();
        }

        yield return new WaitForSeconds(2f);
        yield return new WaitWhile(() => LoadingScript.loadScreen == null);

        DungeonHolderScript.holder.StartBuildNavMesh();
        LoadingScript.loadScreen.SetActive(false);
    }

    IEnumerator RebuildLevel()
    {
        yield return null;

        LevelGeneratorScript lgs = LevelGeneratorScript.LevelGenerator;
        lgs.InstantiateFogOfWar(map);
        yield return null;

        currentLevel = gameObject;
        LevelFromContents(myContents);
        yield return new WaitForSeconds(2f);
        DungeonHolderScript.holder.StartBuildNavMesh();
        foreach (GameObject player in CameraScript.GameController.Players)
        {
            player.GetComponent<SpecificCharacterScript>().NewLevel();
        }

        yield return new WaitForSeconds(2f);


        LoadingScript.loadScreen.SetActive(false);
    }


    //public void FindSpecialRooms(DungeonModel model)
    //{
    //    var gridModel = model as GridDungeonModel;
    //    if (gridModel == null) return;

    //    var furthestCells = GridDungeonModelUtils.FindFurthestRooms(gridModel);
    //    if (furthestCells.Length == 2 && furthestCells[0] != null && furthestCells[1] != null)
    //    {
    //        var startCell = furthestCells[0];
    //        var endCell = furthestCells[1];
    //        Debug.Log("is this repeating too?");
    //        SetStartingCell(gridModel, startCell);
    //        SetEndingCell(gridModel, endCell);
    //        if (!TestPath())
    //        {
    //            StopCoroutine(BuildLevel());
    //            Debug.Log("is this the spot?");
    //            Create();
    //            return;
    //        }
    //        else if (DungeonScript.CurrentDungeon.CurrentLevelIndex == DungeonScript.CurrentDungeon.MaxLevelIndex)
    //        {
    //            // here is where we will put spawning the boss.

    //            Debug.Log(StartPosition.ToString());
    //            boss = Instantiate(DungeonScript.CurrentDungeon.Boss, StartPosition, Quaternion.Euler(0, 0, 0));
    //        }
    //    }
    //    else
    //    {
    //        Debug.Log("or is this it?");
    //        Create();
    //        return;
    //    }

    //    GridDungeonModel grid = (GridDungeonModel)model;

    //    foreach (Cell cell in grid.Cells)
    //    {
    //        if (cell != furthestCells[0] && cell != furthestCells[1])
    //        {
    //            float spawnGuess = UnityEngine.Random.value;
    //            if (spawnGuess <= DungeonScript.CurrentDungeon.spawnChance)
    //            {
    //                int Monster = UnityEngine.Random.Range(0, DungeonScript.CurrentDungeon.MonsterPrefabs.Length);
    //                Instantiate(DungeonScript.CurrentDungeon.MonsterPrefabs[Monster], MathUtils.GridToWorld(grid.Config.GridCellSize, cell.CenterF) + Vector3.up, Quaternion.Euler(Vector3.zero));
    //            }
    //        }
    //    }
    //    // this is where populating the dungeon goes

    //}
    bool TestPath()
    {
        NavMeshPath path = new NavMeshPath();
        DungeonHolderScript.holder.gameObject.GetComponent<NavMeshSurface>().BuildNavMesh();
        NavMesh.CalculatePath(StartPosition, EndPosition, NavMesh.AllAreas, path);
        if (path.status == NavMeshPathStatus.PathComplete)
        {
            foreach (GameObject player in CameraScript.GameController.Players)
            {
                player.GetComponent<CharacterScript>().TemporaryTurnOff();

            }

            return true;

        }
        else return false;
    }



    void SetStartingCell()
    {
        LocalEntranceScript.Entrance.transform.position = map.start;

        LocalEntranceScript.Entrance.SetActive(false);
    }

    void SetEndingCell()
    {

        LocalExitScript.Exit.transform.position = map.end;

        if (DungeonScript.CurrentDungeon.CurrentLevelIndex == DungeonScript.CurrentDungeon.MaxLevelIndex)
        {
            LocalExitScript.Exit.SetActive(false);

        }
        else
        {
            LocalExitScript.Exit.SetActive(true);
        }
    }



    public void ReCreate()
    {
        StartCoroutine(RebuildLevel());

    }

    private void OnEnable()
    {
        SaveFileScript.StartLoad += Load;
        SaveFileScript.StartSave += Save;
        SaveFileScript.FinishedSaving.Add(gameObject, true);
    }
    private void OnDisable()
    {
        SaveFileScript.StartLoad -= Load;
        SaveFileScript.StartSave -= Save;
        SaveFileScript.FinishedSaving.Remove(gameObject);
    }


    void LevelFromContents(LevelContents levelContents)
    {
        // populate the monsters
        foreach (MonsterData monsterData in levelContents.AllMonsters)
        {
            GameObject CurrentMonster = Instantiate(Resources.Load("EnemyPrefabs/TargetDummy"/* + monsterData.MonsterType.ToString()*/) as GameObject);
            CurrentMonster.transform.position = ConvertToVector(monsterData.position);
            CurrentMonster.transform.rotation = Quaternion.Euler(ConvertToVector(monsterData.rotation));

            MonsterScript monsterScript = CurrentMonster.GetComponent<MonsterScript>();
            monsterScript.MonsterLevel = monsterData.Level;

            // not sure that we will be using monster buffs, if we have time we will fix this
            /*for (int i = 0; i < monsterData.CurrentBuffs.Length; i++)
            {
                Type mytype = Type.GetType(monsterData.CurrentBuffs[i].ToString());

                CurrentMonster.AddComponent(mytype);

                BuffScript[] BuffArray = CurrentMonster.GetComponents<BuffScript>();

                BuffArray[i].BuffStrength = monsterData.BuffStrengths[i];
            }*/
        }
        if (levelContents.myConsumableLoot.Length > 0)
            for (int i = 0; i < levelContents.myConsumableLoot.Length; i++)
            {
                GameObject currentConsumable = Instantiate(Resources.Load("DroppedConsumables/" + levelContents.myConsumableLoot[i].consumableType.ToString()) as GameObject);
                currentConsumable.transform.position = ConvertToVector(levelContents.myConsumablePositions[i]);
                currentConsumable.transform.rotation = Quaternion.Euler(ConvertToVector(levelContents.myConsumableRotations[i]));
                currentConsumable.GetComponent<DroppedConsumableScript>().amount = levelContents.myConsumableLoot[i].amountCarried;
            }
        if (levelContents.myWeaponLoot.Length > 0)
            for (int i = 0; i < levelContents.myWeaponLoot.Length; i++)
            {
                WeaponData currentWeaponData = levelContents.myWeaponLoot[i];
                GameObject currentWeapon = Instantiate(Resources.Load("DroppedWeapons/" + currentWeaponData.weaponType.ToString() + "/" + currentWeaponData.weaponVariant) as GameObject);

                currentWeapon.transform.position = ConvertToVector(levelContents.myWeaponLootPositions[i]);
                currentWeapon.transform.rotation = Quaternion.Euler(ConvertToVector(levelContents.myWeaponRotations[i]));



                if (currentWeaponData.enchants != null)
                {
                    for (i = 0; i < currentWeaponData.enchants.Length; i++)
                    {
                        EnchantmentType enchant = currentWeaponData.enchants[i];
                        Type mytype = Type.GetType(enchant.ToString() + "Script");
                        currentWeapon.AddComponent(mytype);

                        EnchantmentScript[] myEnchants = currentWeapon.GetComponents<EnchantmentScript>();


                        myEnchants[i].Strength = currentWeaponData.enchantmentStrengths[i];

                        myEnchants[i].ManaPenalty = currentWeaponData.enchantmentPenalties[i];
                        myEnchants[i].ManaCost = currentWeaponData.enchantmentCosts[i];
                    }
                }
            }
        map = levelContents.map;
        LocalExitScript.Exit.transform.position = map.end;

        LocalEntranceScript.Entrance.transform.position = map.start;
        if (DungeonScript.CurrentDungeon.CurrentLevelIndex == DungeonScript.CurrentDungeon.MaxLevelIndex)
        {
            LocalExitScript.Exit.SetActive(false);
        }
        else
        {
            LocalExitScript.Exit.SetActive(true);
        }
    }


    public void Load()
    {

        BinaryFormatter bf = new BinaryFormatter();

        Debug.Log(Application.persistentDataPath);
        if (
                File.Exists
                (

                        Application.persistentDataPath + "/"

                + SaveFileScript.CurrentSaveFile.ToString() + "/"

                + gameObject.transform.root.gameObject.GetComponent<DungeonScript>().DungeonName.ToString() +

                "/Level" + gameObject.transform.GetSiblingIndex().ToString() + ".Bannana"

                )
        )
        {
            FileStream file = File.Open(Application.persistentDataPath + "/"

                + SaveFileScript.CurrentSaveFile.ToString() + "/"

                + gameObject.transform.root.gameObject.GetComponent<DungeonScript>().DungeonName.ToString() +

                "/Level" + gameObject.transform.GetSiblingIndex().ToString() + ".Bannana", FileMode.Open);



            myContents = (LevelContents)bf.Deserialize(file);
            file.Close();
            if (DungeonScript.CurrentDungeon.CurrentLevelIndex == gameObject.transform.GetSiblingIndex())
            {
                StartCoroutine(RebuildLevel());
            }

        }
        else throw new ArgumentException("Level Doesn't Exist, Don't mess with savefiles!");

    }

    public void Unload()
    {
        if (currentLevel == gameObject)
        {
            GameObject[] Monsters = GameObject.FindGameObjectsWithTag("Monster");

            foreach (GameObject monster in Monsters)
            {
                MonsterList.Add(new MonsterData
                {
                    position = ConvertToArray(monster.transform.position),
                    rotation = ConvertToArray(monster.transform.rotation.eulerAngles),

                    Level = monster.GetComponent<MonsterScript>().MonsterLevel,
                    MonsterType = monster.GetComponent<MonsterScript>().monsterType
                });
                Destroy(monster);
            }

            GameObject[] droppedStuffs = GameObject.FindGameObjectsWithTag("Shiny");

            List<WeaponData> droppedWeaponData = new List<WeaponData>();

            List<Array> droppedWeaponPositions = new List<Array>();
            List<Array> droppedWeaponRotations = new List<Array>();

            List<Array> droppedConsumablePositions = new List<Array>();
            List<Array> droppedConsumableRotations = new List<Array>();

            List<ConsumableInfo> droppedConsumableInfo = new List<ConsumableInfo>();

            foreach (GameObject droppedItem in droppedStuffs)
            {
                if (droppedItem.GetComponent<DroppedConsumableScript>())
                {
                    ConsumableInfo consumable = new ConsumableInfo()
                    {
                        amountCarried = droppedItem.GetComponent<DroppedConsumableScript>().amount,
                        consumableType = droppedItem.GetComponent<DroppedConsumableScript>().ConsumableType
                    };

                    droppedConsumableInfo.Add(consumable);
                    droppedConsumablePositions.Add(ConvertToArray(droppedItem.transform.position));
                    droppedConsumableRotations.Add(ConvertToArray(droppedItem.transform.rotation.eulerAngles));
                }
                else if (droppedItem.GetComponent<DroppedWeaponScript>())
                {
                    DroppedWeaponScript currentWeapon = droppedItem.gameObject.GetComponent<DroppedWeaponScript>();


                    // setting these up so that we can turn them into arrays then save the inventory as an array
                    List<EnchantmentType> currentEnchants = new List<EnchantmentType>();
                    List<int> currentEnchantStrengths = new List<int>();


                    List<int> currentEnchantCosts = new List<int>();
                    List<int> currentEnchantPenalties = new List<int>();


                    foreach (EnchantmentScript enchant in currentWeapon.gameObject.GetComponents<EnchantmentScript>())
                    {
                        currentEnchants.Add(enchant.EnchantType);
                        currentEnchantStrengths.Add(enchant.Strength);

                        currentEnchantCosts.Add(enchant.ManaCost);
                        currentEnchantPenalties.Add(enchant.ManaPenalty);
                    }
                    WeaponData currentData = new WeaponData()
                    {

                        reactionCount = 0,
                        weaponType = currentWeapon.WeaponCategory,

                        weaponVariant = currentWeapon.WeaponVariation,
                        enchants = currentEnchants.ToArray(),

                        enchantmentStrengths = currentEnchantStrengths.ToArray(),
                        enchantmentCosts = currentEnchantCosts.ToArray(),
                        enchantmentPenalties = currentEnchantPenalties.ToArray()

                    };
                    droppedWeaponData.Add(currentData);
                    droppedWeaponPositions.Add(ConvertToArray(droppedItem.transform.position));
                    droppedWeaponRotations.Add(ConvertToArray(droppedItem.transform.rotation.eulerAngles));


                }
            }
            // converting the lists we filled to something we can use
            float[][] consumablePos = new float[droppedConsumablePositions.Count][];
            for (int i = 0; i < consumablePos.Length; i++)
            {
                consumablePos[i] = (float[])droppedConsumablePositions[i];
            }
            float[][] consumableRot = new float[droppedConsumableRotations.Count][];
            for (int i = 0; i < consumableRot.Length; i++)
            {
                consumableRot[i] = (float[])droppedConsumableRotations[i];
            }

            float[][] droppedWeaponPos = new float[droppedWeaponPositions.Count][];
            for (int i = 0; i < droppedWeaponPos.Length; i++)
            {
                droppedWeaponPos[i] = (float[])droppedWeaponPositions[i];
            }
            float[][] droppedWeaponRot = new float[droppedWeaponRotations.Count][];
            for (int i = 0; i < droppedWeaponRot.Length; i++)
            {
                droppedWeaponRot[i] = (float[])droppedWeaponRotations[i];
            }

            // putting it all into contents so that we can reload it later.
            myContents = new LevelContents()
            {
                AllMonsters = MonsterList.ToArray(),

                myConsumableLoot = droppedConsumableInfo.ToArray(),
                myConsumablePositions = consumablePos,
                myConsumableRotations = consumableRot,

                myWeaponLoot = droppedWeaponData.ToArray(),
                myWeaponLootPositions = droppedWeaponPos,
                myWeaponRotations = droppedWeaponRot,


                eF = exitFound,

                map = map
            };


        }
    }

    private Vector3 ConvertToVector(float[] array)
    {
        //if (array.Length == 3)
        {
            Vector3 currentVector = new Vector3(array[0], array[1], array[2]);
            return currentVector;
        }
        //else throw new ArgumentException("Array must have x, y, and z values");
    }

    private float[] ConvertToArray(Vector3 vector)
    {
        float[] current = new float[3] { vector.x, vector.y, vector.z };
        return current;
    }

    void PrepLevelInfo()
    {

    }

    public void Save()
    {
        SaveFileScript.FinishedSaving[gameObject] = false;
        if (currentLevel != gameObject)
        {




            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Create(Application.persistentDataPath + "/"

                + SaveFileScript.CurrentSaveFile.ToString() + "/"

                + gameObject.transform.root.gameObject.GetComponent<DungeonScript>().DungeonName.ToString() +

                "/Level" + gameObject.transform.GetSiblingIndex().ToString() + ".Bannana");


            bf.Serialize(file, myContents);
            file.Close();

            SaveFileScript.FinishedSaving[gameObject] = true;

        }
        else StartCoroutine(Saving());
    }
    IEnumerator Saving()
    {
        Unload();
        yield return null;
        yield return new WaitForSeconds(1f);

        BinaryFormatter bf = new BinaryFormatter();

        FileStream file = File.Create(Application.persistentDataPath + "/"

            + SaveFileScript.CurrentSaveFile.ToString() + "/"

            + gameObject.transform.root.gameObject.GetComponent<DungeonScript>().DungeonName.ToString() +

            "/Level" + gameObject.transform.GetSiblingIndex().ToString() + ".Bannana");


        bf.Serialize(file, myContents);
        file.Close();

        SaveFileScript.FinishedSaving[gameObject] = true;
    }
}
[Serializable]
public class LevelContents
{
    public MonsterData[]
        AllMonsters;
    public WeaponData[]
        myWeaponLoot;
    public ConsumableInfo[]
        myConsumableLoot;
    public float[][]
        myWeaponLootPositions,
        myConsumablePositions,
        myWeaponRotations,
        myConsumableRotations;
    public bool
        eF;
    public Map map;

}
