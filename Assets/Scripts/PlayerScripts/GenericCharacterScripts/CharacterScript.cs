using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public enum Indicators
{
    regeneration = 0,
    defense,
    attackBuff,
    stunned,
    thorned,
    defenseShattered,
    disarmed
}


public class CharacterScript : BuffReferenceScript
{

    public Button CharacterButton;

    // this is for weapon Leveling, so that having different damages and costs doesn't effect the needed damage.
    public static IDictionary<WeaponType, int> WeaponDamage = new Dictionary<WeaponType, int>()
    {
        { WeaponType.Ranged, 7 },
        {WeaponType.Dagger, 5 },
        { WeaponType.SwordAndShield, 5 },
        {WeaponType.GreatAxe, 5 },
        {WeaponType.Fisticuffs, 7},
        {WeaponType.Magic, 5 }
    };

    private RaycastHit
        Strucken;
    [SerializeField]
    public int
        DamageNeededToLevelUp;
    public int
        TotalManaPenalty;

    public WeaponType
        EquippedWeaponType;

    public IDictionary<WeaponType, int> WeaponLevels = new Dictionary<WeaponType, int>();

    public IDictionary<WeaponType, int> TotalWeaponDamages = new Dictionary<WeaponType, int>();

    private TypeOfStats
        EquippedWeaponStat;

    private bool
        IsTurn,
        CM;

    public bool
        IsLoaded,
        Unarmed,
        HasNoMovement,
        StillMoving;

    public Material
        EnoughMovement,
        NotEnoughMovement;

    public float
        DistanceFromCheckpoint,
        RemainingMovement,
        AttackCost;

    public int
        DamageDealt,
        CurrentWeaponLevel,
        ShieldLevel,
        MaxMana,
        ModifiedMaxMana,
        TotalManaRegened,
        CurrentMana,
        MaxHealth,
        CurrentHP;

    public float
        TotalDistanceTraveled;

    private float
        MaxMovement;

    public float
        AttackRoll,
        Range,
        modifiedMaxMovement;

    private GameObject
        Self;

    public GameObject
        CharacterMeshObject,
        SphereColliderObject,
        Backpack,
        WeaponHook,
        OffHandHook,
        InventoryManager,
        WeaponSlot,
        OnTopSlot,
        WeaponHeld,
        HitTarget,
        LastHit;
    

    private SpecificCharacterScript
        mySheet;
    public GameObject RangeIndicator;
    public Transform
        MovementIndicator,
        AttackCostIndicator;

    public GameObject
        TargetSeen,
        ActivePlayer,
        EquippedWeapon;

    private Vector3
        MoveTowards,
        CurrentCheckpoint,
        InActive;

    private Rigidbody
        Mine;

    public int
        AverageDamage;

    public int
        NumberOfAttacks,

        TotalAverageDamage,
        CounterChance;

    public Slider
        CharacterButtonManaIndicator,
        CharacterButtonMaxManaIndicator,
        InventoryHealthSlider,
        InventoryMaxManaSlider,
        InventoryCurrentManaSlider;
    public Text
        InventoryHealthText,
        InventoryManaText;

    private int
        SelectorMask,
        AttackSafe,
        WallMask,
        frameCount,
        TotalMagicUses;
    public int
        BlockRoll,
        CounterRoll;

    public RaycastHit
        MoveTo;

    private Ray
        Hitter;

    public GameObject
        QuickSlot1,
        QuickSlot2,
        QuickSlot3;

    private HealthScript
        MyHealthScript;

    public Slider
        HealthIndicator;

    SphereCollider detectionArea;

    Camera MainCamera;
    //contains player character info and controls
    public DistanceChecker distanceChecker;
    public List<Transform> WeaponHooks = new List<Transform>();
    public List<Transform> OffHandHooks = new List<Transform>();
    
    public void ShowWeapons(GameObject weapon)
    {
        WeaponHooks.ForEach(e => {
            if (e.childCount > 0)
            {
                Transform child = e.GetChild(0);
                if (child != null)
                    {
                        Destroy(child.gameObject);
                    }
            }
            Instantiate(weapon, e);
        });
        OffHandHooks.ForEach(e =>
        {
            if (e.childCount > 0)
            {
                Transform child = e.GetChild(0);
                if (child != null)
                {
                    Destroy(child.gameObject);
                }
            }
        });
    }
    public void ShowWeapons(GameObject weapon, GameObject offHandWeapon)
    {
        ShowWeapons(weapon);
        OffHandHooks.ForEach(e => {
            Instantiate(offHandWeapon, e); });
    }
    

    public int CalculateWeaponDamage(WeaponScript weapon)
    {

        int damage = 0;
        if (WeaponLevels.ContainsKey(weapon.WeaponCategory))
        {
            if (!gameObject.GetComponent<DisarmedScript>())
            {
                damage = (weapon.CalcRules(SpecialRulesEnum.ExtraDamage) + 1) * (WeaponLevels[EquippedWeaponType] - 1 + weapon.BaseDamage);
            }
            else damage = ((weapon.CalcRules(SpecialRulesEnum.ExtraDamage) + 1) * (WeaponLevels[EquippedWeaponType] - 1 + weapon.BaseDamage)) / (gameObject.GetComponent<DisarmedScript>().strength + 1);
        }
        else
        {
            if (!gameObject.GetComponent<DisarmedScript>())
            {
                damage = (weapon.CalcRules(SpecialRulesEnum.ExtraDamage) + 1) * (weapon.BaseDamage);
            }
            else damage = ((weapon.CalcRules(SpecialRulesEnum.ExtraDamage) + 1) * (weapon.BaseDamage)) / (gameObject.GetComponent<DisarmedScript>().strength + 1);
        }
        return damage;
    }

