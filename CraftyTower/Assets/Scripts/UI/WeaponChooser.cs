using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class WeaponChooser : MonoBehaviour
{
    // Script for the DropDown menu when choosing weapon to spawn
    //Weapon creation handling:
    private List<GameObject> weapons = new List<GameObject>(); //A list of all the weapons
    private GameObject selectedWeapon; // GameObj of the option selected in the dropdown
    private List<string> availableWeapons = new List<string>(); // name of weapons (prefab names) - DropDown data

    [SerializeField]
    private Dropdown weaponSelector;

    [SerializeField]
    private TowerCube towerCubePrefab;

    private Vector3 towerPos;

    // TODO: Make it such that list is only filled with weapons possible to create (e.g. some are unlocked after clearing wave XX)
    // TODO: Update list when new weapons are unlocked.

    // Use this for initialization
    void Start()
    {
        weaponSelector.ClearOptions();

        // Load all prefabs in Weapons folder, but only add the weapon prefabs (not projectile etc.)
        var wepPrefabs = Resources.LoadAll("Prefabs/Weapons");

        foreach (GameObject w in wepPrefabs)
        {
            if (w.tag == "Weapon")
            {
                // We only need the GameObjects that are tagged as weapon
                availableWeapons.Add(w.name);
                weapons.Add(w);
            }
        }
        weaponSelector.AddOptions(availableWeapons);

        // Selected weapon on start is the first weapon in the list of available weapons
        selectedWeapon = weapons[0];

        // Get the position of the tower
        towerPos = GameObject.FindGameObjectWithTag("Tower").transform.position;
        towerPos.y += 10f;
    }

    // Update is called once per frame
    // TODO: When new weapons are activated (fx. through leveling up), add them to the dropdown
    void Update()
    {

    }

    #region Weapon Selection/Creation
    // Weapon instantiate
    public void CreateWeapon()
    {       
        Transform Tc = Instantiate(towerCubePrefab.transform, towerPos, Quaternion.identity) as Transform;
        Tc.parent = GameObject.FindGameObjectWithTag("Tower").transform;

        GameObject curWep =  Instantiate(selectedWeapon, towerPos, Quaternion.identity) as GameObject;
        curWep.transform.parent = Tc;

        Debug.Log("Created " + selectedWeapon.name);
    }

    // Get the GameObj of the selected option in the dropdown
    public void SelectWeapon()
    {
        //Dropdown wepDrop
        string weaponName = weaponSelector.captionText.text;

        // Find the weapon in the list where the name match
        selectedWeapon = weapons.Find(w => w.name == weaponName);
    }
    #endregion
}
