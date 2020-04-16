using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPotionScript : ConsumableScript {

	private CharacterScript Character;


    private void Start()
    {
        gameObject.tag = "ManaPotion";
        Character = gameObject.transform.root.gameObject.GetComponent<CharacterScript>();
        Explanation = "Refills Mana on consumption";
        Type = ConsumableEnum.ManaPotion;
        StartCoroutine(SetNumber());
    }
    private IEnumerator SetNumber()
    {
        yield return new WaitWhile(() => AmountShower == null);
        AmountShower.text = NumberHeld.ToString();
    }
    public override void ConsumedEffect ()
    {
        Character.ManaRegen(Character.MaxMana);
	}
    public override bool CanBeConsumed()
    {
        if(Character.CurrentMana < Character.ModifiedMaxMana)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
