using UnityEngine;
using System.Collections;

public class MageDude : SpecificCharacterScript {

    public static GameObject mageDude;
    void Awake()
    {
        CharacterName = "MageDude";
        if (mageDude == null)
        {
            mageDude = gameObject;
            IsCharacter = true;
        }
        else if (gameObject != mageDude)
        {
            StartCoroutine(DestroySoon());
        }
        BaseStrength = StatEnums.Worst;
        BaseConstitution = StatEnums.Bad;
        BaseDexterity = StatEnums.Good;
        BaseIntelligence = StatEnums.Best;
        OffSet = new Vector3(-2f, 0f, 0f);
    }
}
