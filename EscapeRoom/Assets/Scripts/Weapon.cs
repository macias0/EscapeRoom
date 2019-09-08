using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Weapon : Item, IFireable
{

    [SerializeField]
    protected float range = 5.0f;

    public abstract void Fire();

}
