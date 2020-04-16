using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.AI;
public abstract class MonsterScript : BuffReferenceScript
{
    //base class for all monsters
    public Text
        TurnCounter;
    public int
        RevealedAttackRoll,
        AverageDamage,
        Defense,
        MonsterLevel,
        BaseMaxHealth;
    public GameObject
        turnCounterCanvas,
        Model,
        PlayerToBeChecked,
        WeaponHeld;
    public float // RangeOffset is for large or small creatures, most don't need it
        RangeOffset,
        MaxMovement,
        RemainingMovement,
        Range,
        AttackCost,
        AttacksMade,
        DistanceFromCheckpoint,
        TotalDistanceTraveled;
    public bool
        Unarmed,
        NoRemainingMovement;
    public static bool
        IsMonsterTurn;
    public bool
        HasSeenPlayerThisTurn,
        HasSeenPlayerLastTurn;
    private bool
        HasStartedAttacking;
    public bool
        HasCheckedPlayerDistance,
        IsActiveMonster;
    public RaycastHit
        Seen;
    public GameObject
        HealthCanvas;
    public GameObject
        LastSeenPlayer,
        ClosestPlayer,
        TargetLastHit,
        ActiveMonster;
    protected int dR;
    public int
        DefaultLayerMask,
        PlayersOnlyLayerMask,
        CombinedMask,
        AttackRoll,
        DamageDealt;
    public Component[]
        Renderers;
    public float
        Spacer,
        SavedMovement,
        ClosestDistance,
        LastCheckedDistance;
    public Vector3
        CurrentCheckpoint,
        PositionChange;
    private float
        AttackSpacer;
    private bool
        AgentOn,
        DelaySet;
    private float
        TimeDelay,
        TimePoint;
    public MonsterTypes
        monsterType;
    private int
        turnsTillUp;
    public Transform
        WeaponHook;
    public Text 
        timer;
    public List<FogOfWarRoomScript> roomsInside = new List<FogOfWarRoomScript>();

    public void EnteredRoom(FogOfWarRoomScript fogOfWarRoom)
    {
        roomsInside.Add(fogOfWarRoom);
    }

    public void LeftRoom(FogOfWarRoomScript fogOfWarRoom)
    {
        roomsInside.Remove(fogOfWarRoom);
    }

    void OnEnable()
    {
        CameraScript.StartinPlayerTurn += EndingMonsterTurn;
        CameraScript.EndingPlayerTurn += StartingMonsterTurn;
    }
    void OnDisable()
    {
        CameraScript.StartinPlayerTurn -= EndingMonsterTurn;
        CameraScript.EndingPlayerTurn -= StartingMonsterTurn;
    }
    void Start()
    {
        CurrentCheckpoint = transform.position;
        DefaultLayerMask = 1 << 9;
        PlayersOnlyLayerMask = 1 << 11;
        CombinedMask = PlayersOnlyLayerMask | DefaultLayerMask;
        TimeDelay = 2.5f;
        Setup();
        StartCoroutine(WaitForReady());
        GenerateWeapon();
        StartCoroutine(WaitForDungeon());
        Renderers = GetComponentsInChildren<Renderer>();
    }
    IEnumerator WaitForDungeon()
    {
        yield return new WaitWhile(() => DungeonScript.CurrentDungeon == null);
        MonsterLevel = DungeonScript.CurrentDungeon.CurrentLevelIndex;

    }
    private WeaponType RandomWeaponType()
    {
        int weaponClass = Random.Range(0, 7);
        switch (weaponClass)
        {
            case 0:
                {
                    return WeaponType.SwordAndShield;
                }
            case 1:
                {
                    return WeaponType.GreatAxe;
                }
            case 2:
                {
                    return WeaponType.Ranged;
                }
            default:
                {
                    return WeaponType.Magic;
                }
        }
    }
    private void GenerateWeapon()
    {
        WeaponType weapon = RandomWeaponType();


        ChooseWeapon(weapon);
    }

