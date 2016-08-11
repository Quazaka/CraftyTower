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
public interface IWaveLevel
{
    int waveLevel
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

public interface IWaveEnemyCountLeft
{
    int WaveEnemyCountLeft
    {
        get;
    }
}

public interface IWaveTimeLeft
{
    float WaveTimeLeft
    {
        get;
    }
}

public interface IWaveEnemyType
{
    string WaveEnemyType
    {
        get;
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
