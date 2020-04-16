using UnityEngine;
using System.Collections;

public class KnightGuy : SpecificCharacterScript
{
    public static GameObject knightGuy;
    void Awake()
    {
        CharacterName = "KnightGuy";
        if (knightGuy == null)
        {
            knightGuy = gameObject;
            DontDestroyOnLoad(gameObject);
            Debug.Log("KnightGuyAdded");
            IsCharacter = true;
        }
        else if (knightGuy != gameObject)
        {
            StartCoroutine(DestroySoon());
        }
        BaseStrength = StatEnums.Average;
        BaseConstitution = StatEnums.Best;
        BaseDexterity = StatEnums.Average;
        BaseIntelligence = StatEnums.Worst;
        OffSet =new Vector3 (0f,0f,2f);
    }
    
}
