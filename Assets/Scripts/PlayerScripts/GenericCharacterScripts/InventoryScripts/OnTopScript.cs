using UnityEngine;
using System.Collections;

public class OnTopScript : MonoBehaviour {

void Awake()
    {
        gameObject.transform.root.GetComponent<CharacterScript>().OnTopSlot = this.gameObject;
    }
}
