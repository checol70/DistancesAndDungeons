using UnityEngine;
using System.Collections;

public class CharacterMeshObjectScript : MonoBehaviour {
    GameObject BasePlayer;
	// Use this for initialization
	void Awake () {
        BasePlayer = gameObject.transform.root.gameObject;
        gameObject.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("CharacterButtonAndAnimatorController/CharacterAnimatorController") as RuntimeAnimatorController;
        BasePlayer.GetComponent<CharacterScript>().CharacterMeshObject = gameObject;
	}
	
	
}
