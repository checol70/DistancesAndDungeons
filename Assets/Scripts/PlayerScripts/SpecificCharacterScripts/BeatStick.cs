using UnityEngine;
using System.Collections;

public class BeatStick : SpecificCharacterScript {
    public static GameObject beatStick;
    void Awake()
    {
        CharacterName = "BeatStick";
        if(beatStick == null)
        {
            beatStick = gameObject;
            IsCharacter = true;
        }
        else if (gameObject != beatStick)
        {
            StartCoroutine(DestroySoon());
        }
    
        BaseStrength = StatEnums.Best;
        BaseConstitution = StatEnums.Good;
        BaseDexterity = StatEnums.Worst;
        BaseIntelligence = StatEnums.Worst;
        OffSet = new Vector3(0f, 1f, -2f);
    }
}
