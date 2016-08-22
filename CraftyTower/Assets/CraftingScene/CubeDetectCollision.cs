using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CubeDetectCollision : MonoBehaviour {

    private Renderer rend;

    void Start()
    {
        rend = gameObject.GetComponent<Renderer>();
    }

	// Update is called once per frame
	void Update () {
        if (IsQuadTaken(transform.position))
        {
            rend.material.color = Color.red;
        } else
        {
            rend.material.color = Color.green;
        }
	}

    private bool IsQuadTaken(Vector3 center)
    {
        //OverlapSphere returns an array - converted to list here
        Collider[] hitCollidersArray = Physics.OverlapSphere(center, 0.5f);
        List<GameObject> hitCollidersList = new List<GameObject>();

        //"Convert" the Array<Collider> to List<GameObject>
        for (int i = 0; i < hitCollidersArray.Length; i++)
        {
            hitCollidersList.Add(hitCollidersArray[i].gameObject);
        }

        //Remove non enemies from list
        for (int i = hitCollidersList.Count - 1; i >= 0; i--)
        {
            if (hitCollidersList[i].GetComponent<TempScript>())
            {
                return true;

            }
        }
        //Return list
        return false;
    }
}
