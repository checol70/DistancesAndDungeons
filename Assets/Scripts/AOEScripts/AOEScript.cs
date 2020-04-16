using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AOEScript : MonoBehaviour {

    protected int 
        TurnsRemaining,
        Damage;
    public DamageType
        damageType;
    private int layer;
    
    protected List<GameObject> TargetsAffected = new List<GameObject>();

    public void SetUp(int turnsRemaining, int damage, float size, GameObject target, int physLayer)
    {
        layer = physLayer;
        if (gameObject.GetComponent<SphereCollider>())
        {

        }
        else if (gameObject.GetComponent<CapsuleCollider>())
        {
            
        }
    }

    protected void TargetEntered(GameObject target)
    {
        TargetsAffected.Add(target);
    }

    protected abstract void Effect();

    protected abstract void EndingPlayerTurn();

    protected abstract void StartingPlayerTurn();

    void OnEnable()
    {
        CameraScript.StartinPlayerTurn += StartingPlayerTurn;
        CameraScript.UpdatingNavMesh += EndingPlayerTurn;
    }

    void OnDisable()
    {
        CameraScript.StartinPlayerTurn -= StartingPlayerTurn;
        CameraScript.UpdatingNavMesh -= EndingPlayerTurn;
    }
    protected abstract bool IsOpposedElement(DamageType test);
    private void OnTriggerEnter(Collider other)
    {
        if (!TargetsAffected.Contains(other.gameObject))
        {
            TargetsAffected.Add(other.gameObject);
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (!TargetsAffected.Contains(other.gameObject))
        {
            TargetsAffected.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        TargetsAffected.Remove(other.gameObject);
    }
}
