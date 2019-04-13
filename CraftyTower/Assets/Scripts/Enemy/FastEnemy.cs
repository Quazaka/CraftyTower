using UnityEngine;
using System.Collections;
using CraftyTower.Spawner;

public class FastEnemy : Enemy, IEnemy {

    public void Move(Vector3 target)
    {
        Debug.Log("SHIT");
    }

    //protected override void Start()
    //{
    //    base.Start();

    //    //Enemy stats
    //    MoveSpeed = 2;
    //    AttackDamage = CalculateDamage(0.05f, Wave.level);
    //    AttackRate = 1;
    //    DamageReduction = 0;        
    //    health = CalculateHealth(1.3f, Wave.level);
    //    futureHealth = health;

    //    OnSpawn();
    //}
}
