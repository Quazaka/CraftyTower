using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerCube : MonoBehaviour {

    private bool stop = false;
    private float moveSpeed = 1f;
    private Vector3 towerPos;
    private GameObject towerObj;
    private GameObject selectedWeapon;
    private List<GameObject> weapons = new List<GameObject>(); //A list of all the weapons



    public void SetWeaponType(GameObject weapon)
    {
        selectedWeapon = weapon;
    }

    // Use this for initialization
    void Start () {


        var wepPrefabs = Resources.LoadAll("Prefabs/Weapons");

        foreach (GameObject w in wepPrefabs)
        {
            if (w.tag == "TowerCube")
            {
                // We only need the GameObjects that are tagged as weapon
                weapons.Add(w);
            }
        }

        towerObj = GameObject.FindGameObjectWithTag("Tower");
        // If it exist make sure the Cube got the tower base as target.
        if (towerObj != null)
        {
            towerPos = towerObj.transform.position;
            towerPos.y = towerPos.y + 0.49f; // Ensure it lands ontop of the tower
        }

        // if not destroy the TowerCube
        else
        {
            Destroy(gameObject);
        }
    }
	

    protected void FixedUpdate()
    {
        // if we haven't reached tower - keep moving
        if (!stop)
        {
            Move();
        }
    }

    protected void OnTriggerEnter(Collider co) //Why am i not getting called?
    {
        Debug.Log("Triggered");
        if (co.tag == "Tower" || co.tag == "TowerCube")
        {
            stop = true;
            CreateWeapon();
        }
    }

    // Move the creep towards the tower in real time
    protected virtual void Move()
    {
        moveSpeed = moveSpeed + 0.20f;
        transform.position = Vector3.MoveTowards(transform.position, towerPos, moveSpeed * Time.deltaTime);
    }

    private void CreateWeapon()
    {
        GameObject projectile = (GameObject)Instantiate(weapons[1], transform.position, Quaternion.identity); //create projectile
    }
}
