using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {
    //Projectile
    public GameObject projectilePrefab;

    void Start()
    {
        Debug.Log("Weapon created");
    }

    // Update is called once per frame
    void Update () {
	
	}

    void OnTriggerEnter(Collider co)
    {
        Debug.Log("Enemy trigger");
        if (co.GetComponent<Enemy>())
        {
            GameObject g = (GameObject)Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            g.GetComponent<Projectile>().target = co.transform;
        }
    }
}
