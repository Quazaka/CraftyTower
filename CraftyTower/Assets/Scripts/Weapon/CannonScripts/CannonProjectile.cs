using UnityEngine;

public class CannonProjectile : BaseProjectile
{
    public float SplashRadius { get; set; }
    public float SplashDamage { get; set; }

    void Start()
    {
        Speed = 5.0f;
    }

    //When cannon projectile collide with enemy
    protected override void OnTriggerEnter(Collider co)
    {
        if (co.GetComponent<BaseEnemy>())
        {
            DealAOEDamage(SplashRadius, transform.position);
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
                    targetIDamage = hitColliders[i].GetComponent<BaseEnemy>();
                    targetIDamage.TakeDamage(Damage);
                    Destroy(gameObject);
                }
            }
            i++;
        }
        Destroy(gameObject);
    }
}
