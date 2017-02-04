using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    public static Dictionary<string, LootDrop> inventory = new Dictionary<string, LootDrop>();

    public void OnEnable()
    {
        LootDrop.OnLoot += LootItem;        
    }
    public void OnDisable()
    {
        LootDrop.OnLoot -= LootItem;
    }

    // The lootDrop script delegates to this method on its MouseUp method
    private void LootItem(LootDrop item)
    {
        //Debug.Log("Trying to loot: " + item);
        LootDrop result = null;

        // if the item we click on is already in the inventory - destroy it and increase its inventory amount (out result)
        if (inventory.TryGetValue(item.name, out result))
        {
            Destroy(item.gameObject);      
            item = result;           
            //Debug.Log("Already got " + item.name + " in inventory - count increased");
        }
        else // if not - add it
        {
            item.transform.SetParent(this.transform);
            inventory.Add(item.name, item);
            //Debug.Log(item.name + " was not in inventory - it was added");
        }

        UpdateCount(true, item);
        //Debug.Log("Successfully looted: " + item);
    }

    // Increase or decrease the count of an item and set its text to reflect the change
    public void UpdateCount(bool increase, LootDrop item)
    {
        if (increase)
        {
            inventory[item.name].ItemCount++;
        }
        else
        {
            inventory[item.name].ItemCount--;
        }

        // Set the count text (or remove the item) based on the count of the item in inventory
        switch (inventory[item.name].ItemCount)
        {
            case 0: // We have used the last item - remove it from inventory and delete its gameobject
                inventory.Remove(item.name);
                Destroy(item.gameObject);
                break;
            case 1: // we only have one item - dont write it (defaults to this when looting an item not currently in inventory)
                item.GetComponentInChildren<Text>().text = "";
                break;
            default: // show the number of items we have (we have at least one of the looted item in inventory already)
                item.GetComponentInChildren<Text>().text = inventory[item.name].ItemCount.ToString();
                break;
        }
    }
}
