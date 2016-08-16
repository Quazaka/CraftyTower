using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public class LightningWeapon : BaseWeapon, IDamage {
    private float _damage = 1; //private constanst as damage - in other weapon classes retrived from projectile
    private int jumpCount = 2; //Number of jumps
    private float jumpDistance = 10; //Jump Distance
    private List<GameObject> IHaveBeenHit = new List<GameObject>(); //List to store enemies already hit by this chain jump
    private List<GameObject> jumpEnemyList = new List<GameObject>(); //List to store enemies that can be jumped to
    private float lifetime = 0.1f;

    public override float cooldown
    {
        get { return 1f; }
    }

    public float damage
    {
        set{ damage = _damage; }
    }

    public override float range
    {
        get { return 10; }
    }

    protected override float GetProjectileDamage(GameObject projectile)
    {
        return _damage;
    }

    protected override GameObject setTarget(GameObject projectile, GameObject currentTarget)
    {
        Debug.Log("This is an error - Check LightningWeapon.cs setTarget should not be called");
        throw new NotImplementedException(); // Not to be implemented, if called it's a mistake
    }

    //Override shoot
    protected override void Shoot(GameObject currentTarget)
    {
        Debug.Log("Shooting with lightnings");
        //Remove null targets from enemyList
        RemoveNullObjectFromList(enemyList);

        //Instanciate first lightning arc
        LightningAttack(currentTarget, jumpCount);

    }
    void Awake() // This is used instead of Start(), as start runs in the baseclass.
    {
        targetSwitch = 1; //Closest target
    }

    private void LightningAttack(GameObject target, int noJumps)
    {
        List<GameObject> targetList = new List<GameObject>();
        targetList.Add(target);
        IHaveBeenHit.Add(target);

        //Create list with targets to deal damge and draw arch to.
        for (int i = 0; i < noJumps; i++)
        {
            target = SetNextTarget(target); // Find next target in range of Target
            if(target == null) { break; }
            targetList.Add(target);
            IHaveBeenHit.Add(target);
        }
        CreateLightningArc(targetList); // Create a lightningArc effect from previous target to the next target and apply damage.
        IHaveBeenHit.Clear(); //Clear IHaveBeenHit so the next chain can use it.

    }

    private void CreateLightningArc(List<GameObject> targetList)
    {
        DrawArc(targetList); // Draw arch
        foreach (GameObject target in targetList)
        {
            DealDamage(target); // Deal damage
        }
    }

    private void DrawArc(List<GameObject> targetList)
    {
        LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
        int vertexCount = targetList.Count() + 1;

        lineRenderer.GetComponent<LineRenderer>().enabled = true; //Enable line renderer
        lineRenderer.GetComponent<LineRenderer>().SetWidth(0.05f, 0.05f); // set width of lightning
        lineRenderer.GetComponent<LineRenderer>().SetVertexCount(vertexCount); //How many parts to devide the line into
        lineRenderer.GetComponent<LineRenderer>().SetPosition(0, gameObject.transform.position); //position of the first vortex

        int i = 1;
        foreach (GameObject target in targetList)
        {
            //var pos = Vector3.Lerp(previousTarget.transform.position, target.transform.position, i / ((float)vertexCount));
            lineRenderer.GetComponent<LineRenderer>().SetPosition(i, target.transform.position);
            i++;
        }

        Co_DisableLineRendereAfterDelay(lineRenderer, lifetime, vertexCount);
    }

    private void DealDamage(GameObject Target)
    {
        //Set future health to prevent overkill
        IHealth enemyHealth = (IHealth)Target.GetComponent<BaseEnemy>();
        enemyHealth.futureHealth -= _damage;

        //Damage target instantly
        IDamage enemyDealDamage = (IDamage)Target.GetComponent<BaseEnemy>();
        enemyDealDamage.damage = _damage;
    }

   
    private GameObject SetNextTarget(GameObject originObj)
    {
        jumpEnemyList = base.GetEnemisInRange(originObj.transform.position, jumpDistance); // Get all enemies in range.
        jumpEnemyList.RemoveAll(i => IHaveBeenHit.Contains(i)); // Remove enemies that have been hit

        //Targeting script
        Targeting scriptTargeting = GetComponent<Targeting>();
        currentTarget = scriptTargeting.ChooseTargetScanType(jumpEnemyList, targetSwitch);
        return currentTarget;
    }

    IEnumerator Co_DisableLineRendereAfterDelay(LineRenderer linerenderer, float lifetime, int vertexCount)
    {
        for (int i = 0; i < vertexCount; i++)
        {
            linerenderer.GetComponent<LineRenderer>().SetPosition(i, Vector3.zero);
        }
        
        yield return new WaitForSeconds(lifetime);
    }
}

