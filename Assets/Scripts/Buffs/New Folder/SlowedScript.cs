using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowedScript : BuffScript {
    public float origMovement;
    public override void SetUp(int strong, int time)
    {
        if (gameObject.GetComponent<StunnedScript>())
        {
            origMovement = gameObject.GetComponent<StunnedScript>().originalMovement;

            
        }
        else if (!gameObject.GetComponent<StunnedScript>())
        {
            if (gameObject.GetComponent<MonsterScript>())
            {
                MonsterScript mon = gameObject.GetComponent<MonsterScript>();
                origMovement = mon.MaxMovement;
                Effect();
            }
            else if (gameObject.GetComponent<CharacterScript>())
            {
                CharacterScript chara = gameObject.GetComponent<CharacterScript>();
                origMovement = chara.modifiedMaxMovement;
                Effect();
            }
        }    
    }

    private void Effect()
    {
        if (gameObject.GetComponent<MonsterScript>())
        {
            MonsterScript mon = gameObject.GetComponent<MonsterScript>();
            mon.MaxMovement = 10 / (1 + strength);
        }
        if (gameObject.GetComponent<CharacterScript>())
        {
            CharacterScript c = gameObject.GetComponent<CharacterScript>();
            c.modifiedMaxMovement = 10 / (1+ strength); 
        }
    }

    private IEnumerator WaitForStun()
    {
        yield return new WaitWhile(() => gameObject.GetComponent<StunnedScript>());
        Effect();
    }

    public override void Remove()
    {
        
    }
    protected override void Decrease()
    {

    }
}
