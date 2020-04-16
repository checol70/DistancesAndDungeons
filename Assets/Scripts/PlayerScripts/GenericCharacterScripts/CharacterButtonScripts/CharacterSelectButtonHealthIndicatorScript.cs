using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterSelectButtonHealthIndicatorScript : MonoBehaviour {

    private GameObject Player;
	// Use this for initialization
    void Awake()
    {
        gameObject.transform.parent.gameObject.GetComponent<CharacterSelectButtonScript>().HealthIndicator = gameObject;
    }
	public void Assign ()
    {
        Player = gameObject.transform.parent.gameObject.GetComponent<CharacterSelectButtonScript>().CharacterAssigned;
        Player.GetComponent<HealthScript>().CharacterButtonHealthIndicator = gameObject.GetComponent<Slider>();
        Player.GetComponent<CharacterHealthScript>().ButtonCreated();
	}
}
