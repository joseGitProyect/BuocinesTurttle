using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableMovible : InteractableObject
{
    public bool isMovible;
    public float speed;
    private void Awake()
    {
        if (isMovible)
        {
            if (!GetComponent<SpeedControl>())
            {
                gameObject.AddComponent<SpeedControl>();
            }
            GetComponent<SpeedControl>().maxSpeed = speed;
        }
        Awake2();
    }

    protected virtual void Awake2()
    {

    }
}
