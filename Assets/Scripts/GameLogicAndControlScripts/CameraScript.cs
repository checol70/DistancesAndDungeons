using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using System;
using System.IO;
[Serializable]
public class CameraBoundary
{
    public float xMax, xMin, yMax, yMin, zMax, zMin;
}

public class CameraScript : MonoBehaviour
{
    void Awake()
    {
        if (GameController == null)
        {
            GameController = this;
        }
        else if (GameController != this)
        {
            Destroy(gameObject);
        }
    }
    
    NavMeshBuildSettings buildSettings = new NavMeshBuildSettings() {
        agentClimb = .6f,
        agentHeight = 2,
        agentRadius = .5f,
        agentSlope = 45,
        agentTypeID = 0,
        minRegionArea = 4,
        tileSize = 64,
    };


    public static IDictionary<GameObject, bool> animationsFinished = new Dictionary<GameObject,bool>();
    
    public static bool 
        InventoryOpen;
    public static CameraScript 
        GameController;
    public delegate void PlayersTurn();
    public static event PlayersTurn StartinPlayerTurn;
    public delegate void MonstersTurn();
    public static event MonstersTurn EndingPlayerTurn;

    public delegate void UpdateNavMesh();
    public static event UpdateNavMesh UpdatingNavMesh;
    public Button
        EndTurnButton,
        InventoryButton,
        StatButton;
    public bool[]
        NoRemainingMovementPlayers,
        NoRemainingMovementMonsters;
    public Slider
        PlayerHealthBar,
        PlayerManaBar;
    public GameObject[]
        Players,
        Monsters;
    public GameObject
        FunctionButtonBar,
        CharacterButtonSorter,
        Stricken,
        ActivePlayer,
        ActiveMonster;
    public float
        Spacer,
        successValue,
        distanceToPlayer,
        ActiveRange,
        AttackRoll;
    public Text
        outRange;
    
    public CameraBoundary
        boundary;
    public bool
        PlayerTurn;
    public bool
        MonsterTurn;
    private int
        //PlayerNumber,
        MonsterNumber,
        WaitForCameraBoundary;
    public int
        MonsterNumberChecker,
        MonsterLengthChecker,
        AverageDamage;
    public IDictionary<GameObject, float> Xpos = new Dictionary<GameObject, float>();
    public IDictionary<GameObject, float> Zpos = new Dictionary<GameObject, float>();
    private GameObject
        CurrentCharacterSelectButton;
    public bool 
        Ready;
    private Vector3
        baseOffSet;
    public Vector3 
        offSet;

    

    public void StartingThePlayerTurn()
    {
        if (StartinPlayerTurn != null)
            StartinPlayerTurn();
        PlayerTurn = true;
        MonsterTurn = false;
        EndTurnButton.interactable = true;
        InventoryButton.interactable = true;
        MonsterNumber = 0;
        WeaponScript.MonsterTurn = false;
    }
    public void UpdatinNavMesh()
    {
        if (UpdatingNavMesh != null)
            UpdatingNavMesh();

        EndTurnButton.interactable = false;
        InventoryButton.interactable = false;
        PlayerTurn = false;
        MonsterTurn = false;
        StartCoroutine(WaitForAnimations());
    }
    IEnumerator WaitForAnimations()
    {
        yield return null;
        yield return new WaitForSeconds(2f);
        yield return new WaitWhile(() => animationsFinished.Values.Contains(false));
        EndingThePlayerTurn();

    }
    public void EndingThePlayerTurn()
    {
        if (EndingPlayerTurn != null)
            EndingPlayerTurn();
        //PlayerNumber = 0;
        
        Monsters = null;
        Monsters = GameObject.FindGameObjectsWithTag("Monster");
        WeaponScript.MonsterTurn = true;
        MonsterTurn = true;
        PlayerTurn = false;
    }
    
