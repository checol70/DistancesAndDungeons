using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBuffStaffScript : MagicScript
{

    public override void Effect(GameObject target)
    {
        // first assign strength

        strength = gameObject.transform.root.gameObject.GetComponent<CharacterScript>().GetMagicLevel();

        // then test for if target character is not buffed
        if (!target.GetComponent<DamageBuffScript>())
        {
            //add the buff
            target.AddComponent<DamageBuffScript>();
            
            // calculate the buff duration
            int dur = CalcRules(SpecialRulesEnum.Prolonged) +1;

            // set up the buff.
            target.GetComponent<DamageBuffScript>().SetUp(strength, dur);
        }
        //if they have the buff then just add strength on top.
        else target.GetComponent<DamageBuffScript>().strength += strength;
    }
    public override void UpdateTip()
    {
        // so that we can use the single/plural in the advanced tooltip
        string dur;

        // so that we can show the power of the Magic.
        strength = gameObject.transform.root.gameObject.GetComponent<CharacterScript>().GetMagicLevel();

        // figuring out if it is plural or single
        if (CalcRules(SpecialRulesEnum.Prolonged) > 0)
            dur = (CalcRules(SpecialRulesEnum.Prolonged) + 1).ToString() + "attacks";
        else dur = "attack";

        // putting it all together to create this.
        AdvancedTooltip = "Increases targets damage by " + strength.ToString() + " for next " + dur;
    }
    protected override void SetUpSpecifics()
    {
        BaseCost = 7f;

        SpecialRules.Add(SpecialRulesEnum.Magic);



        BaseRange = 10;

        WeaponVariation = "Damage Buff Magic";

        DMagicType = "DamageBuffMagic";
        
    }
}
