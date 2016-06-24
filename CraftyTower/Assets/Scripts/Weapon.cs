using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Weapon : MonoBehaviour {
    //Projectile
    public GameObject projectilePrefab;

    //firerate
    private float cooldown = 1.0f;
    private float nextShot = 0.0f;

    //Range
    private float range = 5.0f;

    //Enemy list
    private List<GameObject> enemyList = new List<GameObject>();

    void Start()
    {
        Debug.Log("Weapon created");
    }

    // Update is called once per frame
    void Update () {
        if (Time.time > nextShot && enemyList.Count > 0)
        {
            GameObject currentTarget = enemyList.FirstOrDefault();
            if (currentTarget == null) { 
                enemyList.Remove(currentTarget);
                return;
            }
            nextShot = Time.time + cooldown;
            GameObject g = (GameObject)Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            g.GetComponent<Projectile>().target = currentTarget.transform;

        }
    }

    void OnTriggerEnter(Collider co)
    {
        if (co.GetComponent<Enemy>())
        {
            enemyList.Add(co.gameObject);
        }
    }
}
