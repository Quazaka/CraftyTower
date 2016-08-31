using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    GameObject itemCopy;
    InventoryManager iMan;

    private Vector3 itemPos;
    private bool copyWasPlaced;

    void Start()
    {
        iMan = GameObject.FindGameObjectWithTag("Inventory").GetComponentInChildren<InventoryManager>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        MakeDraggableCopy();

        itemPos = Camera.main.WorldToScreenPoint(itemCopy.transform.position);
    }

    // this is a loop
    public void OnDrag(PointerEventData eventData)
    {
        Vector3 newScreenPoint = new Vector3(eventData.position.x, eventData.position.y, itemPos.z);
        itemCopy.transform.position = Camera.main.ScreenToWorldPoint(newScreenPoint);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // TODO: Have this bool be changed whenever we take an item from inventory and place it in the grid
        // We didn't place the item we dragged out - put it back in the inventory
        if (!copyWasPlaced)
        {
            iMan.UpdateCount(true);           
        }
        else // We placed the item on the grid, remove it from inventory
        {
            iMan.UpdateCount(false);
        }
        Destroy(itemCopy);
    }

    // When we start dragging the item in the inventory,
    // we create a copy of it, add an image component and set it to be the same as the item we're dragging
    // We update the count of the item (false = -1)
    private void MakeDraggableCopy()
    {
        itemCopy = new GameObject();
        // set the copys parent so we can see the image (parent is inventoryPanel)
        //itemCopy.transform.parent = this.transform.parent;

        // add an image component and set its sprite to be the same as the item in the inventory we selected
        itemCopy.AddComponent(typeof(SpriteRenderer));
        itemCopy.GetComponent<SpriteRenderer>().sprite = this.GetComponent<Image>().sprite;

        iMan.UpdateCount(false);        
    }
}
