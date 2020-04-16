using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseStaffScript : MagicScript {

    protected override void SetUpSpecifics()
    {

        BaseCost = 7f;

        SpecialRules.Add(SpecialRulesEnum.Magic);

        // don't remember why this was here, but oh well, won't change it till I do.
        SpecialRules.Add(SpecialRulesEnum.Defending);

        BaseRange = 10;

        WeaponVariation = "Defense Magic";

        DMagicType = "DefenseMagic";

        strength = gameObject.transform.root.gameObject.GetComponent<CharacterScript>().GetMagicLevel() +9;

        // dur is to enable using plural or singular versions of words.
        string dur;
        if (CalcRules(SpecialRulesEnum.Prolonged) > 0)
            dur = (CalcRules(SpecialRulesEnum.Prolonged) + 1).ToString() + " rounds";
        else dur = "1 round";


        AdvancedTooltip = "Defends target for " + strength.ToString() + " health per round for " + dur;
    }
    public override void UpdateTip()
    {
        string dur;

        strength = gameObject.transform.root.gameObject.GetComponent<CharacterScript>().GetMagicLevel();

        if (CalcRules(SpecialRulesEnum.Prolonged) > 0)
            dur = (CalcRules(SpecialRulesEnum.Prolonged) + 1).ToString() + "rounds";
        else dur = "1 round";

        AdvancedTooltip = "Blocks next " + strength.ToString() +" Damage to target";
    }
    public override void Effect(GameObject target)
    {
        
            target.AddComponent<DefenseScript>();

            target.GetComponent<DefenseScript>().SetUp(gameObject.transform.root.gameObject.GetComponent<CharacterScript>().GetMagicLevel() +9, CalcRules(SpecialRulesEnum.Prolonged) + 1);
        
    }
}
