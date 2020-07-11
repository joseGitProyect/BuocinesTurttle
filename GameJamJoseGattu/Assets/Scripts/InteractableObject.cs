using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    ObjectType objectType;

}

public enum ObjectType
{
    bouncinesObject,  powerUp, Enemy
}
