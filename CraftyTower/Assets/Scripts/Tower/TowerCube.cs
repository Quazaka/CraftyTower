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

    // Why are each cubed stacked like shit
    protected void OnTriggerEnter(Collider co)
    {        
        if ((co.tag == "Tower") && !towerCubePlaced)
        {
            towerCubePlaced = true;

            cubeRigid.isKinematic = true;
            Destroy(cubeRigid);

            Vector3 cubePos = co.transform.position;
            cubePos.y += this.transform.localScale.y;
            this.transform.position = cubePos;
            this.tag = "Tower";
            Debug.Log("Placed towerCube");
        }
    }
}
