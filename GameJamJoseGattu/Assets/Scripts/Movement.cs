using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void MoveRB(float _x, float _z, float _speed)
    {
        Vector3 dir = rb.velocity;
        dir.x = _x * _speed;
        dir.z = _z * _speed;
        rb.velocity = dir;
    }

    public void Jump(Transform _shoes, float _jumpForce, LayerMask _jumpeable)
    {
        if(Physics.Raycast(new Ray(_shoes.position,-_shoes.up),0.01f,_jumpeable))
        {
            rb.AddForce(_shoes.up*_jumpForce, ForceMode.Impulse);
        }
    }

    public void Dash(Vector3 _direction, float _force)
    {
        rb.AddForce(_direction * _force, ForceMode.Impulse);
    }
}
