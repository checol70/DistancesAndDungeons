using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornsStaffScript : MagicScript {

    public override void Effect(GameObject target)
    {
        // first assign strength

        strength = gameObject.transform.root.gameObject.GetComponent<CharacterScript>().GetMagicLevel();

        // then test for if target character is not buffed
        if (!target.GetComponent<ThornedScript>())
        {
            //add the buff
            target.AddComponent<ThornedScript>();

            // calculate the buff duration
            int dur = CalcRules(SpecialRulesEnum.Prolonged);

            // set up the buff.
            target.GetComponent<ThornedScript>().SetUp(strength, dur);
        }
        //if they have the buff then just add strength on top.
        else target.GetComponent<ThornedScript>().strength += strength;
    }
    protected override void SetUpSpecifics()
    {
        BaseCost = 7f;

        SpecialRules.Add(SpecialRulesEnum.Magic);

        

        BaseRange = 10;

        WeaponVariation = "Thorned Magic";
        DMagicType = "ThornsMagic";

        int level = gameObject.transform.root.gameObject.GetComponent<CharacterScript>().GetMagicLevel();

        string dur;
        if (CalcRules(SpecialRulesEnum.Prolonged) > 0)
            dur = (CalcRules(SpecialRulesEnum.Prolonged) + 1).ToString() + " rounds";
        else dur = "1 round";

        AdvancedTooltip = "Makes " + level.ToString() + " Damage taken be reflected to the attacker for " + dur;
    }
    public override void UpdateTip()
    {
        string dur;

        strength = gameObject.transform.root.gameObject.GetComponent<CharacterScript>().GetMagicLevel();

        if (CalcRules(SpecialRulesEnum.Prolonged) > 0)
            dur = (CalcRules(SpecialRulesEnum.Prolonged) + 1).ToString() + "rounds";
        else dur = "1 round";

        AdvancedTooltip = "Reflects " + strength.ToString() +" Damage to target to the attacker for " + dur;
    }
}
