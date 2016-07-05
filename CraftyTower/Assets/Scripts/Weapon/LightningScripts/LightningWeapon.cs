using UnityEngine;
using System.Collections;
using System;

public class LightningWeapon : BaseWeapon {
    float _damage = 1; // Hack fordi alle de andre våben er bygget med projektiler.

    public override float cooldown
    {
        get { return 0.2f; }
    }

    public override float range
    {
        get { return 3; }
    }

    protected override float GetProjectileDamage(GameObject projectile)
    {
        return _damage;
    }

    protected override GameObject setTarget(GameObject projectile, GameObject currentTarget)
    {
        throw new NotImplementedException();
    }

    //Override shoot
    protected override void Shoot(GameObject currentTarget)
    {
        //Remove null targets from enemyList
        RemoveNullObjectFromList(enemyList);

        //Create projectile and set it's target
        GameObject projectile = (GameObject)Instantiate(projectilePrefab, transform.position, Quaternion.identity); //create projectile
        projectile = setTarget(projectile, currentTarget);

        //Set future health to prevent overkill
        float projectileDamage = GetProjectileDamage(projectile);

        IHealth enemyHealth = currentTarget.GetComponent<BaseEnemy>();
        enemyHealth.futureHealth -= projectileDamage;
    }

    // Use this for initialization
    void Start () {
        targetSwitch = 2;
	}
	
	// Update is called once per frame
	void Update () {
        while (currentTarget != null)
        {
            Debug.Log("Drawing lines");
            LineRenderer lineRenderer = new LineRenderer();
            lineRenderer.SetVertexCount(2);
            lineRenderer.SetWidth(0.2F, 0.2F);
            lineRenderer.SetPosition(1, transform.position);
            lineRenderer.SetPosition(2, currentTarget.transform.position);
        }
	}
}