    void OnEnable()
    {
        Players = GameObject.FindGameObjectsWithTag("Player");
        //SceneManager.sceneLoaded += ResetBoundary;
    }
    // Use this for initialization
    void Start()
    {
        //WaitForBoundary = 0;
        PlayerTurn = true;
        Monsters = GameObject.FindGameObjectsWithTag("Monster");
       // boundary.zMin = -11f;
        StartCoroutine(CharacterButtonSetup());
        baseOffSet = new Vector3(1, 40, -9f);
        ResetOffset();
    }
    IEnumerator CharacterButtonSetup()
    {
        yield return new WaitWhile(() => CharacterButtonSorter == null);
        foreach (GameObject player in Players)
        {
            CurrentCharacterSelectButton = Instantiate(Resources.Load("CharacterButtonAndAnimatorController/CharacterButton", typeof(GameObject)) as GameObject);
            CurrentCharacterSelectButton.transform.SetParent(CharacterButtonSorter.transform, false);
            GameObject PlayerImage;
            PlayerImage = Instantiate(Resources.Load("CharacterImages/" + player.name + "Image", typeof(GameObject)) as GameObject);
            PlayerImage.transform.SetParent(CurrentCharacterSelectButton.transform, false);
            CurrentCharacterSelectButton.name = player.name + "Button";
            CurrentCharacterSelectButton.GetComponent<CharacterSelectButtonScript>().CharacterAssigned = player;
            CurrentCharacterSelectButton.GetComponent<CharacterSelectButtonScript>().Begin();
            if (player == ActivePlayer)
            {
                CurrentCharacterSelectButton.GetComponent<CharacterSelectButtonScript>().Selected();
            }

        }
    }

    public void OpenTheStatPage()
    {

    }

    public void ResetOffset()
    {
        offSet = baseOffSet;
    }

    public void CloseTheStatPage()
    {

    }
    
