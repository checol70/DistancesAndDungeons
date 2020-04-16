using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class WeaponScript : MonoBehaviour
{
    private float
        EnchantSpacer;

    public int
        ParryRoll,
        reactionCount;
    public int
        MaxBraceCount,
        MaxFleeingCount,
        BlockAmount,
        CounterAmount,
        CritChance;


    public WeaponType
    WeaponCategory;
    public string
        WeaponVariation,
        AdvancedTooltip;
    public List<SpecialRulesEnum> SpecialRules = new List<SpecialRulesEnum>();
    public List<SpecialRulesEnum> Modifiers = new List<SpecialRulesEnum>();
        
    public TypeOfStats
        WeaponStat;
    public int
        BaseDamage;
    public float
        BaseRange,
        BaseCost;
    public bool
        IsMelee,
        CanBeThrown;
    public GameObject
        EquippedTo,
        WeaponMesh;
    static public bool
        MonsterTurn;
    public ItemType 
        HandsRequired;
    public int ManaPenalty,
        ManaCost;
    public List<WeaponEnchantmentScript> enchantments = new List<WeaponEnchantmentScript>();
    
    //bE or Braced Enemies
    public List<GameObject> bE = new List<GameObject>();

    public void CalculateManaPenalty()
    {
        ManaPenalty = 0;
        for (int i = 0; i < enchantments.Count; i++)
        {
            if (enchantments[i] != null)
            {
                ManaPenalty += enchantments[i].ManaPenalty;
            }
            else
            {
                enchantments.Remove(enchantments[i]);
            }
        }
    }

    // for General things like adding the block rule to sword and shield or that the weapon is a sword or shield
    protected abstract void SetUpBaseClass();


    private void Start()
    {
        // this is so that we always have a stat to go off of, just in case I forget to put them on
        WeaponStat = TypeOfStats.Strength;
        // these are so that when we have variations we can just create derived classes that override only what they need to.

        SetUpBaseClass();
        SetUpSpecifics();

    }


    // for setting up the initial stats of the weapon
    protected abstract void SetUpSpecifics();
 

    public void Equipped()
    {
        EquippedTo = gameObject.transform.root.gameObject;

        WeaponMesh = Resources.Load("WeaponMeshes/" + WeaponVariation) as GameObject;
        EquippedTo.GetComponent<CharacterScript>().ShowWeapons(WeaponMesh);
    }
    
    bool HasRule(SpecialRulesEnum rule)
    {
        if (SpecialRules.Contains(rule) || Modifiers.Contains(rule))
        {
            return true;
        }
        else return false;
    }

    public void OnCrit(GameObject target)
    {
        if (HasRule(SpecialRulesEnum.DisarmingCrits))
        {
            if(!target.GetComponent<DisarmedScript>())
                target.AddComponent<DisarmedScript>();
            target.GetComponent<DisarmedScript>().SetUp(CalcRules(SpecialRulesEnum.DisarmingCrits), 1);
        }
        if (HasRule(SpecialRulesEnum.ShatteringCrits))
        {
            if(!target.GetComponent<ShatteredScript>())
                target.AddComponent<ShatteredScript>();
            target.GetComponent<ShatteredScript>().SetUp(CalcRules(SpecialRulesEnum.ShatteringCrits), 1);
        }
        if (HasRule(SpecialRulesEnum.StunningCrits))
        {
            if (!target.GetComponent<StunnedScript>())
                target.AddComponent<StunnedScript>();
            target.GetComponent<StunnedScript>().SetUp(CalcRules(SpecialRulesEnum.StunningCrits), 1);
        }
    }

    // this is for calculating how many times we can hit a fleeing enemy or charging enemy
    public int CalcRules(SpecialRulesEnum checker)
    {
        int tempMaxReaction = 0;

        foreach(SpecialRulesEnum mum in SpecialRules)
        {
            if(mum == checker)
            {
                tempMaxReaction++;
            }
        }
        foreach(SpecialRulesEnum mum in Modifiers)
        {
            if(mum == checker)
            {
                tempMaxReaction++;
            }
        }


        return tempMaxReaction;
    }
    public void ReactionReset()
    {
        reactionCount = 0;
        bE.Clear();
    }

    public void ChargingEnemy(GameObject enemy)
    {
        MaxBraceCount = CalcRules(SpecialRulesEnum.Brace);

        if (!bE.Contains(enemy))
        {
            bE.Add(enemy);
            if (SpecialRules.Contains(SpecialRulesEnum.Brace) || Modifiers.Contains(SpecialRulesEnum.Brace))
                if (reactionCount < MaxBraceCount)
                {
                    gameObject.transform.root.gameObject.GetComponent<CharacterScript>().Attacking(enemy);
                    reactionCount++;
                }
        }
    }

    
    public void StartOnHit(int damage, GameObject target)
    {
        if (SpecialRules.Contains(SpecialRulesEnum.ExplosionAOE))
        {

        }
        if (SpecialRules.Contains(SpecialRulesEnum.LineAOE))
        {

        }
        if (SpecialRules.Contains(SpecialRulesEnum.PerpendicularLineAOE))
        {

        }
        StartCoroutine(OnHit(damage, target));
    }

    public IEnumerator OnHit(int damage, GameObject target)
    {
        if(enchantments != null)
        {
            foreach (WeaponEnchantmentScript e in enchantments)
            {
                Debug.Log("WeaponScript OnHit " + damage.ToString());

                // set up a pause between attacking and enchantments || set up a pause between enchantments
                EnchantSpacer = CameraScript.GameController.Spacer + Time.time;
                //use the pause
                yield return new WaitWhile(() => Time.time <= EnchantSpacer);

                // tell that it was successfully spaced


                // call the OnHit method in the current enchantment
                e.OnHit(damage, target);
            }
        }
    }

    public void CalculateManaCost()
    {
        ManaCost = 0;
        foreach (EnchantmentScript enchantment in enchantments)
        {
            ManaCost += enchantment.ManaCost;
        }
    }

    public void AddSpecialRules(SpecialRulesEnum[] s)
    {
        foreach (SpecialRulesEnum r in s)
        {
            SpecialRules.Add(r);
        }
    }
}
