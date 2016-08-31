using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {

    public Dictionary<string, LootDrop> inventory;

    private LootDrop currentItem;

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
            if (inventory.TryGetValue(currentItem.name, out result))
            {
                Destroy(currentItem.gameObject);
                currentItem = result;
                UpdateCount(true);
                Debug.Log("Already got " + currentItem.name + " in inventory - count increased");
            }
            else // if not - add it
            {
                AddItemToInventory();
                Debug.Log(currentItem.name + " was not in inventory - it was added");
            }
        }        
    }

    // All the keys remain intact but the values are all null because the GameObject is destroyed when looted
    // The inventory should consist of GameObjects created in this function, (Find some way to do it).
    private void AddItemToInventory()
    {
        currentItem.transform.SetParent(transform, false);
        currentItem.GetComponent<Image>().sprite = currentItem.ItemSprite;

        inventory.Add(currentItem.name, currentItem);
        UpdateCount(true);
    }

    // We can't use lootedItem as a reference - because its original GO is destroyed on loot.
    // TODO: Fixing the dictionary and the AddItemToInventory should fix this too
    public void UpdateCount(bool increase)
    {
        if (increase)
        {
            inventory[currentItem.name].ItemCount++;
        }
        else
        {
            inventory[currentItem.name].ItemCount--;
        }

        // Do something depending on how many of the item we have in the inventory
        switch (inventory[currentItem.name].ItemCount)
        {
            case 0: // We used the last item from the inventory - remove it from inventory
                inventory.Remove(currentItem.name);
                //Destroy(currentItem.gameObject);
                break;
            case 1: // we only have one item - dont write it (defaults to this when looting an item not currently in inventory)
                currentItem.GetComponentInChildren<Text>().text = "";
                break;
            default: // show the number of items we have (we have at least one of the looted item in inventory already)
                currentItem.GetComponentInChildren<Text>().text = inventory[currentItem.name].ItemCount.ToString();
                break;
        }
    }

    // The lootDrop script delegates to this method on its MouseUp method
    private void LootItem(LootDrop item)
    {  
        currentItem = item;
        Debug.Log("Trying to loot: " + currentItem);
        UpdateInventory();
    }
}
