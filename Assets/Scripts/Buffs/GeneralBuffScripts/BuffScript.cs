using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuffScript : MonoBehaviour {

    public int 
        strength, 
        duration;
    protected GameObject 
        indicator;
    void OnEnable()
    {
        CameraScript.StartinPlayerTurn += Decrement;
    }

    void OnDisable()
    {
        CameraScript.StartinPlayerTurn -= Decrement;
    }

    private void Decrement()
    {
        duration--;
        Decrease();
        if(duration <= 0)
        {
            Remove();
        }
    }
    protected abstract void Decrease();
    public abstract void Remove();
    public abstract void SetUp(int strong, int time);

}
