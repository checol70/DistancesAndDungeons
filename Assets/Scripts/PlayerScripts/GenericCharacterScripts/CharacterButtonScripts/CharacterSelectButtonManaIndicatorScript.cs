using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectButtonManaIndicatorScript : MonoBehaviour {

    private GameObject Player;
    // Use this for initialization
    void Awake()
    {
        gameObject.transform.parent.parent.parent.gameObject.GetComponent<CharacterSelectButtonScript>().ManaIndicator = gameObject;
    }


    public void Assign()
    {
        Player = gameObject.transform.parent.parent.parent.gameObject.GetComponent<CharacterSelectButtonScript>().CharacterAssigned;
        Player.GetComponent<CharacterScript>().CharacterButtonManaIndicator = gameObject.GetComponent<Slider>();
        Player.GetComponent<CharacterScript>().ButtonCreated();
    }
}
