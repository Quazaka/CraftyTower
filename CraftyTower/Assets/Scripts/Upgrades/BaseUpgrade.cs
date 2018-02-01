using UnityEngine;
using CraftyTower.Upgrades;
using System.Collections.Generic;
using System.Linq;

public abstract class BaseUpgrade : MonoBehaviour {

    protected string displayName; // Display name in GUI
    protected Texture2D sprite; // sprite in inventory
    protected Rarity rarity; // upgrade rarity
    protected BaseRoom room; // Weapon Room. (Weapon type)
    protected string checksum; // Dictonary key used for stacking in inventory
    protected bool weaponTire2 = false; // tower got the extra upgrade, ex Poison Arrow.
    protected int wave = 1; // Wave is used to scale stats. //TODO: Connect to global variable
    protected GameObject prefab; // Prefab for upgrade
    protected float minRoll; //Stat Floor multiplier
    protected float maxRoll; //Stat Celing multiplier

    // Use this for initialization
    void Start () {

        ChildStart(); // Start child class
        RollRarity(); 
        SetStatRange(); 
        RollStats();
        CalcChecksum();
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
    }

    //Roll Large or Small set of stats in Child.
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

    //Decide what the range for stats rolls (min to max)
    protected void SetStatRange()
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

        float scaling = 1.05f * (wave / 3); // increase range with wave level

        minRoll = Mathf.RoundToInt((scaling * multiplyer)); 
        maxRoll = Mathf.RoundToInt((scaling * multiplyer) * 2); 

        //Prevent upgrade from not gaining an effect.
        if(maxRoll == 0 || minRoll == 0)
        {
            maxRoll = 1;
            minRoll = 1;
        }
    }

    //abstracts
    protected abstract void ChildStart();

    protected abstract void RollLargeSet();

    protected abstract void RollSmallSet();

    protected abstract void CalcChecksum();


    //Debug
    public void HitMeDebug()
    {
        rarity = Rarity.Common;
        SetStatRange();
        Debug.Log(rarity + minRoll.ToString() + "-" + maxRoll.ToString());

        rarity = Rarity.Uncommon;
        SetStatRange();
        Debug.Log(rarity + minRoll.ToString() + "-" + maxRoll.ToString());

        rarity = Rarity.Rare;
        SetStatRange();
        Debug.Log(rarity + minRoll.ToString() + "-" + maxRoll.ToString());

        rarity = Rarity.Legendary;
        SetStatRange();
        Debug.Log(rarity + minRoll.ToString() + "-" + maxRoll.ToString());
    }
}
