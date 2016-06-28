using UnityEngine;
using System.Collections.Generic;

public class CannonAOE : MonoBehaviour {

    public List<GameObject> potentialTargets = new List<GameObject>();

    void OnTriggerEnter(Collider co)
    {
        if (co.gameObject.tag == "Enemy")
        {
            potentialTargets.Add(co.gameObject);
        }
    }
    void OnTriggerExit(Collider co)
    {
        potentialTargets.Remove(co.gameObject);
    }

    public void DealAOEDamage(float radius, float damage) //TODO make this deal damage
    {
        Debug.Log("DealAOEDamage");
        SphereCollider sc = GetComponent<SphereCollider>();
        sc.radius = radius;

        foreach (GameObject enemy in potentialTargets)
        {
            IHealth enemyHealth = enemy.GetComponent<Enemy>();
            enemyHealth.futureHealth -= damage;
        }

        Destroy(gameObject);
    }
}
