using UnityEngine;
using System.Collections;
using System;

public class Projectile : MonoBehaviour {

    private Damage enemyHit;

    // Target (set by tower)
    public Transform target;

    //Bullet damage
    public int damage;

    //Speed
    public float speed = 10;


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
            enemyHit = (Damage)co.GetComponent<Enemy>();
            enemyHit.damage = damage;
            Destroy(gameObject);
        }
    }
}
