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
    private int targetSwitch = 1;

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
        if (co.GetComponent<Enemy>())
        {
            enemyList.Add(co.gameObject);
        }
    }

    //Get enemies in range of a center
    private List<GameObject> GetEnemisInRange(Vector3 center, float radius)
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
            if (!hitCollidersList[i].GetComponent<Enemy>())
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
            while (enemyList.Count > 0)
            {
                //Targeting script
                Targeting scriptTargetinng = GetComponent<Targeting>();
                GameObject currentTarget = scriptTargetinng.ChooseTargetScanType(enemyList, targetSwitch);

                if (isTargetNull(currentTarget)) { enemyList.Remove(currentTarget); break; }
                //Access target futureHealth using IHealth
                IHealth enemyHealth = currentTarget.GetComponent<Enemy>();

                //Prevent overkill
                if (enemyHealth.futureHealth > 0)
                {
                    Shoot(currentTarget);
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
    protected void Shoot(GameObject currentTarget)
    {
        RemoveNullObjectFromList(enemyList);

        GameObject projectile = (GameObject)Instantiate(projectilePrefab, transform.position, Quaternion.identity); //create projectile

        if (currentTarget == null)
        {
            RemoveNullObjectFromList(enemyList); //Remove null objects from target list
            return;
        }
        projectile = setTarget(projectile, currentTarget);

        //Set future health to prevent overkill
        float projectileDamage = GetProjectileDamage(projectile);

        IHealth enemyHealth = currentTarget.GetComponent<Enemy>();
        enemyHealth.futureHealth -= projectileDamage;
    }

    //Set target in subclass to ensure corret script is initialzed on projectile
    protected abstract GameObject setTarget(GameObject projectile, GameObject currentTarget);

    protected abstract float GetProjectileDamage(GameObject projectile);
}
