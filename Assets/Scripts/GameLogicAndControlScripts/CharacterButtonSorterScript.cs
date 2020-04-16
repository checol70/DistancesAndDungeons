using UnityEngine;
using System.Collections;

public class CharacterButtonSorterScript : MonoBehaviour {
    public static GameObject characterButtonSorter;
    // Use this for initialization
    void Awake() {
        if (characterButtonSorter == null)
        {
            characterButtonSorter = gameObject;
            StartCoroutine(Setup());
        }
        else if(characterButtonSorter != gameObject)
        {
            Destroy(gameObject);
        }

    }
    IEnumerator Setup() {
        yield return new WaitWhile(() => CameraScript.GameController == null);
        CameraScript.GameController.CharacterButtonSorter = gameObject;
    }
	
	
}
