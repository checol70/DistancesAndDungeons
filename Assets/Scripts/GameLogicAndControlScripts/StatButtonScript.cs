using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatButtonScript : MonoBehaviour {
    public static GameObject StatButton;
    // Use this for initialization
    void Start()
    {
        if (StatButton == null)
        {
            StartCoroutine(SetStatButton());
            StatButton = gameObject;
        }
        if (StatButton != gameObject)
        {
            Destroy(gameObject);
        }
    }
	IEnumerator SetStatButton()
    {
        yield return new WaitWhile(() => CameraScript.GameController == null);
        CameraScript.GameController.StatButton = gameObject.GetComponent<Button>();
        gameObject.GetComponent<Button>().onClick.AddListener(CameraScript.GameController.OpenTheStatPage);
    }
	
}
