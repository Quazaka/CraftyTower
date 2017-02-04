using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public abstract class BaseWeapon : MonoBehaviour {

    //Interfaces
    protected IHealth enemyHealth;
    protected IDamage dealDamageToEnemy;

    //Enemy list
    public List<GameObject> enemyList = new List<GameObject>();

    //Caseswith to choose targeting type
    protected int targetSwitch = 1;

    //Current Target
    protected GameObject currentTarget;

    //Firerate and range defined here as they are needed for all weapons
    public abstract float Firerate { get; set; }
    public abstract float Range { get; set; }

    void Start()
    {
        StartCoroutine(Co_ShootAtEnemies());
        StartCoroutine(Co_UpdateEnemyList());
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
    internal List<GameObject> GetEnemiesInRange(Vector3 center, float radius)
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
            enemyList = GetEnemiesInRange(transform.position, Range);

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
                    Debug.Log("Shooting");
                    Shoot(currentTarget); // shoot
                }
                else
                {
                    enemyList.Remove(currentTarget);
                    currentTarget = null;
                }
                yield return new WaitForSeconds(Firerate);
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

    //Implementation of shoot
    private void Shoot(GameObject currentTarget)
    {
        //Remove null targets from enemyList
        RemoveNullObjectFromList(enemyList);

        //Ready up the weapon and shoot at the target
        ReadyWeapon(currentTarget, true);
    }

    //Readies the weapon by calculating the projectile damage and setting the target.
    //bool determines whether modifiers such as crit chance etc. are used.
    protected abstract void ReadyWeapon(GameObject target, bool useModifiers);

    //Prevents the weapon from shooting projectiles at enemies that would be dead at next hit
    //Damage done is handled by each weapons unique projectile
    protected void PreventMultipleProjectiles(BaseProjectile projectile)
    {
        //Calculate the future health of an enemy based on what projectile is supposed to hit it
        enemyHealth = currentTarget.GetComponent<BaseEnemy>();
        enemyHealth.futureHealth -= projectile.Damage;
        //Debug.Log("future health: " + enemyHealth.futureHealth);
    }

    //Prevents weapons without projectiles attacking enemies more than necessary
    //Damage done is passed as an argument in the weapons class
    protected void PreventMultipleAttacks(GameObject target, float damage)
    {
        enemyHealth = target.GetComponent<BaseEnemy>();
        enemyHealth.futureHealth -= damage;

        //Damage target instantly using the IDamage interface because there is no projectile
        dealDamageToEnemy = target.GetComponent<BaseEnemy>();
        dealDamageToEnemy.damage = damage;
    }

    //Calculate modified damage in subclasses
    protected abstract float CalculateDamageWithVariables();
}
