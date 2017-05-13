using UnityEngine;
using System.Collections;
using System;

public class BaseProjectile : MonoBehaviour {

    protected IDamage targetIDamage;

    // Target (set by weapon)
    public Transform target;

    public float Damage { get; set; }
    protected virtual float Speed { get; set; }

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
            targetIDamage = co.GetComponent<BaseEnemy>();
            targetIDamage.takeDamage = Damage;
            Destroy(gameObject);
        }
    }
}
