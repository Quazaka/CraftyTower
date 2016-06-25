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
    //Tempoary enemy health used in Update()
    private float futureHealth;

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
            //Pick first GameObject in list as target
            GameObject currentTarget = enemyList.FirstOrDefault();      

            //Chekc for null reference
            if (isTargetNull(currentTarget)) {enemyList.Remove(currentTarget); return;}

            //Access target health using IHealth
            IHealth enemyHealth = currentTarget.GetComponent<Enemy>();  
            futureHealth = enemyHealth.health;
            Debug.Log(futureHealth);

            //Handle cooldown
            nextShot = Time.time + cooldown;
            
            //Create projectile
            GameObject projectile = (GameObject)Instantiate(projectilePrefab, transform.position, Quaternion.identity);


            //Prevent overkill - multiple projectile after killing projectile have been initiated
            int i = 0;
            while (futureHealth <= 0 && enemyList.Count >= i)
            {
                currentTarget = enemyList[i];
                if (isTargetNull(currentTarget)) { enemyList.Remove(currentTarget); return; }
                i++;
            }

            //Shoot at enemy
            Shoot(currentTarget, projectile);
        }
    }
    //Detect collision with enemy
    void OnTriggerEnter(Collider co)
    {
        if (co.GetComponent<Enemy>())
        {
            enemyList.Add(co.gameObject);
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
    void Shoot(GameObject currentTarget, GameObject projectile)
    {
        projectile.GetComponent<Projectile>().target = currentTarget.transform;
        futureHealth = futureHealth - projectile.GetComponent<Projectile>().damage;
    }
}
