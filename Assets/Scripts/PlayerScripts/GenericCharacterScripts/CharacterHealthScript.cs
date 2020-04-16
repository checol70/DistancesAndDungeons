using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealthScript : HealthScript {

    public static GameObject
        CurrentCharacter;
    public int
        TotalHealed;
    public int
        TotalHealthRegened;
    private SpecificCharacterScript
        mySheet;

    private void Start()
    {
        mySheet = gameObject.GetComponent<SpecificCharacterScript>();
    }
    public override IEnumerator Begin()
    {
        yield return new WaitWhile(() => HealthIndicator == null);
        HealthIndicator.maxValue = MaxHp;
        CurrentHP = MaxHp;
        if (gameObject != CameraScript.GameController.ActivePlayer)
            HealthIndicator.gameObject.SetActive(false);
        CurrentCharacter = CameraScript.GameController.ActivePlayer;
    }
    public void ButtonCreated()
    {

        CharacterButtonHealthIndicator.maxValue = MaxHp;
        CharacterButtonHealthIndicator.value = MaxHp;

    }
    protected override void Hit(int DamageTaken, GameObject Attacker, DamageType damageType)
    {
        // for damage resist and reflect
        gameObject.GetComponent<CharacterScript>().BlockAndCounter(Attacker, DamageTaken, damageType);
    }

    // for the actual damage we take
    public override void Damaged(int DamageTaken, DamageType Type)
    {
        Debug.Log(DamageTaken);
        if (DamageTaken > CurrentHP)
            DamageTaken = CurrentHP;
        CurrentHP = CurrentHP - DamageTaken;
        HealthIndicator.value = CurrentHP;
        
            CharacterButtonHealthIndicator.value = CurrentHP;
            CharacterButtonHealthIndicator.maxValue = MaxHp;
        ShowDamageTaken(DamageTaken.ToString(), Type);
        if (CurrentHP <= 0.0f)
        {
            CharacterButtonHealthIndicator.transform.GetChild(1).gameObject.SetActive(false);
            CharacterButtonHealthIndicator.transform.parent.GetChild(1).GetChild(1).gameObject.SetActive(false);
            Destroy(gameObject);
        }
        Debug.Log(DamageTaken);
    }

    public void Load()
    {
        TotalHealed = TotalHealthRegened;
    }

    public void BigSliderChange()
    {
        HealthIndicator.gameObject.SetActive(true);
        if(CurrentCharacter != null)
        {
            CurrentCharacter.GetComponent<CharacterHealthScript>().HealthIndicator.gameObject.SetActive(false);
        }
        CurrentCharacter = gameObject;

    }

    // this is so that we can level up our max health
    public override void Healed(int HealedAmount)
    {
        Debug.Log(HealedAmount.ToString() + " CharacterHealthScript");
        Debug.Log("Healing Started");
        //actual healing
        if (CurrentHP == MaxHp)
            return;
        if(CurrentHP + HealedAmount > MaxHp)
        {
            Debug.Log("No More Healing");
            HealedAmount = MaxHp - CurrentHP;
            CurrentHP = MaxHp;
            TotalHealed += HealedAmount;
        }
        else
        {
            CurrentHP += HealedAmount;
        }
        TotalHealed += HealedAmount;
        CharacterButtonHealthIndicator.value = CurrentHP;
        HealthIndicator.value = CurrentHP;
        Debug.Log("finished HealthRegen");
        ShowDamageTaken(HealedAmount.ToString(), DamageType.Positive);

        if(TotalHealed >= MaxHp-(int)gameObject.GetComponent<SpecificCharacterScript>().BaseConstitution)
        {
            TotalHealed -= MaxHp;
            ShowDamageTaken("Max HP +1", DamageType.Physical);
            MaxHp++;
        }
    }
}
