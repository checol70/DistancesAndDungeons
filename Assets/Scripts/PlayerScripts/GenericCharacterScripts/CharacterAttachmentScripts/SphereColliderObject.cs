using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SphereColliderObject : MonoBehaviour {
    private GameObject Player;
    List<GameObject> hitObjects = new List<GameObject>();
	// this is for brace and weapons like that

	void Awake ()
    {
        Player = gameObject.transform.root.gameObject;
        Player.GetComponent<CharacterScript>().SphereColliderObject = gameObject;
	}
    
    void OnTriggerEnter(Collider other)
    {
        if(!hitObjects.Contains(other.gameObject))
        if (CameraScript.GameController.MonsterTurn)
            if (!Player.GetComponent<CharacterScript>().Unarmed)
                if (Player.GetComponent<CharacterScript>().WeaponSlot.transform.GetChild(0).gameObject.GetComponent<WeaponScript>() != null)
                    if (other.gameObject.CompareTag("Monster"))
                    {
                        Player.GetComponent<CharacterScript>().WeaponSlot.transform.GetChild(0).GetComponent<WeaponScript>().ChargingEnemy(other.gameObject);
                        hitObjects.Add(other.gameObject);
                    }
    }
}
