using UnityEngine;
using System.Collections;

public class BowChick : SpecificCharacterScript
{
    public static GameObject bowChick;
    void Awake()
    {
        CharacterName = "BowChick";
        if (bowChick == null)
        {
            bowChick = gameObject;
            DontDestroyOnLoad(gameObject);
            Debug.Log("BowChickAdded");
            IsCharacter = true;
        }
        else if (gameObject != bowChick)
        {
            StartCoroutine(DestroySoon());
        }
        BaseStrength = StatEnums.Bad;
        BaseConstitution = StatEnums.Worst;
        BaseDexterity = StatEnums.Best;
        BaseIntelligence = StatEnums.Good;
        OffSet = new Vector3(2f, 0f, 0f);
    }
}
