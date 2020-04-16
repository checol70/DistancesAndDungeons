using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleFlamingScript : FlamingScript {

    // Use this for initialization
    public override IEnumerator Ready()
    {
        Strength = 1;
        ManaPenalty = 3;
        ManaCost = 2;
        yield return new WaitWhile(() => Strength == 0);
        ManaCost = Strength;
        
	}
	
	
}
