using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class WeaponChooser : MonoBehaviour
{
    // Script for the DropDown menu when choosing weapon to spawn
    // TODO: Make it possible to create a weapon from the chosen option
    // TODO: Make it such that list is only filled with weapons possible to create (e.g. some are unlocked after clearing wave XX)
    // TODO: Update list when new weapons are unlocked.

    public Dropdown dropDown;
    public List<string> AvaiableWeapons = new List<string>();

    // Use this for initialization
    void Start()
    {
        dropDown.ClearOptions();

        // Load all prefabs in Weapons folder, but only add the weapon prefabs (not projectile etc.)
        var wepPrefabs = Resources.LoadAll("Prefabs/Weapons");

        foreach (GameObject wep in wepPrefabs)
        {
            if (wep.tag == "Weapon")
            {
                AvaiableWeapons.Add(wep.name);
            }
        }
        dropDown.AddOptions(AvaiableWeapons);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
