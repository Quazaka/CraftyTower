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
    }

    int enemyCountLeft
    {
        get;
        set;
    }

    //float timeLeft
    //{
    //    get;
    //}

    string enemyType
    {
        get;
    }
}

public interface IWaveEnemyHp
{
    int damage
    {
        get;
    }
}
#endregion

public interface IExperience
{
    int experience
    {
        set;
    }
}

public interface IGameOver
{
    bool isGameOver
    {
        get;
        set;
    }
}