    public WeaponData SetUpWeaponData(GameObject weapon)
    {
        WeaponScript currentWeapon = weapon.GetComponent<WeaponScript>();


        // setting these up so that we can turn them into arrays then save the inventory as an array
        List<EnchantmentType> currentEnchants = new List<EnchantmentType>();
        List<int> currentEnchantStrengths = new List<int>();
        List<int> currentEnchantCosts = new List<int>();
        List<int> currentEnchantPenalties = new List<int>();

        // this is for saving all the weapon enchantments
        foreach (EnchantmentScript enchant in currentWeapon.gameObject.GetComponents<EnchantmentScript>())
        {
            currentEnchants.Add(enchant.EnchantType);
            currentEnchantStrengths.Add(enchant.Strength);
            currentEnchantCosts.Add(enchant.ManaCost);
            currentEnchantPenalties.Add(enchant.ManaPenalty);
        }


        WeaponData currentData = new WeaponData()
        {

            reactionCount = currentWeapon.reactionCount,

            weaponType = currentWeapon.WeaponCategory,

            weaponVariant = currentWeapon.WeaponVariation,

            enchants = currentEnchants.ToArray(),

            enchantmentStrengths = currentEnchantStrengths.ToArray(),

            enchantmentCosts = currentEnchantCosts.ToArray(),

            enchantmentPenalties = currentEnchantPenalties.ToArray(),

            Modifiers = currentWeapon.Modifiers.ToArray()
        };





        return currentData;
    }

    public void Save()
    {

        WeaponData equippedWeaponData = new WeaponData();

        if (EquippedWeapon != null)
        {

            equippedWeaponData = SetUpWeaponData(EquippedWeapon);

        }

        IDictionary<int, WeaponData> inventoryWeaponContents = new Dictionary<int, WeaponData>();
        IDictionary<int, ConsumableInfo> inventoryConsumableContents = new Dictionary<int, ConsumableInfo>();

        foreach (Transform child in Backpack.transform)
        {


            if (child.childCount > 0)
            {
                if (child.GetChild(0).gameObject.GetComponent<WeaponScript>())
                {
                    WeaponData currentData = SetUpWeaponData(child.GetChild(0).gameObject);

                    inventoryWeaponContents.Add(child.GetSiblingIndex(), currentData);
                }
                else if (child.GetChild(0).gameObject.GetComponent<ConsumableScript>())
                {
                    ConsumableScript currentConsumable = child.GetChild(0).gameObject.GetComponent<ConsumableScript>();
                    ConsumableInfo currentConsumableInfo = new ConsumableInfo()
                    {
                        amountCarried = currentConsumable.NumberHeld,
                        consumableType = currentConsumable.Type
                    };
                    inventoryConsumableContents.Add(child.GetSiblingIndex(), currentConsumableInfo);
                }
            }
        }
        WeaponData[] weaponsCarried = new WeaponData[22];
        ConsumableInfo[] consumablesCarried = new ConsumableInfo[22];
        foreach (int key in inventoryWeaponContents.Keys)
        {
            weaponsCarried[key] = inventoryWeaponContents[key];
        }

        foreach (int key in inventoryConsumableContents.Keys)
        {
            consumablesCarried[key] = inventoryConsumableContents[key];
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + SaveFileScript.CurrentSaveFile.ToString() + "/" + gameObject.GetComponent<SpecificCharacterScript>().CharacterName + ".dat");

        // needs to be last so that we can push everything to it.
        PlayerData Player1Data = new PlayerData()
        {
            MaxHealth = MyHealthScript.MaxHp,
            MaxMana = MaxMana,

            CurrentHealth = MyHealthScript.CurrentHP,
            TotalDistanceTraveled = (TotalDistanceTraveled + DistanceFromCheckpoint + (AttackCost * NumberOfAttacks)),
            CurrentMana = CurrentMana,


            TotalHealthRegened = gameObject.GetComponent<CharacterHealthScript>().TotalHealthRegened,
            TotalManaRegened = TotalManaRegened,

            leveledWeaponType = WeaponLevels.Keys.ToArray(),
            LevelOfWeapon = WeaponLevels.Values.ToArray(),

            WeaponsWithTotalDamages = TotalWeaponDamages.Keys.ToArray(),
            TotalDamage = TotalWeaponDamages.Values.ToArray(),

            currentCheckpoint = ConvertToArray(CurrentCheckpoint),

            weaponEquipped = equippedWeaponData,

            weaponsCarried = weaponsCarried,
            consumablesCarried = consumablesCarried,

            currentPosition = ConvertToArray(transform.position),
            currentRotation = transform.rotation.eulerAngles.y


        };







        bf.Serialize(file, Player1Data);
        file.Close();



        SaveFileScript.FinishedSaving[gameObject] = true;
    }

    // put this in for damage resist and reflect.
    public void BlockAndCounter(GameObject Attacker, int totalDamageTaken, DamageType damageType)
    {
        Debug.Log(totalDamageTaken);
        if (!gameObject.GetComponent<ShatteredScript>())
        {
            if (EquippedWeapon != null)
            {
                if (EquippedWeapon.GetComponent<WeaponScript>().SpecialRules.Contains(SpecialRulesEnum.Counter) && damageType == DamageType.Physical)
                {
                    int mult = EquippedWeapon.GetComponent<WeaponScript>().CalcRules(SpecialRulesEnum.Counter);
                    int roll = UnityEngine.Random.Range(1, 21);

                    if (roll >= 20 - (mult * 5))
                    {
                        Attacking(Attacker);
                    }
                }
                if (EquippedWeapon.GetComponent<WeaponScript>().SpecialRules.Contains(SpecialRulesEnum.Block))
                {
                    //get the block multiplier so that we can calculate defense
                    int mult = EquippedWeapon.GetComponent<WeaponScript>().CalcRules(SpecialRulesEnum.Block);


                    int DamageBlocked = (CurrentWeaponLevel * mult);
                    if (totalDamageTaken <= DamageBlocked)
                    {
                        DamageBlocked = totalDamageTaken - 1;
                    }
                    TotalWeaponDamages[EquippedWeaponType] += DamageBlocked;

                    totalDamageTaken -= DamageBlocked;
                }
            }
            gameObject.GetComponent<HealthScript>().Damaged(totalDamageTaken, damageType);
        }
        else gameObject.GetComponent<HealthScript>().Damaged((totalDamageTaken * gameObject.GetComponent<ShatteredScript>().strength + 2) / 2, damageType);
    }

    void OnDestroy()
    {
        if (gameObject.GetComponent<SpecificCharacterScript>().IsCharacter)
        {
            if (CameraScript.GameController != null)
                CameraScript.GameController.PlayerDestroyed();
            if (CharacterButton != null)
                CharacterButton.gameObject.GetComponent<CharacterSelectButtonScript>().Deceased();
            // need to put in a save function so that when we return to town the character shows up with their decreased stats
        }
    }

