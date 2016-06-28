using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public abstract class WeaponBase : MonoBehaviour {
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
        foreach(GameObject Target in enemies)
        {
            if (Target == null)
            {
                enemies.Remove(Target);
            }
        }
    }

    //Shoot projectile at target
    protected abstract void Shoot(GameObject currentTarget);
}
