using UnityEngine;
using System.Collections;
using System;

public abstract class ProjectileBase : MonoBehaviour {

    private IDamage enemyHit;

    // Target (set by weapon)
    public Transform target;

    //Bullet damage
    public abstract float damage { get; }

    //Speed
    public abstract float speed { get; }


    // Update is called once per frame
    void FixedUpdate() {
        // Still has a Target?
        if (target)
        {
            // Fly towards the target        
            Vector3 dir = target.position - transform.position;
            GetComponent<Rigidbody>().velocity = dir.normalized * speed;
        }
        else
        {
            // Otherwise destroy self
            Destroy(gameObject);
        }

    }

    // Monster Hit
    void OnTriggerEnter(Collider co)
    {
        if (co.GetComponent<Enemy>())
        {
            enemyHit = (IDamage)co.GetComponent<Enemy>();
            enemyHit.damage = damage;
            Destroy(gameObject);
        }
    }
}
