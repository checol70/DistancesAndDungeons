using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashScript : MonoBehaviour
{
    public float speed;
    Material mat;
    private void Start()
    {
        mat = gameObject.GetComponent<Renderer>().material;
    }
    // Update is called once per frame
    void Update()
    {
        mat.mainTextureOffset += new Vector2(0, Time.deltaTime * speed);
    }
}
