using UnityEngine;
using System.Collections;
using System;

public class ArrowProjectile : BaseProjectile
{
    private float _damage;


    public override float Damage
    {
        get{ return _damage; }
        set { _damage = value; }
    }

    public override float Speed
    {
        get{ return 15.0f; }
    }
}
