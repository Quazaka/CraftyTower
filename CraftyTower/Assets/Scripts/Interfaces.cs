using UnityEngine;
using System.Collections;

public interface IDamage
{
    float takeDamage
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

#region Wave
public interface IWave
{
    int level
    {
        get;
        set;
    }

    int enemiesAlive
    {
        get;
        set;
    }

    int enemiesSpawned
    {
        get;
        set;
    }
}
#endregion


public interface IGameOver
{
    bool isGameOver
    {
        get;
        set;
    }
}