    void Update()
    {
        {
            if (Input.mousePresent)
            {
                if (Input.GetButton("Jump"))
                {
                    gameObject.GetComponent<Camera>().orthographicSize ++;
                }
                if(Input.GetAxis("ScrollWheel") < 0)
                {
                    gameObject.GetComponent<Camera>().orthographicSize++;
                }
                if(Input.GetAxis("ScrollWheel") > 0 && gameObject.GetComponent<Camera>().orthographicSize >2)
                {
                    gameObject.GetComponent<Camera>().orthographicSize--;
                }
                if (Input.GetButton("Crouch") && gameObject.GetComponent<Camera>().orthographicSize > 2)
                {
                    gameObject.GetComponent<Camera>().orthographicSize--;
                }
            }
            if(ActivePlayer != null)
            {
                gameObject.transform.position = ActivePlayer.transform.position + offSet;
            }
        }
        /*if (LavaPlaneScript.lavaPlane != null)
        {
            transform.position = new Vector3(
              //clamping the x axis
              Mathf.Clamp(transform.position.x, pos.x - (scale.x + gameObject.GetComponent<Camera>().orthographicSize / 2), pos.x + (scale.x - gameObject.GetComponent<Camera>().orthographicSize / 2)),
              //clamping the y axis
              Mathf.Clamp(transform.position.y, 40, 40),
              //clamping the z axis
              Mathf.Clamp(transform.position.z, pos.z - (scale.z + gameObject.GetComponent<Camera>().orthographicSize / 2), pos.z + (scale.z - gameObject.GetComponent<Camera>().orthographicSize / 2))
              );
        }*/
        if (ActivePlayer != null)
        {

            if (Xpos.ContainsKey(ActivePlayer))
                if (Xpos[ActivePlayer] != ActivePlayer.transform.position.x)
                {
                    Xpos[ActivePlayer] = ActivePlayer.transform.position.x;
                }
            if (Zpos.ContainsKey(ActivePlayer))
                if (Zpos[ActivePlayer] != ActivePlayer.transform.position.z)
                {
                    Zpos[ActivePlayer] = ActivePlayer.transform.position.z;
                }
        }
        

         if (LavaPlaneScript.lavaPlane != null)
         {

            Vector3 pos = LavaPlaneScript.lavaPlane.transform.position;
            Vector3 scale = LavaPlaneScript.lavaPlane.transform.localScale;
            //setting the current camera bounds

            float invLerpedHeight = Mathf.InverseLerp(30, 10, gameObject.transform.position.y);
            if (scale.z > scale.x)
            {
                if (gameObject.GetComponent<Camera>().orthographicSize > scale.x)
                {
                    gameObject.GetComponent<Camera>().orthographicSize = scale.x;
                }
            }
            else
            {
                if (gameObject.GetComponent<Camera>().orthographicSize > scale.z)
                {
                    gameObject.GetComponent<Camera>().orthographicSize = scale.z;
                }
            }
        }
        if (Players.Length != 0)
        {
/*
            if (PlayerTurn == true && InventoryOpen == false)
            {
                if (Players[PlayerNumber] != null && PlayerNumber < Players.Length)
                {
                    if (Players[PlayerNumber].GetComponent<CharacterScript>() != null)
                    {
                        if (Players[PlayerNumber].GetComponent<CharacterScript>().HasNoMovement == true)
                        {
                            PlayerNumber++;
                        }
                    }
                }
                if (PlayerNumber < Players.Length && Players[PlayerNumber] == null)
                    PlayerNumber++;
                if (PlayerNumber >= Players.Length)
                    UpdatinNavMesh();

            }
            */

                if (MonsterTurn == true)
                {
                    MonsterLengthChecker = Monsters.Length;
                    MonsterNumberChecker = MonsterNumber;
                    //Make sure that the Monster length isn't zero before we test our monsters for their status
                    if (MonsterNumber < Monsters.Length && Monsters.Length != 0 && Monsters[MonsterNumber] != null)
                    {
                        if (ActiveMonster != Monsters[MonsterNumber] || ActiveMonster == null)
                        {
                            ActiveMonster = Monsters[MonsterNumber];
                        }
                        if (Monsters[MonsterNumber].GetComponent<MonsterScript>().NoRemainingMovement == true || Monsters[MonsterNumber] == null)
                            MonsterNumber++;
                    }
                    if (MonsterNumber < Monsters.Length && Monsters[MonsterNumber] == null)
                    {
                        MonsterNumber++;
                    }
                    if (MonsterNumber == Monsters.Length)
                    {
                        StartingThePlayerTurn();
                    }
                }
                //Making sure the camera stays somewhere where it can see something.
                if (LavaPlaneScript.lavaPlane != null)
                    transform.position = new Vector3(
                    //clamping the x axis
                    Mathf.Clamp(transform.position.x, boundary.xMin, boundary.xMax),
                    //clamping the y axis
                    Mathf.Clamp(transform.position.y, boundary.yMin, boundary.yMax),
                    //clamping the z axis
                    Mathf.Clamp(transform.position.z, boundary.zMin, boundary.zMax)
                    );

            }
        }
    
    public void InventoryOpened()
    {
        
        ActivePlayer.GetComponent<CharacterScript>().InventoryManager.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        InventoryOpen = true;
        // set up the Health and Mana bars and numbers in CharacterScript
        var characterScript = ActivePlayer.GetComponent<CharacterScript>();
        characterScript.SetInventoryBarsAndText();
        MainCanvasScript.MainCanvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        MainCanvasScript.MainCanvas.GetComponent<Canvas>().worldCamera = InventoryActiveCameraScript.InventoryCamera;
        MainCanvasScript.MainCanvas.GetComponent<CanvasGroup>().interactable = false;

        ActivePlayer.GetComponent<CharacterScript>().InventoryManager.GetComponent<CanvasGroup>().interactable = true;
        ActivePlayer.GetComponent<CharacterScript>().InventoryManager.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void InventoryClosed()
    {
        ActivePlayer.GetComponent<CharacterScript>().InventoryManager.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        InventoryOpen = false;

        MainCanvasScript.MainCanvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        MainCanvasScript.MainCanvas.GetComponent<CanvasGroup>().interactable = true;

        ActivePlayer.GetComponent<CharacterScript>().InventoryManager.GetComponent<CanvasGroup>().interactable = false;
        ActivePlayer.GetComponent<CharacterScript>().InventoryManager.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void PlayerDestroyed()
    {
        Players = GameObject.FindGameObjectsWithTag("Player");
        
    }

}