using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public abstract class HealthScript : MonoBehaviour {

    public GameObject 
        DamageCanvas,
        DamagePrefab;
    public int
        Defense,
        MaxHp,
        CurrentHP,
        DamageTaken,
        TotalDamageTaken;
    public Slider
        HealthIndicator,
        CharacterButtonHealthIndicator;
    public bool
        isPlayer;
    public GameObject
        MainCamera;
    protected Vector3
        Standard;
    private Color32
        Type;
    private GameObject
        DamageReveal;

    public List<GameObject> regenerationIndicators = new List<GameObject>(),
        defenseIndicators = new List<GameObject>(),
        attackBuffIndicators = new List<GameObject>(),
        stunnedIndicators = new List<GameObject>(),
        thornedIndicators = new List<GameObject>(),
        defenseShatteredIndicators = new List<GameObject>(),
        disarmedIndicators = new List<GameObject>();

    public void ShowBuff(Indicators i)
    {
        List<GameObject> l;
        switch (i)
        {
            case Indicators.regeneration:
                l = regenerationIndicators;
                break;
            case Indicators.defense:
                l = defenseIndicators;
                break;
            case Indicators.attackBuff:
                l = attackBuffIndicators;
                break;
            case Indicators.stunned:
                l = stunnedIndicators;
                break;
            case Indicators.thorned:
                l = thornedIndicators;
                break;
            case Indicators.defenseShattered:
                l = defenseShatteredIndicators;
                break;
            case Indicators.disarmed:
                l = disarmedIndicators;
                break;
            default:
                l = new List<GameObject>();
                Debug.Log("Missed the right object");
                break;
        }
        l.ForEach(e =>
        {
            e.SetActive(true);
        });
    }
    public void AddIndicator(Indicators i, GameObject g)
    {
        switch (i)
        {
            case Indicators.regeneration:
                regenerationIndicators.Add(g);
                break;
            case Indicators.defense:
                defenseIndicators.Add(g);
                break;
            case Indicators.attackBuff:
                attackBuffIndicators.Add(g);
                break;
            case Indicators.stunned:
                stunnedIndicators.Add(g);
                break;
            case Indicators.thorned:
                thornedIndicators.Add(g);
                break;
            case Indicators.defenseShattered:
                defenseShatteredIndicators.Add(g);
                break;
            case Indicators.disarmed:
                disarmedIndicators.Add(g);
                break;
            default:
                Debug.Log("Missed the right object");
                break;
        }
    }
    public void HideBuff(Indicators i)
    {
        List<GameObject> l;
        switch (i)
        {
            case Indicators.regeneration:
                l = regenerationIndicators;
                break;
            case Indicators.defense:
                l = defenseIndicators;
                break;
            case Indicators.attackBuff:
                l = attackBuffIndicators;
                break;
            case Indicators.stunned:
                l = stunnedIndicators;
                break;
            case Indicators.thorned:
                l = thornedIndicators;
                break;
            case Indicators.defenseShattered:
                l = defenseShatteredIndicators;
                break;
            case Indicators.disarmed:
                l = disarmedIndicators;
                break;
            default:
                l = new List<GameObject>();
                Debug.Log("Missed the right object");
                break;
        }
        l.ForEach(e =>
        {
            e.SetActive(false);
        });
    }
    void Start()
    {
        MainCamera = CameraScript.GameController.gameObject;

    }
    public abstract IEnumerator Begin(); 
    public void IsReady()
    {
        StartCoroutine(Begin());
    }
    //for initial hits
    public void InitHit(int DamageTaken, GameObject Attacker, DamageType damageType)
    {
        if (gameObject.GetComponent<DefenseScript>())
        {
            DamageTaken -= gameObject.GetComponent<DefenseScript>().ReduceDamage();
        }
        Hit(DamageTaken, Attacker, damageType);
    }
    public abstract void Healed(int amount);
    //for handling characters and monsters differently
    protected abstract void Hit(int DamageTaken, GameObject Attacker, DamageType damageType);
    //for actual damage
    public abstract void Damaged(int DamageTaken, DamageType damageType);
    //for showing damage.
    public void ShowDamageTaken(string ActualDamageTaken, DamageType damageType)
    {
        DamageReveal = Instantiate(Resources.Load("DamageText") as GameObject,DamageCanvas.transform);
        switch (damageType)
        {
            case DamageType.Physical:
                {
                    Type = Color.white;
                    break;
                }
            case DamageType.Fire:
                {
                    Type = Color.red;
                    break;
                }
            case DamageType.Acid:
                {
                    Type = new Color32(128, 255, 0, 255);
                    break;
                }
            case DamageType.Ice:
                {
                    Type = new Color32(153, 255, 255, 255);
                    break;
                }
            case DamageType.Bleed:
                {
                    Type = new Color32(122, 0, 0, 255);
                    break;
                }
            case DamageType.Electric:
                {
                    Type = new Color32(255, 255, 51, 255);
                    break;
                }
            case DamageType.Positive:
                {
                    Type = new Color32(0, 255, 122, 255);
                    break;
                }
            case DamageType.Negative:
                {
                    Type = new Color32(155, 0, 155, 255);
                    break;
                }
            default:
                {
                    Type = Color.black;
                    Debug.Log("damageType not recognized");
                    break;
                }

        }
        DamageReveal.GetComponent<DamageTextScript>().Ready(ActualDamageTaken, Type);
    }
}
