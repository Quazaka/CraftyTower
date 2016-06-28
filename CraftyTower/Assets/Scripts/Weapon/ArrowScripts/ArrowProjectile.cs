using UnityEngine;
using System.Collections;
using System;

public class ArrowProjectile : BaseProjectile
{

    public override float damage
    {
        get{ return 5.0f; }
    }

    public override float speed
    {
        get{ return 15.0f; }
    }
}
