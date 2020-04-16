using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ScrollScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private Vector3
        max,
        min;
    bool
        moving;
    protected Vector3 
        offset;
    float 
        t;

    public void OnPointerEnter(PointerEventData p)
    {
        max = offset + CameraScript.GameController.GetComponent<CameraScript>().offSet;
        min = CameraScript.GameController.GetComponent<CameraScript>().offSet;
        t = 0;
        moving = true;
    }
    private void Update()
    {
        if (moving)
        {
            t += Time.deltaTime *4;
            Vector3 lOff = Vector3.LerpUnclamped(min, max, t);
            CameraScript.GameController.GetComponent<CameraScript>().offSet = lOff;
        }
    }

    public void OnPointerExit (PointerEventData p) {
        moving = false;
	}
}
