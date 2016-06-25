using UnityEngine;
using System.Collections;

public interface IDamage
{
    float damage
    {
        set;
    }
}
public interface IHealth
{
    float health
    {
        get;
        set;
    }
    float futureHealth
    {
        get;
        set;
    }
}


