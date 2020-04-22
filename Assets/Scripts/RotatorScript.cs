using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorScript : MonoBehaviour
{
    Rigidbody rb;
    private Vector3 rotate = new Vector3(0, 0, 2.5f);
    // Start is called before the first frame update
    void Awake()
    {
        gameObject.transform.root.gameObject.GetComponent<CharacterScript>().RangeIndicator = gameObject;
        rb = gameObject.GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Quaternion rotation = Quaternion.Euler(rotate);
        rb.MoveRotation(rb.rotation * rotation);
    }
}
