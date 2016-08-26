using UnityEngine;
using System.Collections;
using CraftyTower.Items;

public class ItemDropper : MonoBehaviour {

    public Sprite healthSprite, lightningSprite;
    public GameObject lootPrefab;


    // TODO: make this general/abstract so we can handle all types of loot drops
    // This class is just for testing looting and adding to inventory
    public void CreateHealthDrop()
    {
        LootDrop healthItem = Instantiate(lootPrefab).GetComponent<LootDrop>();
        healthItem.name = "Heart";
        healthItem.ItemSprite = healthSprite;
    }

    public void CreateLightningDrop()
    {
        LootDrop lightningItem = Instantiate(lootPrefab).GetComponent<LootDrop>();
        lightningItem.name = "Lightning";
        lightningItem.ItemSprite = lightningSprite;
    }
}
