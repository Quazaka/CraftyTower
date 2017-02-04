using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CraftyTower.Crafting;

public class TileSelector : MonoBehaviour {

    private Renderer rend;
    private CraftingGrid grid;

    private GameObject gridGameObj;
    private float gridheightOffset;
    private Vector3 curTileCoords;
    
    public GameObject prefab;

    void Start()
    {
        grid = GameObject.FindGameObjectWithTag("CraftingGrid").GetComponent<CraftingGrid>();
        gridGameObj = grid.gameObject;
        curTileCoords.y = gridGameObj.transform.position.y; // height offset for the selector
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (grid != null) {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Mouse position on screen
            RaycastHit hitInfo; // Where in the grid we're pointing - what tile

            // if we are mousing over the grid
            if (gridGameObj.GetComponent<Collider>().Raycast(ray, out hitInfo, Mathf.Infinity))
            {
                MoveSelector(hitInfo); // Updating position of the selector               

                if (Input.GetMouseButtonDown(1))
                {
                    DestroyUpgrade();
                }

                if (Input.GetMouseButtonDown(2))
                {
                    UnlockTile();
                }
            }
            else if (rend.enabled) { rend.enabled = false; }
        }
	}

    private void MoveSelector(RaycastHit hitInfo)
    {
        if (!rend.enabled) { rend.enabled = true; }
           
        float x = Mathf.FloorToInt(hitInfo.point.x);
        float z = Mathf.FloorToInt(hitInfo.point.z);

        curTileCoords.x = x + 0.5f; // Offset to place selector in the center of a tile.
        curTileCoords.z = z + 0.5f;        

        transform.position = curTileCoords; // Set the position
        ChangeSelectorColor(); 
    }

    // Changing color based on what tile we are mousing over
    private void ChangeSelectorColor()
    {
        switch (grid.GetTileTypeAt(transform.position))
        {
            case (int)TileType.Locked:
                rend.material.color = Color.red;
                break;
            case (int)TileType.Taken:
                rend.material.color = Color.yellow;
                break;
            case (int)TileType.Free:
                rend.material.color = Color.green;
                break;
            default:
                break;
        }
    }

    private GameObject GetUpgradeAtTile()
    {
        // All colliders in a radius from a tile
        Collider[] hitCollidersArray = Physics.OverlapSphere(transform.position, 0.5f);

        // If we find a collider with the tempScript(attached to the upgrade prefab), then return it
        for (int i = 0; i < hitCollidersArray.Length; i++)
        {
            if (hitCollidersArray[i].GetComponent<TempScript>())
            {
                return hitCollidersArray[i].gameObject;
            }
        }
        return null;
    }

    private void UnlockTile()
    {
        if (grid.GetTileTypeAt(transform.position) == (int)TileType.Locked)
        {
            grid.SetTileTypeAt(transform.position, TileType.Free);
        }
    }

    private void DestroyUpgrade()
    {
        if (grid.GetTileTypeAt(transform.position) == (int)TileType.Taken)
        {
            GameObject upgrade = GetUpgradeAtTile();
            if (upgrade != null)
            {
                Destroy(upgrade);
                grid.SetTileTypeAt(transform.position, TileType.Free);
            }
        }
    }

    public bool PlaceUpgrade()
    {
        if (grid.GetTileTypeAt(transform.position) == (int)TileType.Free)
        {       
            Instantiate(prefab, transform.position, Quaternion.identity);
            grid.SetTileTypeAt(transform.position, TileType.Taken);
            Debug.Log("Placed upgrade");
            return true;
        }

        Debug.Log("Can't place upgrade here!");
        return false;
    }
}
