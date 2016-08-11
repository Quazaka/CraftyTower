using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class LightningWeapon : BaseWeapon, IDamage {
    private float _damage = 1; //private constanst as damage - in other weapon classes retrived from projectile
    private int jumpCount = 2;
    private float jumpDistance = 2;
    [SerializeField]
    private GameObject lightningRendererPrefab;

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
        GameObject lightningArc = (GameObject)Instantiate(lightningRendererPrefab, transform.position, Quaternion.identity); //create first "chain" in lightning jump
        lightningArc.GetComponent<LightningArc>().Instantiate(jumpCount,currentTarget, _damage, jumpDistance, transform.position, true, null);
    }
    void Awake() // This is used instead of Start(), as start is run in the baseclass.
    {
        targetSwitch = 2; //Random target
    }

    // Update is called once per frame
    void Update () {

	}
}
