using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainCanvasScript : MonoBehaviour {
    public static GameObject
        MainCanvas;
	// Use this for initialization
	void Awake ()
    {
	if(MainCanvas == null)
        {
            MainCanvas = gameObject;
        }
    if(gameObject != MainCanvas)
        {
            Destroy(gameObject);
        }
	}

}