    IEnumerator WaitForReady()
    {
        yield return new WaitUntil(() => CameraScript.GameController.Ready);
        HasSeenPlayerLastTurn = false;
        HasSeenPlayerThisTurn = false;
    }

    //for setting up health mana movement and others for derived classes, not to be filled with base class uselessness
    // I did this so that I could have different start methods without having to copy all of them.
    protected abstract void ChooseWeapon(WeaponType wt);
    protected abstract void Setup();


    void Update()
    {
        if (LoadingScript.loadScreen.activeInHierarchy == false)
        {
            if (DelaySet)
            {
                if (Time.time >= TimePoint && ActiveMonster == gameObject)
                {
                    NoRemainingMovement = true;
                }
            }
            DistanceFromCheckpoint = Vector3.Distance(gameObject.transform.position, CurrentCheckpoint);
            RemainingMovement = (MaxMovement + SavedMovement) - (DistanceFromCheckpoint + TotalDistanceTraveled + (AttackCost * AttacksMade));
            // for checking if we see the players
            if (CameraScript.GameController != null)
                if (CameraScript.GameController.ActiveMonster != null)
                {
                    ActiveMonster = CameraScript.GameController.ActiveMonster;
                }
            IsActiveMonster = gameObject == ActiveMonster;
            if (true)
            {
                if (HasSeenPlayerThisTurn || HasSeenPlayerLastTurn)
                {
                    foreach (Renderer R in Renderers)
                    {
                        R.enabled = true;
                    }

                    HealthCanvas.SetActive(true);
                }
                if (!HasSeenPlayerThisTurn && !HasSeenPlayerLastTurn)
                {
                    foreach (Renderer R in Renderers)
                    {
                        R.enabled = false;
                    }

                    HealthCanvas.SetActive(false);
                }
                if (!IsMonsterTurn)
                {
                    if (HasSeenPlayerThisTurn == true)
                    {
                        PlayerToBeChecked = CameraScript.GameController.ActivePlayer;
                        if (PlayerToBeChecked != null)
                        {
                            Physics.Raycast(gameObject.transform.position, (PlayerToBeChecked.transform.position - gameObject.transform.position), out Seen, 16.0f, CombinedMask);
                            if (Seen.collider != null)
                            {
                                if (Seen.collider.gameObject.CompareTag("Player") == true)
                                {
                                    HasSeenPlayerThisTurn = StartManagerScript.StartGame;
                                    LastSeenPlayer = Seen.collider.gameObject;
                                    ClosestPlayer = LastSeenPlayer;
                                    ClosestDistance = Vector3.Distance(gameObject.transform.position, ClosestPlayer.transform.position);
                                }
                            }
                        }
                    }
                }
                if (HasSeenPlayerThisTurn || HasSeenPlayerLastTurn)
                {
                    if (IsMonsterTurn)
                    {
                        ActiveMonster = CameraScript.GameController.ActiveMonster;

                        IsActiveMonster = gameObject == ActiveMonster;
                        if (gameObject == ActiveMonster)
                        {
                            if (!DelaySet)
                            {
                                TimePoint = Time.time + TimeDelay;
                                DelaySet = true;
                            }
                            if (AgentOn == false && NoRemainingMovement == false)
                            {
                                StartCoroutine(TurnOnOffNavigation(false));
                                AgentOn = true;
                            }
                            if (DistanceFromCheckpoint >= 1)
                            {
                                TotalDistanceTraveled += DistanceFromCheckpoint;
                                CurrentCheckpoint = gameObject.transform.position;
                            }
                            DistanceFromCheckpoint = Vector3.Distance(CurrentCheckpoint, transform.position);
                            if (RemainingMovement > 0f && !NoRemainingMovement)
                            {
                                if (ClosestPlayer != null)
                                {
                                    if (Vector3.Distance(gameObject.transform.position, ClosestPlayer.transform.position) > Range + RangeOffset && gameObject.GetComponent<NavMeshAgent>())
                                    {
                                        ClosestPlayer.GetComponent<UnityEngine.AI.NavMeshObstacle>().carving = false;
                                        if (gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled)
                                        {
                                            gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().destination = ClosestPlayer.transform.position;
                                            if (gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().pathStatus != UnityEngine.AI.NavMeshPathStatus.PathComplete)
                                            {
                                                Debug.Log("I cannot Get there");
                                                NoRemainingMovement = true;
                                            }
                                            gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().stoppingDistance = Range +(RangeOffset);
                                        }
                                    }
                                    else if (!HasStartedAttacking && Vector3.Distance(gameObject.transform.position, ClosestPlayer.transform.position) <= Range + RangeOffset)
                                    {
                                        if (gameObject.name == "Iron Golem(Clone)")
                                        {
                                            Debug.Log("Attacking " + ClosestPlayer.name); 
                                        }
                                        RaycastHit Strucken;

                                        Physics.Raycast(ClosestPlayer.transform.position, gameObject.transform.position - ClosestPlayer.transform.position, out Strucken, Vector3.Distance(ClosestPlayer.transform.position, gameObject.transform.position), CombinedMask);


                                        if (Strucken.collider == null)
                                        {
                                            HasStartedAttacking = true;
                                            StartCoroutine(AttackTimer());
                                            
                                            ClosestPlayer.GetComponent<UnityEngine.AI.NavMeshObstacle>().carving = true;
                                        }
                                        else
                                        {
                                            if(gameObject.GetComponent<NavMeshAgent>())
                                                gameObject.GetComponent<NavMeshAgent>().SetDestination(ClosestPlayer.transform.position);
                                        }
                                    }
                                }
                                if (ClosestPlayer == null)
                                {
                                    HasCheckedPlayerDistance = false;
                                    ChoosingTarget();
                                }
                            }
                        }
                        if (RemainingMovement <= 0f || NoRemainingMovement)
                        {
                            if (AgentOn == true)
                            {
                                StartCoroutine(TurnOnOffNavigation(true));
                                AgentOn = false;
                                Debug.Log("Out of Movement");
                            }
                            NoRemainingMovement = true;
                        }
                    }
                }
            }
        }
    }

