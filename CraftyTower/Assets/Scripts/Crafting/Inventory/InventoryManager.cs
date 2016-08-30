using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {

    public Dictionary<string, LootDrop> inventory;

    private LootDrop lootedItem;
    private GameObject itemInInventory;

    // Use this for initialization
    void Start ()
    {
        inventory = new Dictionary<string, LootDrop>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // Have the LootDrop subscribe to the event 
    void OnEnable()
    {
        LootDrop.OnLoot += LootItem;
    }

    void OnDisable()
    {
        LootDrop.OnLoot -= LootItem;
    }

    // TODO: This doesn't work as intended right now - adding items of a new type fucks shit up
    public void UpdateInventory()
    {
        // Inventory is empty - just add the item
        if (inventory.Count == 0)
        {            
            AddItemToInventory();
        }
        else // We have at least one item in the inventory
        {
            LootDrop result = null;

            // if the item we click on is already in the inventory - increase its amount
            if (inventory.TryGetValue(lootedItem.name, out result))
            {
                  itemInInventory = lootedItem.gameObject;
                  UpdateCount(true);
                  Debug.Log("Already got " + lootedItem.name + " in inventory - count increased");
            }
            else // if not - add it
            {
                AddItemToInventory();
                Debug.Log(lootedItem.name + " was not in inventory - it was added");
            }
        }        
    }

    // All the keys remain intact but the values are all null because the GameObject is destroyed when looted
    // The inventory should consist of GameObjects created in this function, (Find some way to do it).
    private void AddItemToInventory()
    {
        LootDrop test = Instantiate(lootedItem.inventoryItemPrefab).GetComponent<LootDrop>();
        itemInInventory = Instantiate(lootedItem.inventoryItemPrefab);
        itemInInventory.name = lootedItem.name;
        itemInInventory.transform.SetParent(transform, false);
        itemInInventory.GetComponent<Image>().sprite = lootedItem.ItemSprite;

        inventory.Add(lootedItem.name, lootedItem);
        UpdateCount(true);
    }

    // We can't use lootedItem as a reference - because its original GO is destroyed on loot.
    // TODO: Fixing the dictionary and the AddItemToInventory should fix this too
    public void UpdateCount(bool increase)
    {
        if (increase)
        {
            inventory[lootedItem.name].ItemCount++;
        }
        else
        {
            inventory[lootedItem.name].ItemCount--;
        }

        // Do something depending on how many of the item we have in the inventory
        switch (inventory[lootedItem.name].ItemCount)
        {
            case 0: // We used the last item from the inventory - remove it from inventory
                inventory.Remove(lootedItem.name);
                break;
            case 1: // we only have one item - dont write it (defaults to this when looting an item not currently in inventory)
                itemInInventory.GetComponentInChildren<Text>().text = "";
                break;
            default: // show the number of items we have (we have at least one of the looted item in inventory already)
                itemInInventory.GetComponentInChildren<Text>().text = inventory[lootedItem.name].ItemCount.ToString();
                break;
        }
    }

    // The lootDrop script delegates to this method on its MouseUp method
    private void LootItem(LootDrop item)
    {  
        lootedItem = item;
        Debug.Log("Trying to loot: " + lootedItem);
        UpdateInventory();
    }
}
