using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(TileMap))]
public class TileMapMouse : MonoBehaviour {
	
	TileMap _tileMap;
	Vector3 currentTileCoord;
	public Transform selectionCube;
    public GameObject prefab;
    DTileMap map;
	
	void Start() {

        StartCoroutine(CO_StartWithDealy());
	}

    IEnumerator CO_StartWithDealy()
    {
        yield return new WaitForSeconds(2);
        _tileMap = GetComponent<TileMap>();
        map = _tileMap.map; // get tile map
    }

	// Update is called once per frame
	void Update () {
        if (selectionCube != null && map != null )
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (GetComponent<Collider>().Raycast(ray, out hitInfo, Mathf.Infinity))
            {
                int x = Mathf.FloorToInt(hitInfo.point.x);
                int z = Mathf.FloorToInt(hitInfo.point.z);

                float xx = (float)(x + 0.5f);
                float zz = (float)(z + 0.5f);

                currentTileCoord.x = xx;
                currentTileCoord.z = zz;
                currentTileCoord.y = 6.51f;

                selectionCube.transform.position = currentTileCoord * _tileMap.tileSize;
            }
            else
            {
                selectionCube.transform.position = Vector3.zero;
            }

            if (Input.GetMouseButtonDown(0))
            {

                if (!IsQuadTaken(selectionCube.transform.position) && map.GetTileAt(Mathf.FloorToInt(selectionCube.transform.position.z), Mathf.FloorToInt(selectionCube.transform.position.x)) == 1) //Is quad not taken then place object
                {
                    GameObject projectile = (GameObject)Instantiate(prefab, selectionCube.transform.position, Quaternion.identity); //create projectile
                    Debug.Log("Placing upgrade");
                }
                else
                {
                    Debug.Log("Cannot place upgrade here");
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                GameObject target = GetSelectedObject(selectionCube.transform.position);
                if (target != null)
                {
                    Debug.Log("Removing upgrade");
                    Destroy(target);
                }
            }
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

    private GameObject GetSelectedObject(Vector3 center)
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
                return hitCollidersList[i] ;
            }
        }
        //Return list
        return null;
    }
}