    public void PlayerEntered(GameObject player)
    {
        HasSeenPlayerThisTurn = true;

        ClosestPlayer = player;

        ClosestDistance = Vector3.Distance(gameObject.transform.position, ClosestPlayer.transform.position);
    }

    //void FindingTarget()
    //{
    //    for (int Number = 0; Number < CameraScript.GameController.Players.Length; Number++)
    //    {
    //        if (CameraScript.GameController.Players[Number] != null)
    //        {

    //            Physics.Raycast(gameObject.transform.position, CameraScript.GameController.Players[Number].transform.position - gameObject.transform.position, out Seen, 20.0f, CombinedMask);
    //            if (Seen.collider != null)
    //            {
    //                if (Seen.collider.gameObject.CompareTag("Player") == true)
    //                {
    //                    if (HasCheckedPlayerDistance == false)
    //                    {
    //                        ClosestPlayer = Seen.collider.gameObject;
    //                        ClosestDistance = Vector3.Distance(gameObject.transform.position, ClosestPlayer.transform.position);
    //                        HasCheckedPlayerDistance = true;
    //                    }
    //                    if (HasCheckedPlayerDistance == true)
    //                    {
    //                        LastSeenPlayer = Seen.collider.gameObject;
    //                        LastCheckedDistance = Vector3.Distance(gameObject.transform.position, LastSeenPlayer.transform.position);
    //                        if (LastCheckedDistance < ClosestDistance)
    //                        {
    //                            ClosestPlayer = LastSeenPlayer;
    //                            ClosestDistance = LastCheckedDistance;
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}

    void ChoosingTarget()
    {
        for (int Number = 0; Number < CameraScript.GameController.Players.Length; Number++)
        {
            if (CameraScript.GameController.Players[Number] != null)
            {

                Physics.Raycast(gameObject.transform.position, CameraScript.GameController.Players[Number].transform.position - gameObject.transform.position, out Seen, 20.0f, CombinedMask);
                if (Seen.collider != null)
                {
                    if (Seen.collider.gameObject.CompareTag("Player") == true)
                    {
                        if (HasCheckedPlayerDistance == false)
                        {
                            ClosestPlayer = Seen.collider.gameObject;
                            ClosestDistance = Vector3.Distance(gameObject.transform.position, ClosestPlayer.transform.position);
                            HasCheckedPlayerDistance = true;
                        }
                        if (HasCheckedPlayerDistance == true)
                        {
                            LastSeenPlayer = Seen.collider.gameObject;
                            LastCheckedDistance = Vector3.Distance(gameObject.transform.position, LastSeenPlayer.transform.position);
                            if (LastCheckedDistance < ClosestDistance)
                            {
                                ClosestPlayer = LastSeenPlayer;
                                ClosestDistance = LastCheckedDistance;
                            }
                        }
                    }
                }
            }
        }
    }

    IEnumerator AttackTimer()
    {
        Spacer = CameraScript.GameController.Spacer;
        Debug.Log("Attack Starting: " + gameObject.name);
        if (gameObject.GetComponent<NavMeshAgent>() != null)
            gameObject.GetComponent<NavMeshAgent>().SetDestination(gameObject.transform.position);
        while (ClosestPlayer != null && RemainingMovement >= AttackCost && Vector3.Distance(ClosestPlayer.transform.position, gameObject.transform.position) <=  Range + RangeOffset)
        {
            AttackSpacer = Time.time + Spacer;
            yield return new WaitUntil(() => Time.time >= AttackSpacer);
            if (ClosestPlayer != null)
            {
                PerformAttack(ClosestPlayer);
                AttacksMade++;
            }
        }
        if (
                (
                    ClosestPlayer == null || Vector3.Distance(ClosestPlayer.transform.position, gameObject.transform.position) > Range +RangeOffset
                )
                && RemainingMovement >= AttackCost
            )
        {
            if (CameraScript.GameController.Players != null)
            {
                HasStartedAttacking = false;
                HasCheckedPlayerDistance = false;
            }
        }
        if (ClosestPlayer != null)
            if (RemainingMovement < AttackCost)
            {
                NoRemainingMovement = true;
                if (AgentOn)
                {
                    StartCoroutine(TurnOnOffNavigation(true));
                    AgentOn = false;
                }
            }
    }

    IEnumerator TurnOnOffNavigation(bool agentOn)
    {
        if (!agentOn)
        {
            gameObject.GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = false;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;

            yield return null;
            gameObject.AddComponent<UnityEngine.AI.NavMeshAgent>();
            gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().baseOffset = 1;
        }
        if (agentOn)
        {
            Destroy(gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>());

            gameObject.GetComponent<Rigidbody>().isKinematic = false;

            yield return null;
            gameObject.GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = true;
        }
    }



    void StartingMonsterTurn()
    {
        IsMonsterTurn = true;
        if (HasSeenPlayerThisTurn || HasSeenPlayerLastTurn)
            NoRemainingMovement = false;
        else
        {
            NoRemainingMovement = true;
        }
        CurrentCheckpoint = gameObject.transform.position;
        AttacksMade = 0;
        HasCheckedPlayerDistance = false;
        TotalDistanceTraveled = 0;
        HasStartedAttacking = false;
    }


    void EndingMonsterTurn()
    {
        IsMonsterTurn = false;
        CurrentCheckpoint = gameObject.transform.position;
        DelaySet = false;
        HasSeenPlayerLastTurn = HasSeenPlayerThisTurn;
        HasSeenPlayerThisTurn = false;
        foreach(FogOfWarRoomScript f in roomsInside)
        {
            if (f.HasPlayer())
            {
                HasSeenPlayerThisTurn = true;
            }
        }
    }


    void PerformAttack(GameObject target)
    {
        {
            target.GetComponent<HealthScript>().InitHit(AverageDamage, gameObject, DamageType.Physical);
        }
    }
    public abstract void DropLoot();
}