    public void SetInventoryBarsAndText()
    {
        InventoryHealthSlider.maxValue = gameObject.GetComponent<HealthScript>().MaxHp;
        InventoryHealthSlider.value = gameObject.GetComponent<HealthScript>().CurrentHP;
        InventoryHealthText.text = (/*gameObject.GetComponent<HealthScript>().CurrentHP.ToString() + "/" +*/ gameObject.GetComponent<HealthScript>().MaxHp.ToString());
        //not sure why that part is commented out, but will revisit with playtesting if I remember.
        InventoryMaxManaSlider.maxValue = MaxMana;
        InventoryMaxManaSlider.value = ModifiedMaxMana;

        InventoryCurrentManaSlider.maxValue = ModifiedMaxMana;
        InventoryCurrentManaSlider.value = CurrentMana;

        if (MaxMana != ModifiedMaxMana)
        {
            InventoryManaText.text = CurrentMana.ToString() + "/" + ModifiedMaxMana.ToString() + "/" + MaxMana.ToString();
        }
        else
        {
            InventoryManaText.text = CurrentMana.ToString() + "/" + MaxMana.ToString();
        }
    }

    public void SpendMana(int ManaSpent)
    {
        CurrentMana -= ManaSpent;
        StartCoroutine(SetManaAndMaxManaBars());
    }

    private Vector3 ConvertToVector(float[] array)
    {
        if (array.Length == 3)
        {
            Vector3 currentVector = new Vector3(array[0], array[1], array[2]);
            return currentVector;
        }
        else throw new Exception();
    }

    private float[] ConvertToArray(Vector3 vector)
    {
        float[] current = new float[3] { vector.x, vector.y, vector.z };
        return current;
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/" + SaveFileScript.CurrentSaveFile.ToString() + "/" + gameObject.GetComponent<SpecificCharacterScript>().CharacterName + ".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + SaveFileScript.CurrentSaveFile.ToString() + "/" + gameObject.GetComponent<SpecificCharacterScript>().CharacterName + ".dat", FileMode.Open);
            PlayerData Player1Data = (PlayerData)bf.Deserialize(file);
            file.Close();

            MaxMana = Player1Data.MaxMana;
            MaxHealth = Player1Data.MaxHealth;

            CurrentMana = Player1Data.CurrentMana;
            Debug.Log(Player1Data.TotalDistanceTraveled + " distance");
            MyHealthScript.CurrentHP = Player1Data.CurrentHealth;



            gameObject.transform.eulerAngles.Set(0, Player1Data.currentRotation, 0);
            gameObject.transform.position = ConvertToVector(Player1Data.currentPosition);

            CurrentCheckpoint = gameObject.transform.position;
            TotalDistanceTraveled = Player1Data.TotalDistanceTraveled;

            WeaponLevels.Clear();
            TotalWeaponDamages.Clear();
            if (Player1Data.leveledWeaponType[0] != WeaponType.Error && Player1Data.leveledWeaponType != null && Player1Data.leveledWeaponType.Length != 0 && Player1Data.LevelOfWeapon != null && Player1Data.LevelOfWeapon.Length != 0)
            {
                for (int i = 0; i < Player1Data.leveledWeaponType.Length; i++)
                {
                    if (!WeaponLevels.ContainsKey(Player1Data.leveledWeaponType[i]))
                    {
                        WeaponLevels.Add(Player1Data.leveledWeaponType[i], Player1Data.LevelOfWeapon[i]);
                    }
                    else WeaponLevels[Player1Data.leveledWeaponType[i]] = Player1Data.LevelOfWeapon[i];
                }
                for (int i = 0; i < Player1Data.TotalDamage.Length; i++)
                {
                    TotalWeaponDamages.Add(Player1Data.WeaponsWithTotalDamages[i], Player1Data.TotalDamage[i]);
                }
            }
            foreach (Transform itemSlot in Backpack.transform)
            {
                if (itemSlot.childCount != 0)
                {
                    Destroy(itemSlot.GetChild(0).gameObject);
                }
                if (Player1Data.weaponsCarried[itemSlot.GetSiblingIndex()] != null)
                {
                    GameObject weapon = WeaponFromData(Player1Data.weaponsCarried[itemSlot.GetSiblingIndex()]);

                    weapon.transform.SetParent(itemSlot);
                    weapon.transform.localPosition = Vector3.zero;
                    weapon.transform.localScale = Vector3.one;

                }
                else if (Player1Data.consumablesCarried[itemSlot.GetSiblingIndex()] != null)
                {
                    GameObject consumable = ConsumableFromData(Player1Data.consumablesCarried[itemSlot.GetSiblingIndex()]);
                    consumable.transform.SetParent(itemSlot);

                    consumable.transform.localPosition = Vector3.zero;
                    consumable.transform.localScale = Vector3.one;
                }
            }
            if (WeaponSlot.transform.childCount != 0)
            {
                foreach (Transform weapon in WeaponSlot.transform)
                {
                    Destroy(weapon.gameObject);
                }
            }
            if (Player1Data.weaponEquipped != null)
            {
                EquippedWeapon = WeaponFromData(Player1Data.weaponEquipped);
                if (EquippedWeapon != null)
                {
                    EquippedWeapon.transform.SetParent(WeaponSlot.transform);
                    EquippedWeapon.transform.localPosition = Vector3.zero;

                    EquippedWeapon.transform.localScale = Vector3.one;
                    StartCoroutine(EquipAfterFrame());
                }
            }
            TotalManaRegened = Player1Data.TotalManaRegened;
            gameObject.GetComponent<CharacterHealthScript>().TotalHealthRegened = Player1Data.TotalHealthRegened;

