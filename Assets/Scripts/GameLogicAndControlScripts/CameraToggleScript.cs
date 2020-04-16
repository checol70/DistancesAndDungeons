using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraToggleScript : MonoBehaviour {
    public static GameObject cameraToggle;
    void Awake()
    {
        if(cameraToggle == null)
        {
            cameraToggle = gameObject;
        }
        else if (cameraToggle != gameObject)
        {
            Destroy(gameObject);
        }
    }
	
    public void Toggled(bool movingCamera)
    {

    }
}
