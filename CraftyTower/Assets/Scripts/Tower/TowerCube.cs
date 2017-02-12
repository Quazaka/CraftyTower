using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerCube : MonoBehaviour {

    private bool towerCubePlaced;

    private GameObject towerObj;
    private Rigidbody cubeRigid;

    // Use this for initialization
    void Start () {        
        towerObj = GameObject.FindGameObjectWithTag("Tower");
        // If it exist make sure the Cube got the tower base as target.
        if (towerObj != null)
        {
            cubeRigid = gameObject.AddComponent<Rigidbody>() as Rigidbody;
            cubeRigid.useGravity = true;

            towerCubePlaced = false;
        }

        // if not destroy the TowerCube
        else
        {
            Destroy(gameObject);
        }
    }

    // Stack towercubes when entering trigger
    protected void OnTriggerEnter(Collider co)
    {        
        if ((co.tag == "Tower") && !towerCubePlaced)
        {            
            Destroy(cubeRigid); //Destroy the rigidbody

            Vector3 cubePos = co.transform.position;

            //Set the y position based on collider
            cubePos.y += transform.localScale.y;            
            transform.position = cubePos;
            tag = "Tower";
            towerCubePlaced = true;
            Debug.Log("Placed towerCube");
        }
    }
}
