using System;
[Serializable]
public class MonsterData {
    public float[]
        position,
        rotation;


    public MonsterTypes
        MonsterType;

    // to remember how many times the monster has been killed
    public int
        Level;

    // dictionary for buff and strength of the buff
    public Buffs[]
        CurrentBuffs;
    public int[]
        BuffStrengths;


}
