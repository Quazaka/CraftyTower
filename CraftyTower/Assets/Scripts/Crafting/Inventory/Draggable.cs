using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private TileSelector selector;

    private Inventory inventory;
    private LootDrop upgrade; // the upgrade we are dragging
    private GameObject upgradeCopy; // used for dragging when stacking multiple of the same upgrade
    private Vector3 upgradeInventoryPos; // the upgrades position in the inventory UI
    private bool usingUpgradeCopy; // used to determine if we drag the copy or the original upgrade

    void Start()
    {
        selector = GameObject.FindGameObjectWithTag("Selector").GetComponent<TileSelector>();

        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
        upgrade = GetComponent<LootDrop>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // If we have more than one of this item use a copy to drag around
        if (Inventory.inventory[this.name].ItemCount > 1)
        {            
            MakeDraggableCopy();
            usingUpgradeCopy = true;            
        }
        else // if not use the original
        {
            usingUpgradeCopy = false;
            upgradeInventoryPos = transform.position;
        }
    }

    // Dragging around the copy or the original
    public void OnDrag(PointerEventData eventData)
    {
        if (usingUpgradeCopy)
        {
            upgradeCopy.transform.position = eventData.position;           
        }
        else
        {
            transform.position = eventData.position;
        }        
    }

    // We stopped dragging - Released finger from mouse
    public void OnEndDrag(PointerEventData eventData)
    {
        // We didn't place the item we dragged out - put it back in the inventory
        if (!selector.PlaceUpgrade())
        {
            if (usingUpgradeCopy)
            {                
                Destroy(upgradeCopy);
                inventory.UpdateCount(true, upgrade);
            }
            else
            {
                // Snap back to the inventory if it isn't placed
                transform.position = upgradeInventoryPos;
            }              
        }
        else // We placed the item on the grid - Destroy a copy or remove the item from inventory
        {
            if (usingUpgradeCopy)
            {
                Destroy(upgradeCopy);
            }
            else // Destroy the original
            {
                // Counts to zero and removes the item from inventory and deletes gameobject
                inventory.UpdateCount(false, upgrade);
            }
        }
    }

    // We drag a copy if we have more than one of the item in inventory
    private void MakeDraggableCopy()
    {
        upgradeCopy = new GameObject();
        // set the copys parent so we can see the image (parent is inventoryPanel)
        upgradeCopy.transform.SetParent(transform.parent);

        // add an image component and set its sprite to be the same as the item in the inventory we selected
        upgradeCopy.AddComponent(typeof(Image));
        upgradeCopy.GetComponent<Image>().sprite = this.GetComponent<Image>().sprite;

        // Count down to show that we've dragged an item out of the inventory
        inventory.UpdateCount(false, upgrade);
    }
}
