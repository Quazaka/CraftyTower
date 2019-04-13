using UnityEngine;

[CreateAssetMenu]
public class EnemySettings : ScriptableObject{

    public float health;
    public float attackDamage;
    public float attackRate;
    public float moveSpeed;

    // Normal
    //        health = 1.5f;
    //        attackDamage = 1f;
    //        attackRate = 1f;
    //        moveSpeed = 1f;

    // Fast
    //        health = 1.3f;
    //        attackDamage = 0.1f;
    //        attackRate = 1f;
    //        moveSpeed = 2f;


    // Boss
    //        health = 20f;
    //        attackDamage = 1f;
    //        attackRate = 1f;
    //        moveSpeed = 0.2f;
}
