﻿using UnityEngine;
using System.Collections;
using System;

public class Bullet : MonoBehaviour, Damage {

    // Target (set by tower)
    public Transform tra_target;
    public GameObject go_target;

    //Bullet damage
    public int damage;

    //Speed
    public float speed = 10;


    // Update is called once per frame
    void FixedUpdate() {
        // Still has a Target?
        if (tra_target)
        {
            // Fly towards the target        
            Vector3 dir = tra_target.position - transform.position;
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
        EnemyController p = co.GetComponent<EnemyController>();
        if (co.tag == "Enemy")
        {
            p.Damage = damage;
            
        }

    }
}
