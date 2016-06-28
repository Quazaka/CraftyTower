using UnityEngine;

public class CannonProjectile : BaseProjectile
{
    //Splash radius
    float AOERadius = 0.7f;

    //AOEDmagePrefab object - Must for multiple colliders
    public GameObject AOEDamagePrefab;

    public override float damage
    {
        get { return 10.0f ; }
    }

    public override float speed
    {
        get { return 5.0f; }
    }

    //When cannon projectile collide with enemy
    protected override void OnTriggerEnter(Collider co)
    {
        if (co.GetComponent<Enemy>())
        {
            DealAOEDamage(AOERadius, damage);
        }
    }

    //Instantiate AOEDamagePrefab with AOEDamage collider
    private void DealAOEDamage(float AOERadius, float damage)
    {
        GameObject AOECollider = (GameObject)Instantiate(AOEDamagePrefab, transform.position, Quaternion.identity);
        AOECollider.transform.parent = transform;
        AOECollider.GetComponent<CannonAOE>().DealAOEDamage(AOERadius, damage);
        Destroy(gameObject);
    }
}
