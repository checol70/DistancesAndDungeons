using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterRotatorScript : MonoBehaviour
{
    private Vector3 rotate = new Vector3(0, 2.5f, 0);
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Quaternion rotation = Quaternion.Euler(rotate);
        rb.MoveRotation(rb.rotation * rotation);
    }
}
