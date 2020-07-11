using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public ObjectType objectType;
    public float bouncinesPower;

    public virtual void DoEffect(PlayerMain playerMain)
    {

    }
}

public enum ObjectType
{
    bouncinesObject,  powerUp, Enemy
}
