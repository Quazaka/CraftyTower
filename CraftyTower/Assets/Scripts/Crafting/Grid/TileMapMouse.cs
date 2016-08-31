using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Class to handle what happens when the mouse is move around and clicking on the grid.

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
        if (selectionCube != null && map != null ) // Ensure everything exists
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // mose position
            RaycastHit hitInfo; // store what we are poiting at

            //Set position of selection cube if it is hovering grid
            if (GetComponent<Collider>().Raycast(ray, out hitInfo, Mathf.Infinity))
            {
                MoveSelectionCube(hitInfo);
            }
            else // Deactivate object
            {
                selectionCube.gameObject.SetActive(false);
            }

            //Handle left mouse click
            if (Input.GetMouseButtonDown(0))
            {
                PlaceUpgradeAtMouse();
            }

            //Handle right mouse click
            if (Input.GetMouseButtonDown(1))
            {
                DestroyObjectAtMouse();
            }

            //Handle middle mouse click
            if (Input.GetMouseButtonDown(2))
            {
                UnlockTile();
            }
        }
    }

    private void UnlockTile()
    {
        Debug.Log("Clicking");
        if (!IsTileUnlocked())
        {
            Debug.Log("X: " + Mathf.FloorToInt(selectionCube.position.x) + " Y: " + Mathf.FloorToInt(selectionCube.position.z) + " With value: " + 1);
            map.SetTileAt(Mathf.FloorToInt(selectionCube.position.x), Mathf.FloorToInt(selectionCube.position.z), 1);
        }
    }

    //Move cube around
    private void MoveSelectionCube(RaycastHit hitInfo)
    {
        selectionCube.gameObject.SetActive(true);

        int x = Mathf.FloorToInt(hitInfo.point.x);
        int z = Mathf.FloorToInt(hitInfo.point.z);
        float xx = (float)(x + 0.5f);
        float zz = (float)(z + 0.5f);

        currentTileCoord.x = xx;
        currentTileCoord.z = zz;
        currentTileCoord.y = 6.51f; //Offset to ensure selection cube is placed on top

        selectionCube.transform.position = currentTileCoord; // Set the position
    }

    //Try to place upgrade at mouse position on the grid
    private void PlaceUpgradeAtMouse()
    {
        if (IsCubeActivated())
        {
            if (!IsTileTaken(selectionCube.transform.position) && IsTileUnlocked()) //Is tile not taken and tile avalible then place object
            {
                GameObject upgradePrefab = (GameObject)Instantiate(prefab, selectionCube.transform.position, Quaternion.identity);
                Debug.Log("Placing upgrade");
            }
            else
            {
                Debug.Log("Cannot place upgrade here");
            }
        }
    }

    //Try to remove gameobject under mouse, if the mouse is hovering the grid
    private void DestroyObjectAtMouse()
    {
        if (IsCubeActivated())
        {
            if (IsTileTaken(selectionCube.transform.position) && IsCubeOnGrid())
            {
                GameObject target = GetSelectedObject(selectionCube.transform.position);
                if (target != null)
                {
                    Destroy(target);
                }
            }
        }
    }

    #region Help Methods
    //Retrun gameobject state (enabled / disabled)
    private bool IsCubeActivated()
    {
        return selectionCube.gameObject.activeSelf;
    }

    //Ensure selection cube is whitin grid
    private bool IsCubeOnGrid()
    {
        if(selectionCube.position.z <= map.length && selectionCube.position.x <= map.width)
        {
            return true;
        }
        return false;
    }

    //Check to see that the selected tile is of the correct type
    private bool IsTileUnlocked()
    {
        if(map.GetTileAt(Mathf.FloorToInt(selectionCube.transform.position.x), Mathf.FloorToInt(selectionCube.transform.position.z)) == 1)
        {
            return true;
        }
        return false;
    }

    //Check to see if the tile at the mouse is taken by another object.
    private bool IsTileTaken(Vector3 center)
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
            //If the object is of the correct type return true
            if (hitCollidersList[i].GetComponent<TempScript>())
            {
                return true;
            }
        }
        return false;
    }


    //Get objects on grid on mouse/selection cube position
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
        //TODO return the object
        return null;
    }
    #endregion
}
