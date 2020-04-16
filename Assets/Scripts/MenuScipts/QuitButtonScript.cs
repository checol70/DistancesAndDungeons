using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitButtonScript : MonoBehaviour {

    public static GameObject QuitObject;

    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(Pressed);
    }
    public void Pressed()
    {
        QuitObject.SetActive(true);
    }
}
