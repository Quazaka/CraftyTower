using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerCube : MonoBehaviour {

    private bool towerPlaced;

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

            towerPlaced = false;
        }

        // if not destroy the TowerCube
        else
        {
            Destroy(gameObject);
        }
    }

    // Why are each cubed stacked like shit
    protected void OnTriggerEnter(Collider co)
    {        
        if ((co.tag == "Tower" || co.tag == "TowerCube") && !towerPlaced)
        {
            towerPlaced = true;

            cubeRigid.isKinematic = true;
            Destroy(GetComponent<Rigidbody>(), 2);

            Vector3 cubePos = co.transform.position;
            cubePos.y += towerObj.transform.position.y * 2;
            this.transform.position = cubePos;
        
            Debug.Log("Placed towerCube");
        }
    }
}
