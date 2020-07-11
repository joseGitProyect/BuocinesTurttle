using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedControl : MonoBehaviour
{
    public float maxSpeed;
    Rigidbody rb;
    Vector3 velocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        velocity = rb.velocity;
        velocity = velocity.normalized * maxSpeed;
        rb.velocity = velocity;
    }
}
