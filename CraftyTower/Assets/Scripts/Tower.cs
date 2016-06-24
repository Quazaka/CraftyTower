using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour {
    public GameObject weaponPrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown("space"))
        {
            GameObject wep = Instantiate(weaponPrefab, transform.position, Quaternion.identity) as GameObject;
            Debug.Log("Space pressed");
        }
	}
}
