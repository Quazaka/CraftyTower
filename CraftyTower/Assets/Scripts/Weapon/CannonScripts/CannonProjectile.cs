using UnityEngine;

public class CannonProjectile : BaseProjectile
{
    //Interfaces
    protected IDamage enemy;

    //Splash radius
    private float AOERadius = 2.0f;

    public override float damage
    {
        get { return 5.0f; }
    }

    public override float speed
    {
        get { return 5.0f; }
    }

    //When cannon projectile collide with enemy
    protected override void OnTriggerEnter(Collider co)
    {
        if (co.GetComponent<BaseEnemy>())
        {
            DealAOEDamage2(AOERadius, transform.position);
        }
    }

    //Instantiate AOEDamagePrefab with AOEDamage collider
    private void DealAOEDamage2(float radius, Vector3 center)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].GetComponent<BaseEnemy>())
            {
                if (hitColliders[i] != null)
                {
                    enemy = (IDamage)hitColliders[i].GetComponent<BaseEnemy>();
                    enemy.damage = damage;
                    Destroy(gameObject);
                }
            }
            i++;
        }
        Destroy(gameObject);
    }
}
