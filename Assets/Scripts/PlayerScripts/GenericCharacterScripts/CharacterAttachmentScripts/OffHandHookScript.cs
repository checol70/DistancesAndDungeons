using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffHandHookScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.root.gameObject.GetComponent<CharacterScript>().OffHandHooks.Add(transform);
    }
}