            MyHealthScript.MaxHp = MaxHealth;


        }
        IsLoaded = true;
        StartCoroutine(TurnBackOn());
    }

    IEnumerator EquipAfterFrame()
    {
        yield return null;
        WeaponSlot.GetComponent<WeaponSlotScript>().EquipItem();
    }

    public GameObject ConsumableFromData(ConsumableInfo consumable)
    {
        Debug.Log(consumable.consumableType.ToString());
        GameObject consumableReturn = Instantiate(Resources.Load("ConsumableSprites/" + consumable.consumableType.ToString() + "Sprite") as GameObject);
        consumableReturn.GetComponent<ConsumableScript>().NumberHeld = consumable.amountCarried;
        return consumableReturn;
    }

    public GameObject WeaponFromData(WeaponData weapon)
    {
        if (weapon.weaponType == WeaponType.Error || weapon.weaponType == WeaponType.Fisticuffs)
        {
            return null;
        }
        GameObject weaponReturn = new GameObject();

        if (weapon.weaponVariant != null)
        {
            Debug.Log(weapon.weaponVariant);
            weaponReturn = Instantiate(Resources.Load("WeaponSprites/" + weapon.weaponType.ToString() + "/" + weapon.weaponVariant.Replace(" ", string.Empty) + "Sprite") as GameObject);
        }
        else Debug.Log(weapon.weaponVariant);
        if (weapon.enchants != null)
        {
            for (int i = 0; i < weapon.enchants.Length; i++)
            {
                Debug.Log(weapon.enchants[i].ToString());
                Type mytype = Type.GetType(weapon.enchants[i].ToString() + "Script");
                weaponReturn.AddComponent(mytype);
                EnchantmentScript[] myEnchants = weaponReturn.GetComponents<EnchantmentScript>();
                if (myEnchants != null && myEnchants.Length != 0)
                {
                    myEnchants[i].Strength = weapon.enchantmentStrengths[i];
                    myEnchants[i].ManaCost = weapon.enchantmentCosts[i];
                    Debug.Log(weapon.enchantmentPenalties.ToString());
                    myEnchants[i].ManaPenalty = weapon.enchantmentPenalties[i];
                }
            }
            for (int i = 0; i < weapon.Modifiers.Length; i++)
            {
                if (weapon.Modifiers[i] != SpecialRulesEnum.None)
                {
                    weaponReturn.GetComponent<WeaponScript>().Modifiers.Add(weapon.Modifiers[i]);
                }
            }
        }
        return weaponReturn;
    }

    public IEnumerator ButtonCreated()
    {
        yield return new WaitWhile(() => MaxMana == 0);

        CharacterButtonManaIndicator.maxValue = MaxMana;
        CharacterButtonManaIndicator.value = MaxMana;
        Debug.Log("Added Character Button" + CharacterButtonManaIndicator.transform.parent.name);

    }

    public void IHitAMonster(int DamageDealt, GameObject target)
    {
        if (TotalWeaponDamages.ContainsKey(EquippedWeaponType))
        {
            if (WeaponLevels.ContainsKey(EquippedWeaponType))
            {
                SetDamageNeededToLevel();
                if (EquippedWeapon != null)
                {
                    if (target != null)
                        EquippedWeapon.GetComponent<WeaponScript>().StartOnHit(DamageDealt, target);
                }
                TotalWeaponDamages[EquippedWeaponType] += DamageDealt;
                if (TotalWeaponDamages[EquippedWeaponType] >= DamageNeededToLevelUp)
                {
                    gameObject.GetComponent<HealthScript>().ShowDamageTaken("Level Up " + EquippedWeaponType, DamageType.Physical);
                    TotalWeaponDamages[EquippedWeaponType] = 0;
                    WeaponLevels[EquippedWeaponType]++;

                    CurrentWeaponLevel = WeaponLevels[EquippedWeaponType];
                    if (EquippedWeapon != null)
                    {
                        WeaponScript equippedWeaponScript = EquippedWeapon.GetComponent<WeaponScript>();
                        AverageDamage = CalculateWeaponDamage(equippedWeaponScript);
                    }
                    else AverageDamage = WeaponLevels[WeaponType.Fisticuffs] + 4;
                }
            }
        }
        else Debug.Log("TotalWeaponDamages does not contain " + EquippedWeaponType.ToString());
    }

    void OnEnable()
    {
        CameraScript.StartinPlayerTurn += StartingTurn;
        CameraScript.UpdatingNavMesh += EndingTurn;
        SaveFileScript.StartSave += Save;
        SaveFileScript.StartLoad += Load;
        SaveFileScript.FinishedSaving.Add(gameObject, true);
    }

    void OnDisable()
    {
        CameraScript.StartinPlayerTurn -= StartingTurn;
        CameraScript.UpdatingNavMesh -= EndingTurn;


        SaveFileScript.StartSave -= Save;
        SaveFileScript.StartLoad -= Load;
        SaveFileScript.FinishedSaving.Remove(gameObject);
    }

    public void LoadingNewLevel()
    {
        StartCoroutine(TemporaryTurnOff());
        IsLoaded = false;
    }

    public void LoadedNewLevel()
    {
        StartCoroutine(TurnBackOn());
        IsLoaded = true;
        CurrentCheckpoint = gameObject.transform.position;
        if (!SaveFileScript.loading)
        {
            TotalDistanceTraveled = 0;
        }
    }

    public IEnumerator TemporaryTurnOff()
    {
        if (gameObject.GetComponent<NavMeshAgent>() != null)
        {
            Destroy(gameObject.GetComponent<NavMeshAgent>());

        }
        yield return null;
        gameObject.GetComponent<Rigidbody>().isKinematic = !IsLoaded;
        gameObject.GetComponent<NavMeshObstacle>().enabled = true;
    }

    public IEnumerator TurnBackOn()
    {
        gameObject.GetComponent<NavMeshObstacle>().enabled = false;

        yield return null;
        gameObject.GetComponent<Rigidbody>().isKinematic = IsLoaded;
        gameObject.AddComponent<NavMeshAgent>();
        gameObject.GetComponent<NavMeshAgent>().baseOffset = 1;

        CurrentCheckpoint = gameObject.transform.position;
    }

    void EndingTurn()
    {
        IsTurn = false;
        //
        if (RemainingMovement >= 1)
        {
            ManaRegen((int)RemainingMovement);
        }
        RemainingMovement = 0.0f;


        MovementIndicator.localScale = InActive;
        AttackCostIndicator.localScale = InActive;
        RangeIndicator.SetActive(false);

        StartCoroutine(TemporaryTurnOff());

        if (EquippedWeapon != null)
            EquippedWeapon.GetComponent<WeaponScript>().ReactionReset();
    }

    public void StartingTurn()
    {
        HasNoMovement = false;
        NumberOfAttacks = 0;
        TotalDistanceTraveled = 0;
        CurrentCheckpoint = gameObject.transform.position;
        IsTurn = true;
        if (CurrentMana < ModifiedMaxMana)
        {
            if (MaxMana / 100 > 1)
            {
                ManaRegen(MaxMana / 100);
            }
            else ManaRegen(1);
        }
        StartCoroutine(TurnBackOn());
    }

    void Awake()
    {
        mySheet = gameObject.GetComponent<SpecificCharacterScript>();
        MyHealthScript = gameObject.GetComponent<HealthScript>();
    }

    void Start()
    {
        IsLoaded = false;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        if (MaxMana == 0)
            MaxMana = 10;
        if (ModifiedMaxMana == 0)
            ModifiedMaxMana = MaxMana;
        CurrentMana = MaxMana;
        SelectorMask = (1 << 8) | (1 << 9) | (1 << 11);
        MainCamera = CameraScript.GameController.GetComponent<Camera>();
        HasNoMovement = false;
        IsTurn = true;
        MaxMovement = 10;
        //set RemainingMovement
        modifiedMaxMovement = MaxMovement;
        RemainingMovement = modifiedMaxMovement;
        MaxHealth = 30;
        CurrentHP = MaxHealth;
        MyHealthScript.MaxHp = MaxHealth;
        MyHealthScript.Defense = 2;
        MyHealthScript.IsReady();
        MyHealthScript.isPlayer = true;

        EquippedWeaponType = WeaponType.Fisticuffs;

        if (!WeaponLevels.ContainsKey(WeaponType.Fisticuffs))
            WeaponLevels.Add(WeaponType.Fisticuffs, 1);
        Unarmed = true;

        EquipNewWeapon();

        //make it easy to reference ourself
        Self = gameObject;
        //set Checkpoint for calculating movement
        CurrentCheckpoint = transform.position;
        //setting up a zero value to control momentum
        InActive = new Vector3(0.0f, 0.0f, 0.0f);
        //getting materials so that we can reference them later
        EnoughMovement = Resources.Load("Materials/NewBlueIndicator", typeof(Material)) as Material;
        NotEnoughMovement = Resources.Load("Materials/NewRedIndicator", typeof(Material)) as Material;
        //set rigidbody
        Mine = gameObject.GetComponent<Rigidbody>();
        //make it so that we don't automatically overlap everything
        MoveTowards = transform.position;
        WallMask = 1 << 9;
        // Start the coroutine that assigns mana and max mana values
        StartCoroutine(SetManaAndMaxManaBars());
        InventoryManager.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        InventoryManager.GetComponent<Canvas>().worldCamera = InventoryActiveCameraScript.InventoryCamera;
        detectionArea = distanceChecker.GetComponent<SphereCollider>();
    }

    public int GetMagicLevel()
    {
        if (WeaponLevels.ContainsKey(WeaponType.Magic))
        {
            return WeaponLevels[WeaponType.Magic];
        }
        else return 1;
    }

    public IEnumerator SetManaAndMaxManaBars()
    {
        yield return new WaitWhile(() => (CharacterButtonMaxManaIndicator == null && CharacterButtonManaIndicator == null));
        CharacterButtonMaxManaIndicator.maxValue = MaxMana;
        CharacterButtonMaxManaIndicator.value = ModifiedMaxMana;
        CharacterButtonManaIndicator.maxValue = ModifiedMaxMana;
        CharacterButtonManaIndicator.value = CurrentMana;
    }

    private IEnumerator AttackAnimation()
    {
        switch (EquippedWeaponType)
        {
            case WeaponType.Error:
                Debug.Log("Error weapon Detected");
                break;
            case WeaponType.Fisticuffs:
                break;
            case WeaponType.SwordAndShield:
                break;
            case WeaponType.GreatAxe:
                break;
            case WeaponType.Ranged:
                break;
            case WeaponType.Magic:
                break;
            case WeaponType.Dagger:
                break;
            default:
                break;
        }
        yield return null;
    }

    public void Attacking(GameObject Target)
    {
        if (gameObject.GetComponent<NavMeshAgent>())
        {
            HitTarget = null;
            StillMoving = false;
        }

        StartCoroutine(AttackAnimation());

        // rolling a 20 sided dice for if we get a crit or a fumble.
        int roll = UnityEngine.Random.Range(1, 21);

        // calculating our crit chance.
        int critChance = 1;
        if (EquippedWeapon != null)
        {
            critChance += EquippedWeapon.GetComponent<WeaponScript>().CalcRules(SpecialRulesEnum.IncreasedCrits);
            if (gameObject.GetComponent<DamageBuffScript>())
            {
                critChance += 1;
            }
        }

        //if we fumble.
        if (roll == 1)
        {
            DamageDealt = AverageDamage / 2;
        }
        // if we crit.
        else if (roll > 20 - critChance)
        {
            int mult = 2;
            if (EquippedWeapon != null)
            {
                mult += EquippedWeapon.GetComponent<WeaponScript>().CalcRules(SpecialRulesEnum.BetterCrits);
                EquippedWeapon.GetComponent<WeaponScript>().OnCrit(Target);
            }

            DamageDealt = AverageDamage * mult;
        }
        else
        {
            DamageDealt = AverageDamage;
        }
        // testing for damage buff script and modifying damage accordingly.
        if (gameObject.GetComponent<DamageBuffScript>())
        {
            gameObject.GetComponent<DamageBuffScript>().IncreaseDamage(DamageDealt, Target, gameObject, DamageType.Physical);
        }
        //if we don't have the damage buff put out damage
        else Target.GetComponent<HealthScript>().InitHit(DamageDealt, gameObject, DamageType.Physical);


    }

    void MovementCalculation()
    {
        if (!StartManagerScript.StartGame)
        {
            CurrentCheckpoint = gameObject.transform.position;
        }
        //calculate how much movement we have left
        if (StartManagerScript.StartGame)
        {
            DistanceFromCheckpoint = Vector3.Distance(CurrentCheckpoint, transform.position);
        }
        // to try to cut down on losing movement to shaking
        if (DistanceFromCheckpoint > 0.5f)
        {
            TotalDistanceTraveled += DistanceFromCheckpoint;
            CurrentCheckpoint = transform.position;
            DistanceFromCheckpoint = Vector3.Distance(CurrentCheckpoint, transform.position);
        }
        RemainingMovement = modifiedMaxMovement - (TotalDistanceTraveled + DistanceFromCheckpoint);
    }

    void HandleRange()
    {
        if (ActivePlayer == gameObject)
        {
            RangeIndicator.SetActive(true);
            detectionArea.radius = Range;
        }
        else RangeIndicator.SetActive(false);
    }

    void HandleMovement()
    {
        Vector3 pos = gameObject.transform.position;
        if (Input.GetKey("a") || Input.GetKey("d") || Input.GetKey("w") || Input.GetKey("s"))
        {
            CM = true;
            Vector3 des = Vector3.zero;
            if (Input.GetKey("a"))
            {
                des += Vector3.left;
            }
            if (Input.GetKey("d"))
            {
                des += Vector3.right;
            }
            if (Input.GetKey("w"))
            {
                des += Vector3.forward;
            }
            if (Input.GetKey("s"))
            {
                des += Vector3.back;
            }
            gameObject.GetComponent<NavMeshAgent>().SetDestination(des + pos);
        }
        else
        {
            if (gameObject.GetComponent<NavMeshAgent>() && IsLoaded && gameObject.GetComponent<NavMeshAgent>().isOnNavMesh)
                gameObject.GetComponent<NavMeshAgent>().SetDestination(gameObject.transform.position);
        }
    }

    void RaycastTarget()
    {
        if (Input.mousePresent && Input.GetMouseButtonDown(0) && !CameraScript.InventoryOpen)
        {
            Hitter = MainCamera.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(Hitter, out MoveTo, Mathf.Infinity, SelectorMask);
            if (MoveTo.collider != null)
            {
                HitTarget = MoveTo.collider.gameObject.transform.root.gameObject;
            }
            else
            {
                HitTarget = null;
            }
        }
    }

    void Update()
    {
        //Grab the ActivePlayer Reference.
        if (CameraScript.GameController != null)
            if (CameraScript.GameController.ActivePlayer != null)
                ActivePlayer = CameraScript.GameController.ActivePlayer;

        if (frameCount <= 5)
            frameCount++;

        // check to see if it is our turn
        if (IsTurn == true && IsLoaded)
        {
            MovementCalculation();
            HandleRange();
            if (ActivePlayer == gameObject)
            {
                if (IsTurn)
                {
                    HandleMovement();
                }
            }

            

            if (ActivePlayer != Self && frameCount > 5)
            {
                Mine.velocity = Vector3.zero;
            }


            // test if we are what the player is moving.
            if (RemainingMovement > 0.0f && gameObject == ActivePlayer)
            {
                if (RemainingMovement >= AttackCost)
                {
                    MovementIndicator.GetComponent<MeshRenderer>().material = EnoughMovement;
                }
                else
                {
                    MovementIndicator.GetComponent<MeshRenderer>().material = NotEnoughMovement;
                }
                bool StartedTouch = Input.touchSupported && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
                bool nottouchingThisPlayer = false;
                if (Input.touchSupported)
                {
                    nottouchingThisPlayer = !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
                }
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    if (
                        ((StartedTouch && nottouchingThisPlayer) ||(Input.mousePresent && Input.GetMouseButtonDown(0))
                        ) && !EventSystem.current.IsPointerOverGameObject())
                    {
                        StillMoving = true;
                        AttackSafe = 0;
                        CM = false;
                    }
                    if (StillMoving == true)
                    {
                        RaycastTarget();
                        
                        if (HitTarget != null && !CM)
                        {

                            if (HitTarget.CompareTag("Shiny"))
                            {
                                if (Vector3.Distance(HitTarget.transform.position, gameObject.transform.position) > 1.3f)
                                {
                                    MoveTowards = HitTarget.transform.position;
                                    gameObject.GetComponent<NavMeshAgent>().destination = MoveTowards;
                                }
                                if (Vector3.Distance(HitTarget.transform.position, gameObject.transform.position) <= 1.3f)
                                {
                                    gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                                    MoveTowards = gameObject.transform.position;
                                    gameObject.GetComponent<NavMeshAgent>().destination = MoveTowards;
                                    if (HitTarget.GetComponent<DroppedShinyScript>())
                                        HitTarget.GetComponent<DroppedShinyScript>().PickedUp();
                                    else Debug.Log("You forgot to put DroppedShinyScript On this item");
                                }
                            }
                            else if (HitTarget.CompareTag("EntranceExit"))
                            {
                                if (Vector3.Distance(gameObject.transform.position, HitTarget.transform.position) > 2f)
                                {
                                    MoveTowards = HitTarget.transform.position;
                                    gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().destination = MoveTowards;
                                    gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().stoppingDistance = 2f;
                                }
                                if (Vector3.Distance(gameObject.transform.position, HitTarget.transform.position) <= 2f)
                                {
                                    if (HitTarget.GetComponent<ExitScript>())
                                    {
                                        HitTarget.GetComponent<ExitScript>().LoadLevelCheck();
                                        StillMoving = false;
                                    }
                                    if (HitTarget.GetComponent<EntranceScript>())
                                    {
                                        HitTarget.GetComponent<EntranceScript>().LoadLevelCheck();
                                        StillMoving = false;
                                    }
                                    if (HitTarget.GetComponent<LocalEntranceScript>())
                                    {
                                        HitTarget.GetComponent<LocalEntranceScript>().LoadLevelCheck();
                                        StillMoving = false;
                                    }
                                    if (HitTarget.GetComponent<LocalExitScript>())
                                    {
                                        HitTarget.GetComponent<LocalExitScript>().LoadLevelCheck();
                                        StillMoving = false;
                                    }
                                }
                            }
                            else if (HitTarget.CompareTag("Player") && EquippedWeaponType == WeaponType.Magic)
                            {
                                //first we check if we have enough movement.
                                if (RemainingMovement >= AttackCost)
                                {
                                    //then we check if we are in range
                                    if (Vector3.Distance(gameObject.transform.position, HitTarget.transform.position) - .5 <= Range)
                                    {
                                        //now we check Line of sight(los from here).
                                        Physics.Raycast(HitTarget.transform.position, gameObject.transform.position - HitTarget.transform.position, out Strucken, Vector3.Distance(HitTarget.transform.position, gameObject.transform.position), WallMask);
                                        if (Strucken.collider == null)
                                        {
                                            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                                            MoveTowards = gameObject.transform.position;
                                            if (RemainingMovement >= AttackCost)
                                            {
                                                if (AttackSafe == 0)
                                                {

                                                    EquippedWeapon.GetComponent<MagicScript>().Effect(HitTarget);
                                                    AttackSafe++;
                                                    TotalDistanceTraveled += AttackCost;
                                                    TotalMagicUses++;
                                                    if (TotalMagicUses >= DamageNeededToLevelUp)
                                                    {
                                                        WeaponLevels[WeaponType.Magic]++;
                                                        SetDamageNeededToLevel();
                                                    }
                                                    HitTarget = null;
                                                    StillMoving = false;

                                                }
                                            }
                                        }
                                        else FixLOS(HitTarget);
                                    }
                                    else
                                    {
                                        //get within range.
                                        MoveTowards = MoveTo.collider.gameObject.transform.position;
                                        gameObject.GetComponent<NavMeshAgent>().stoppingDistance = Range + .4f;
                                        if (gameObject.GetComponent<NavMeshAgent>().enabled)
                                            gameObject.GetComponent<NavMeshAgent>().destination = MoveTowards;
                                    }
                                }


                            }
                            else if (HitTarget.CompareTag("Monster") && EquippedWeaponType != WeaponType.Magic)
                            {
                                //first test for if AttackSafe is 0;
                                if (AttackSafe == 0)
                                {
                                    //check for if we have enough movement.
                                    if (RemainingMovement >= AttackCost)
                                    {
                                        //if we are close enough to attack
                                        if (distanceChecker.Contains(HitTarget))
                                        {
                                            // first we check for if we can hit Target directly.

                                            //Mark down that we attacked
                                            TotalDistanceTraveled += AttackCost;
                                            HitTarget.GetComponent<UnityEngine.AI.NavMeshObstacle>().carving = true;
                                            //start by not moving anymore
                                            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                                            MoveTowards = gameObject.transform.position;

                                            /*if (EquippedWeaponType == WeaponType.Wand)
                                            {
                                                //trigger Wand
                                                EquippedWeapon.GetComponent<WandScript>().Effect(HitTarget);
                                            }
                                            else*/
                                            {
                                                //Attack
                                                Attacking(HitTarget);
                                            }
                                            //this makes sure we don't attack again until the player wants to.
                                            AttackSafe++;
                                            //if we didn't destroy the target.
                                            if (HitTarget != null)
                                            {
                                                HitTarget.GetComponent<UnityEngine.AI.NavMeshObstacle>().carving = true;
                                            }


                                        }

                                        //if we aren't close enough
                                        else
                                        {//
                                            MoveTowards = MoveTo.collider.gameObject.transform.position;
                                            gameObject.GetComponent<NavMeshAgent>().stoppingDistance = Range + HitTarget.GetComponent<MonsterScript>().RangeOffset - .1f;
                                            if (gameObject.GetComponent<NavMeshAgent>().enabled)
                                                gameObject.GetComponent<NavMeshAgent>().destination = MoveTowards;
                                        }
                                    }
                                }

                            }
                        }
                    }
                    if (ActivePlayer != Self && gameObject.GetComponent<NavMeshAgent>().velocity == Vector3.zero)
                    {

                        MoveTowards = gameObject.transform.position;
                        StillMoving = false;
                    }

                }
                if (HitTarget == null && gameObject.GetComponent<NavMeshAgent>() != null)
                {
                    MoveTowards = gameObject.transform.position;
                }

            }
            //make sure things don't move after we cannot move them
            if (RemainingMovement <= 0.0f)
            {
                HasNoMovement = true;
                Mine.velocity = InActive;
                MoveTowards = gameObject.transform.position;
                StillMoving = false;

            }
        }
        if (gameObject.GetComponent<NavMeshAgent>() != null && (IsTurn == false || RemainingMovement <= 0 || (ActivePlayer != gameObject && gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().velocity == Vector3.zero)))
        {
            MoveTowards = gameObject.transform.position;

            if (gameObject.GetComponent<NavMeshAgent>() && gameObject.GetComponent<NavMeshAgent>().isActiveAndEnabled && IsLoaded && gameObject.GetComponent<NavMeshAgent>().isOnNavMesh)
                gameObject.GetComponent<NavMeshAgent>().destination = transform.position;

            gameObject.GetComponent<Rigidbody>().velocity = InActive;
            StillMoving = false;
        }
    }
    void FixLOS(GameObject target)
    {

    }
    void MoveToSpot()
    {
        gameObject.GetComponent<NavMeshAgent>().stoppingDistance = 0;
        MoveTowards = MoveTo.point;
        if (gameObject.GetComponent<NavMeshAgent>() != null)
            gameObject.GetComponent<NavMeshAgent>().SetDestination(MoveTowards);
    }

    /*IEnumerator TurnOnOffNavigation(bool TurnOff)
    {
        if (!TurnOff)
        {
            gameObject.GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = false;
            float PauseTimer = .3f + Time.time;
            yield return new WaitWhile(() => Time.time <= PauseTimer);
            gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
            Debug.Log("Navigation Enabled");
        }
        if (TurnOff)
        {
            gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
            float PauseTimer = .3f + Time.time;
            yield return new WaitWhile(() => Time.time <= PauseTimer);
            gameObject.GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = true;
            Debug.Log("Navigation Disabled");
        }
        AgentOn = !TurnOff;
    }*/



    public void EquipNewWeapon()
    {
        if (WeaponSlot != null && WeaponSlot.transform.childCount != 0)
        {
            EquippedWeapon = WeaponSlot.transform.GetChild(0).gameObject;
            if (EquippedWeapon.GetComponent<WeaponScript>() != null)
            {
                TotalManaPenalty = EquippedWeapon.GetComponent<WeaponScript>().ManaPenalty;
                ModifiedMaxMana = MaxMana - TotalManaPenalty;

                if (CurrentMana > ModifiedMaxMana)
                {
                    CurrentMana = ModifiedMaxMana;
                }

                StartCoroutine(SetManaAndMaxManaBars());
                SetInventoryBarsAndText();

                // if the weapon has a set stat, if not choose the higher of the two
                if (EquippedWeapon.GetComponent<WeaponScript>().WeaponStat != TypeOfStats.none)
                    EquippedWeaponStat = EquippedWeapon.GetComponent<WeaponScript>().WeaponStat;
                else
                {
                    if (gameObject.GetComponent<SpecificCharacterScript>().BaseStrength > gameObject.GetComponent<SpecificCharacterScript>().BaseDexterity)
                    {
                        EquippedWeaponStat = TypeOfStats.Strength;
                    }
                    else EquippedWeaponStat = TypeOfStats.Dexterity;
                }



                EquippedWeaponType = EquippedWeapon.GetComponent<WeaponScript>().WeaponCategory;
                Unarmed = false;

                if (!WeaponLevels.ContainsKey(EquippedWeaponType))
                {
                    WeaponLevels.Add(EquippedWeaponType, 1);
                }

                AverageDamage = CalculateWeaponDamage(EquippedWeapon.GetComponent<WeaponScript>());



                Range = EquippedWeapon.GetComponent<WeaponScript>().BaseRange;

                AttackCost = EquippedWeapon.GetComponent<WeaponScript>().BaseCost;
                EquippedWeapon.GetComponent<WeaponScript>().EquippedTo = gameObject;
                

                WeaponHeld = EquippedWeapon;
            }
            else Debug.Log("You forgot to put WeaponScript on the weapon");
        }
        else
        {
            EquippedWeaponType = WeaponType.Fisticuffs;
            if (!WeaponLevels.ContainsKey(EquippedWeaponType))
            {
                WeaponLevels.Add(EquippedWeaponType, 1);
            }
            Unarmed = true;

            if (gameObject.GetComponent<SpecificCharacterScript>().BaseStrength > gameObject.GetComponent<SpecificCharacterScript>().BaseDexterity)
            {
                EquippedWeaponStat = TypeOfStats.Strength;
            }
            else EquippedWeaponStat = TypeOfStats.Dexterity;

            AverageDamage = 3 + WeaponLevels[EquippedWeaponType];
            Range = .75f;
            AttackCost = 5f;

            CurrentWeaponLevel = WeaponLevels[EquippedWeaponType];


        }


        if (!TotalWeaponDamages.ContainsKey(EquippedWeaponType))

            TotalWeaponDamages.Add(EquippedWeaponType, 0);

        CurrentWeaponLevel = WeaponLevels[EquippedWeaponType];

        SphereColliderObject.GetComponent<SphereCollider>().radius = Range - .5f;


        SetDamageNeededToLevel();

        if (gameObject.GetComponent<DisarmedScript>())
        {
            gameObject.GetComponent<DisarmedScript>().originalDamage = 0;
            gameObject.GetComponent<DisarmedScript>().SetUp(0, 0);
        }

    }
    // should probably be a setter instead, but this works to set the damage we need to level up.
    public void SetDamageNeededToLevel()
    {
        float cost;
        if (EquippedWeapon != null)
            cost = EquippedWeapon.GetComponent<WeaponScript>().BaseCost;
        else cost = AttackCost;
        if (EquippedWeaponStat == TypeOfStats.Dexterity)
        {
            DamageNeededToLevelUp = WeaponLevels[EquippedWeaponType] * WeaponLevels[EquippedWeaponType] * WeaponDamage[EquippedWeaponType] * (int)(10f / cost) / (int)((float)gameObject.GetComponent<SpecificCharacterScript>().BaseDexterity) * 100 / 2;

        }
        else if (EquippedWeaponStat == TypeOfStats.Strength)
        {
            DamageNeededToLevelUp = WeaponLevels[EquippedWeaponType] * WeaponLevels[EquippedWeaponType] * WeaponDamage[EquippedWeaponType] * (int)(10f / cost) / (int)((float)gameObject.GetComponent<SpecificCharacterScript>().BaseStrength) * 100 / 2;
        }
        else if (EquippedWeaponType == WeaponType.Magic)
        {
            DamageNeededToLevelUp = WeaponLevels[WeaponType.Magic] + 10 + (5 - (((int)gameObject.GetComponent<SpecificCharacterScript>().BaseIntelligence / 2) - 3));
        }
    }


    // Is accessed by ManaPotionScript, and also used when sleeping
    public void ManaRegen(int ManaRegened)
    {
        //Used for increasing mana
        if (ManaRegened < ModifiedMaxMana - CurrentMana)
        {
            TotalManaRegened += ManaRegened;
        }
        else
        {
            TotalManaRegened += ModifiedMaxMana - CurrentMana;
        }

        //Regening Mana
        CurrentMana += ManaRegened;



        // modifiedMaxMana is used since enchantments will reduce max usable mana
        if (CurrentMana > ModifiedMaxMana)
        {

            // making sure for overflow, if we decide to put something in to super charge mana regen
            CurrentMana = ModifiedMaxMana;
        }
        // testing to see if we have increased our max mana
        if (TotalManaRegened >= (MaxMana * 100) / (int)mySheet.BaseIntelligence)
        {
            MaxMana++;
            TotalManaRegened = 0;
        }
        StartCoroutine(SetManaAndMaxManaBars());
    }
}
[Serializable]
class PlayerData
{
    public int
        MaxHealth,
        MaxMana,
        CurrentMana,
        TotalHealthRegened,
        TotalManaRegened,
        CurrentHealth;
    public float[]
        currentCheckpoint,
        currentPosition;
    public float
        currentRotation;
    public float
        TotalDistanceTraveled;
    // need this for status conditions
    public WeaponType[]
        leveledWeaponType,
        WeaponsWithTotalDamages;
    public int[]
        LevelOfWeapon,
        TotalDamage;
    public WeaponData
        weaponEquipped;
    public WeaponData[]
        weaponsCarried = new WeaponData[22];
    public ConsumableInfo[]
        consumablesCarried = new ConsumableInfo[22];
}
