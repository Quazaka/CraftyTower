using UnityEngine;
using System.Collections;
using System;

public abstract class BaseProjectile : MonoBehaviour {

    protected IDamage enemyHit;

    // Target (set by weapon)
    public Transform target;

    //Bullet damage
    public abstract float Damage { get; set; }


    //Speed
    public abstract float Speed { get; }

    // Update is called once per frame
    void FixedUpdate() {
        // Still has a Target?
        if (target)
        {
            // Fly towards and face target        
            transform.rotation = Quaternion.LookRotation(target.transform.position)* Quaternion.Euler(0, 90, 0); // 90 degrees to face enemy correctly
            Vector3 dir = target.position - transform.position;
            GetComponent<Rigidbody>().velocity = dir.normalized * Speed;
        }
        else
        {
            // Otherwise destroy self
            Destroy(gameObject);
        }
    }

    // Monster Hit
    protected virtual void OnTriggerEnter(Collider co)
    {
        if (co.GetComponent<BaseEnemy>())
        {
            enemyHit = (IDamage)co.GetComponent<BaseEnemy>();
            enemyHit.damage = Damage;
            Destroy(gameObject);
        }
    }
}
