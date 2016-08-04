using UnityEngine;
using System.Collections;

public class FastEnemy : BaseEnemy {

    protected override void Start()
    {
        base.Start();

        //Enemy stats
        _moveSpeed = 2;
        _attackDmg = CalculateDamage(currentWaveLevel);
        _attackRate = 1;
        _damageReduction = 0;
        _futureHp = CalculateHp(currentWaveLevel);
        _hp = CalculateHp(currentWaveLevel);
    }

    public override float attackDmg
    {
        get { return _attackDmg; }
        set { _attackDmg = value; }
    }

    public override float attackRate
    {
        get { return _attackRate; }
        set { _attackRate = value; }
    }

    //TODO implement as armor
    public override float damageReduction
    {
        get { return _damageReduction; }
        set { damageReduction = value; }
    }

    public override float futureHp
    {
        get { return _futureHp; }
        set { _futureHp = value; }
    }

    public override float hp
    {
        get { return _hp; }
        set { _hp = value; }
    }

    public override float moveSpeed
    {
        get { return _moveSpeed; }
        set { _moveSpeed = value; }
    }

    //Computations with CPU POWER - extrem performance.
    protected override float CalculateDamage(int wave)
    {
        float damage = (0.05f * wave);
        return damage;
    }

    protected override int CalculateHp(int wave)
    {
        int hp = (int)Mathf.Pow(wave, 1.3f);
        return hp;
    }
}
