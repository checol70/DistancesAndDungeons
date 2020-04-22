using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenerationStaffScript : MagicScript {

    public override void Effect(GameObject target)
    {
        CharacterScript player = gameObject.transform.root.gameObject.GetComponent<CharacterScript>();
        int level = player.GetMagicLevel();

        target.AddComponent<RegeneratingScript>();
        int strength = level +4;

        int duration = 1;
        target.GetComponent<RegeneratingScript>().SetUp(strength,duration);
    }
    protected override void SetUpSpecifics()
    {
        BaseCost = 7f;

        SpecialRules.Add(SpecialRulesEnum.Magic);



        BaseRange = 10;

        string dur;

        WeaponVariation = "Regeneration Staff";
        DMagicType = "RegenerationStaff";
        if (CalcRules(SpecialRulesEnum.Prolonged) > 0)
            dur = (CalcRules(SpecialRulesEnum.Prolonged) + 1).ToString() + " rounds";
        else dur = "1 round";

        AdvancedTooltip = "Heals target for " + (gameObject.transform.root.gameObject.GetComponent<CharacterScript>().GetMagicLevel() +4).ToString() +" health for "+ dur ;
    }
    public override void UpdateTip()
    {
        string dur;

        strength = gameObject.transform.root.gameObject.GetComponent<CharacterScript>().GetMagicLevel() +4;

        if (CalcRules(SpecialRulesEnum.Prolonged) > 0)
            dur = (CalcRules(SpecialRulesEnum.Prolonged) + 1).ToString() + "rounds";
        else dur = "1 round";

        AdvancedTooltip = "Heals target for " + strength.ToString() + " health per round for " + dur;
    }
}
