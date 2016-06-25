using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Weapon : MonoBehaviour {
    //Projectile
    public GameObject projectilePrefab;

    //Enemy list
    public List<GameObject> enemyList = new List<GameObject>();

    //Caseswith to choose targeting type
    private int caseSwitch = 1;

    private float cooldown = 0.5f;
    public float range = 10f;


    void Start()
    {
        //Set range on weapon/spear collider
        SphereCollider sc = GetComponent("SphereCollider") as SphereCollider;
        sc.radius = range;
        StartCoroutine(Co_ShootAtEnemies());
    }

    // Update is called once per frame
    void Update () {

    }

    //Detect collision with enemy
    void OnTriggerEnter(Collider co)
    {
        if (co.GetComponent<Enemy>())
        {
            enemyList.Add(co.gameObject);
        }
    }

    //Detect closest unit and shoot.
    IEnumerator Co_ShootAtEnemies()
    {
        while (true)
        {
            while (enemyList.Count > 0)
            {
                GameObject currentTarget = ChooseTargetScanType(enemyList);
                if (isTargetNull(currentTarget)) { enemyList.Remove(currentTarget); break; }
                //Access target futureHealth using IHealth
                IHealth enemyHealth = currentTarget.GetComponent<Enemy>();

                //Prevent overkill
                if (enemyHealth.futureHealth > 0)
                {
                    Shoot(currentTarget);
                    Debug.Log("Shooting");
                }
                else
                {
                    enemyList.Remove(currentTarget);
                    currentTarget = null;
                }
                yield return new WaitForSeconds(cooldown);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    //Chekc for null reference
    bool isTargetNull(GameObject currentTarget)
    {
        if (currentTarget == null)
        {
            return true;
        }
        return false;
    }

    //Shoot projectile at target
    void Shoot(GameObject currentTarget)
    {
        GameObject projectile = (GameObject)Instantiate(projectilePrefab, transform.position, Quaternion.identity); //create projectile
        projectile.GetComponent<Projectile>().target = currentTarget.transform; //set target
        
        //Set future health to prevent overkill
        IHealth enemyHealth = currentTarget.GetComponent<Enemy>();
        enemyHealth.futureHealth -= projectile.GetComponent<Projectile>().damage;
    }

    //Search for every enemy in range and pick the closest
    //Caseswitch 1
    GameObject GetClosestEnemy(List<GameObject> enemies)
    {
        GameObject closestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach(GameObject potentialTarget in enemies) //Find distance to each enemy in range
        {
            if (isTargetNull(potentialTarget)) { enemyList.Remove(potentialTarget); break; }
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrTotarget = directionToTarget.sqrMagnitude;
                if (dSqrTotarget < closestDistanceSqr) //if distance is lowest store it with the enemy
                {
                    closestDistanceSqr = dSqrTotarget;
                    closestTarget = potentialTarget;
                }
        }
        return closestTarget; //return loweste distance enemy.
    }

    //TODO improve
    //Search for every enemy in range and pick the furthest
    //Caseswitch 2
    GameObject GetFurthestEnemy(List<GameObject> enemies)
    {
        GameObject closestTarget = null;
        float furthestDistanceSqr = -1;
        Vector3 currentPosition = transform.position;
        foreach (GameObject potentialTarget in enemies) //Find distance to each enemy in range
        {
            if (isTargetNull(potentialTarget)) { enemyList.Remove(potentialTarget); break; }
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrTotarget = directionToTarget.sqrMagnitude;
            if (dSqrTotarget > furthestDistanceSqr) //if distance is furthest store it with the enemy
            {
                furthestDistanceSqr = dSqrTotarget;
                closestTarget = potentialTarget;
            }
        }
        return closestTarget; //return loweste distance enemy.
    }


    //Pick a random unit in range
    //Caseswitch 3
    GameObject GetRandomEnemy(List<GameObject> enemies)
    {
        int i = Random.Range(0, enemies.Count);
        return enemies[i];
    }

    GameObject ChooseTargetScanType(List<GameObject> enemies)
    {
        GameObject currentTarget;
        switch (caseSwitch)
        {
            case 1:
                currentTarget = GetClosestEnemy(enemies);
                break;
            case 2:
                currentTarget = GetRandomEnemy(enemies);
                break;
            case 3:
                currentTarget = GetFurthestEnemy(enemies);
                break;
            default:
                currentTarget = GetClosestEnemy(enemies);
                break;
        }
        return currentTarget;
    }
}
