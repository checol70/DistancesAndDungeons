using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaPlaneScript : MonoBehaviour {

    int detailID,
        mainID;
    public static GameObject
        lavaPlane;
    private void Start()
    {
       detailID = Shader.PropertyToID("_DetailAlbedoMap");
        mainID = Shader.PropertyToID("_MainTex");
        gameObject.transform.eulerAngles = new Vector3(90, 0, 0);
        lavaPlane = gameObject.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update () {
        gameObject.GetComponent<MeshRenderer>().material.SetTextureOffset(detailID, new Vector2(Time.time * .01f, Time.time * .02f));
        gameObject.GetComponent<MeshRenderer>().material.SetTextureOffset(mainID, new Vector2(Time.time * -.02f+.5f, Time.time * -.01f));
    }
}
