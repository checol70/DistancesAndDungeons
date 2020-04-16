
using System;


[Serializable]
public class WeaponData {

    public int
        reactionCount;

    public WeaponType
        weaponType;

    public string
        weaponVariant;

    public Substance
        madeOf;

    public EnchantmentType[]
        enchants;

    public SpecialRulesEnum[]
        SpecialRules,
        Modifiers;

    public int[]
        enchantmentStrengths,
        enchantmentCosts,
        enchantmentPenalties;
}
