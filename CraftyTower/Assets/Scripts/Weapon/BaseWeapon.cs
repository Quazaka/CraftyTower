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
        //TODO Consider - Change from sphear collider to OverlapSphere for better target handling
        //Set range on weapon/sphear collider
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
