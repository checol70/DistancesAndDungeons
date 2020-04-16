using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotionScript : ConsumableScript {

    private CharacterHealthScript Character;


    private void Start()
    {
        gameObject.tag = "HealthPotion";
        Character = gameObject.transform.root.gameObject.GetComponent<CharacterHealthScript>();
        Explanation = "Refills Health on consumption";
        Type = ConsumableEnum.HealthPotion;
        StartCoroutine(SetNumber());
    }
    public override void ConsumedEffect()
    {
        Character.Healed(Character.MaxHp);
    }
    private IEnumerator SetNumber()
    {
        yield return new WaitWhile(() => AmountShower == null);
        AmountShower.text = NumberHeld.ToString();
    }
    public override bool CanBeConsumed()
    {
        if (Character.CurrentHP < Character.MaxHp)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
