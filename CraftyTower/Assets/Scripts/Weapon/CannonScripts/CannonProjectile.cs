using UnityEngine;

public class CannonProjectile : BaseProjectile
{
    //Interfaces
    protected IDamage dealDamageToEnemy;

    //Splash radius
    private float _SplashRadius;

    private float _damage;
    private float _splashDamage;

    public override float Damage
    {
        get { return _damage; }
        set { _damage = value; }

    }

    public override float Speed
    {
        get { return 5.0f; }
    }

    public float SplashRadius
    {
        get { return _SplashRadius; }
        set { _SplashRadius = value; }
    }

    public float SplashDamage
    {
        get { return _splashDamage; }
        set { _splashDamage = value; }
    }

    //When cannon projectile collide with enemy
    protected override void OnTriggerEnter(Collider co)
    {
        if (co.GetComponent<BaseEnemy>())
        {
            DealAOEDamage(_SplashRadius, transform.position);
        }
    }

    //Instantiate AOEDamagePrefab with AOEDamage collider
    private void DealAOEDamage(float radius, Vector3 center)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].GetComponent<BaseEnemy>())
            {
                if (hitColliders[i] != null)
                {
                    dealDamageToEnemy = (IDamage)hitColliders[i].GetComponent<BaseEnemy>();
                    dealDamageToEnemy.damage = _damage;
                    Destroy(gameObject);
                }
            }
            i++;
        }
        Destroy(gameObject);
    }
}
