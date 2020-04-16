using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationScript : MonoBehaviour {

    // this script is for the characters distance checker, to rotate weapons to face where the mouse is pointing.

    public float speed = 5f;

    private RaycastHit pointTo;

    private int floor;

    private void Start()
    {
        floor = 1 << 9;
    }
    // Update is called once per frame
    void Update ()
    {

		if(CameraScript.GameController.ActivePlayer == gameObject.transform.root.gameObject)
        {

            Ray Hitter = CameraScript.GameController.gameObject.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(Hitter, out pointTo, 60f, floor, QueryTriggerInteraction.Ignore);
            if(pointTo.collider != null)
            {
                Vector3 vase = new Vector3(pointTo.point.x, gameObject.transform.position.y, pointTo.point.z);
                Vector3 targetDir =  vase - transform.position;

                // The step size is equal to speed times frame time.
                float step = speed * Time.deltaTime;

                Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
                Debug.DrawRay(transform.position, newDir, Color.red);

                // Move our position a step closer to the target.
                transform.rotation = Quaternion.LookRotation(newDir);
            }
        }
	}
}
