using UnityEngine;
using System.Collections.Generic;
using CraftyTower.Upgrades;
using System.Linq;

public abstract class UpgradeBase : MonoBehaviour {

    protected string displayName; // Display name in GUI
    protected Texture2D sprite; // sprite in inventory
    protected TowerType type; // tower this upgrade fit
    protected Rarity rarity; // upgrade rarity
    protected string checksum; // Dictonary key
    protected bool weaponTire2; // tower got the extra upgrade, ex Poison Arrow.
    protected int wave; // Wave is used to scale stats.
    protected GameObject prefab;
    protected float scaling; // Scale stat range
    protected GameObject weaponObj; // reference to the weapon
    protected float minRoll; //Stat Floor
    protected float maxRoll; //Stat Celing

    // Use this for initialization
    void Start () {

        ChildStart(); // Start child class
        RollRarity();
        SetStatRange(); 
        RollStats(); 
        //HitMeDebug();
    }

    // Roll the rarity of an item 
    private void RollRarity()
    {
        List<Rarity> rarityList = Rarity.GetValues(typeof(Rarity)).Cast<Rarity>().ToList();

        float total = 0;

        foreach (float elem in rarityList)
        {
            total += (int)elem;
        }

        float randomValue = Random.value * total; // Pick random point from 0 to 100 (total)

        for (int i = 0; i < rarityList.Count; i++)
        {
            if (randomValue < (int)rarityList[i])
            {
                rarity = rarityList[i];
                return;
            }
            else
            {
                randomValue -= (int)rarityList[i];
            }
        }
        throw new System.Exception("Something went wrong when deciding rarity of upgrade");      
    }


    private void RollStats()
    {
        if (weaponTire2)
        {
            RollLargeSet();
        }
        else
        {
            RollSmallSet();
        }
    }

    //Decide what the range for stats roll (min to max)
    private void SetStatRange()
    {

        float multiplyer = 0;

        switch (rarity)
        {
            case Rarity.Common:
                multiplyer = 1;
                break;
            case Rarity.Uncommon:
                multiplyer = 1.5f;
                break;
            case Rarity.Rare:
                multiplyer = 2;
                break;
            case Rarity.Legendary:
                multiplyer = 3;
                break;
            default:
                break;
        }

        float scaling = 0.05f * wave; // increase range with wave level

        minRoll = Mathf.RoundToInt(scaling * multiplyer); 
        maxRoll = Mathf.RoundToInt((1 + 2 * scaling) * multiplyer); 
    }

    //abstracts
    protected abstract void ChildStart();

    protected abstract void RollLargeSet();

    protected abstract void RollSmallSet();

    protected abstract void CalcChecksum();


    //Debug
    public void HitMeDebug()
    {
        for (int i = 0; i < 1000; i++)
        {
            RollRarity();
            Debug.Log(rarity);
        }
    }
}
