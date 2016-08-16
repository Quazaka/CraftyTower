using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public abstract class BaseWeapon : MonoBehaviour {
    //Projectile
    public GameObject projectilePrefab;

    //Enemy list
    public List<GameObject> enemyList = new List<GameObject>();

    //Caseswith to choose targeting type
    protected int targetSwitch = 1;

    //Current Target
    protected GameObject currentTarget;

    public abstract float cooldown { get; }
    public abstract float range { get; }

    void Start()
    {
        StartCoroutine(Co_ShootAtEnemies());
        StartCoroutine(Co_UpdateEnemyList());
    }

    void Update () {

    }

    //Detect collision with enemy
    void OnTriggerEnter(Collider co)
    {
        if (co.GetComponent<BaseEnemy>())
        {
            enemyList.Add(co.gameObject);
        }
    }

    //Get enemies in range of a center
    internal List<GameObject> GetEnemisInRange(Vector3 center, float radius)
    {
        //OverlapSphere returns an array - converted to list here
        Collider[] hitCollidersArray = Physics.OverlapSphere(center, radius);
        List<GameObject> hitCollidersList = new List<GameObject>();

        //"Convert" the Array<Collider> to List<GameObject>
        for (int i = 0; i < hitCollidersArray.Length; i++)
        {
            hitCollidersList.Add(hitCollidersArray[i].gameObject);
        } 

        //Remove non enemies from list
        for (int i = hitCollidersList.Count - 1; i >= 0; i--)
        {
            if (!hitCollidersList[i].GetComponent<BaseEnemy>())
            {
                hitCollidersList.RemoveAt(i);
            }
        }

        //Return list
        return hitCollidersList;
    }

    //Get Every enemy in range and add them to enemy list
    IEnumerator Co_UpdateEnemyList()
    {
        while (true)
        {
            enemyList = GetEnemisInRange(transform.position, range);

            yield return new WaitForSeconds(0.5f);
        }
    }

    //Select targeted unit and shoot.
    IEnumerator Co_ShootAtEnemies()
    {
        while (true)
        {
            while (enemyList.Count > 0 && enemyList != null)
            {
                //Targeting script
                Targeting scriptTargeting = GetComponent<Targeting>();
                currentTarget = scriptTargeting.ChooseTargetScanType(enemyList, targetSwitch);

                if (isTargetNull(currentTarget)) { enemyList.Remove(currentTarget); break; }
                //Access target futureHealth using IHealth
                IHealth enemyHealth = currentTarget.GetComponent<BaseEnemy>();

                //Prevent overkill
                if (enemyHealth.futureHealth > 0)
                {
                    Shoot(currentTarget); // shoot
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

    //Check for null reference
    bool isTargetNull(GameObject currentTarget)
    {
        if (currentTarget == null)
        {
            return true;
        }
        return false;
    }

    protected void RemoveNullObjectFromList(List<GameObject> enemies)
    {
        enemyList = enemyList.Where(item => item != null).ToList();

    }

    //Arrow implementation of shoot
    protected virtual void Shoot(GameObject currentTarget)
    {
        //Remove null targets from enemyList
        RemoveNullObjectFromList(enemyList);

        //Create projectile and set it's target
        GameObject projectile = (GameObject)Instantiate(projectilePrefab, transform.position, Quaternion.identity); //create projectile
        projectile = setTarget(projectile, currentTarget);

        //Set future health to prevent overkill
        IHealth enemyHealth = currentTarget.GetComponent<BaseEnemy>();
        enemyHealth.futureHealth -= GetProjectileDamage(projectile);
    }

    //Set target in subclass to ensure corret script is initialzed on projectile
    protected abstract GameObject setTarget(GameObject projectile, GameObject currentTarget);

    protected abstract float GetProjectileDamage(GameObject projectile);
}
