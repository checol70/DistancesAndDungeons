using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectButtonMaxManaIndicatorScript : MonoBehaviour {

    private GameObject Player;
    // Use this for initialization
    void Awake()
    {
        gameObject.transform.parent.gameObject.GetComponent<CharacterSelectButtonScript>().MaxManaIndicator = gameObject;
    }
    public void Assign()
    {
        Player = gameObject.transform.parent.gameObject.GetComponent<CharacterSelectButtonScript>().CharacterAssigned;
        Player.GetComponent<CharacterScript>().CharacterButtonMaxManaIndicator = gameObject.GetComponent<Slider>();
        Player.GetComponent<CharacterScript>().ButtonCreated();
    }
}
