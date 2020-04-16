using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairFireScript : MonoBehaviour {

    IList<GameObject> TrappedIndividuals = new List<GameObject>();

    

    private void Awake()
    {
        gameObject.transform.parent.gameObject.GetComponent<StairHolderScript>().FireObject = gameObject;
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        StartCoroutine(Trap());
    }
    private void OnTriggerEnter(Collider other)
    {
        TrappedIndividuals.Add(other.gameObject);
    }

    IEnumerator Trap()
    {
        yield return null;
        yield return null;
        foreach (GameObject hit in TrappedIndividuals)
        {
            if (hit.GetComponent<HealthScript>())
            {
                hit.GetComponent<HealthScript>().Damaged(15, DamageType.Fire);
            }
        }
        yield return new WaitWhile(() => gameObject.GetComponent<ParticleSystem>().IsAlive());
        TrappedIndividuals.Clear();
        gameObject.SetActive(false);
    }
}
