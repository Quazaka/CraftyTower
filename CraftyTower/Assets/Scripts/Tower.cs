using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Tower : MonoBehaviour, IDamage {

    // TODO: Finish healthbar and add a script to show it

    // When dropdown is working this should be removed
    public GameObject arrowWeaponPrefab;
    public GameObject cannonWeaponPrefab;
    
    //Weapon creation handling:
    private GameObject SelectedWeapon; // GameObj of the option selected in the dropdown
    public List<string> AvaiableWeapons = new List<string>(); // name of weapons (prefab names)
    public Dropdown myDropdown; //might not be necessary

    public float health = 100;

	// Use this for initialization
	void Start ()
    {
        /**
        myDropdown.ClearOptions();

        // Load all prefabs in Weapons folder, but only add the weapon prefabs (not projectile etc.)
        var wepPrefabs = Resources.LoadAll("Prefabs/Weapons"); // When using this the Path must be in folder 'Resources'

        foreach (GameObject wep in wepPrefabs)
        {
            // only add gameobject if it is tagged with 'Weapon'
            if (wep.tag == "Weapon")
            {
                AvaiableWeapons.Add(wep.name);
            }
        }
        myDropdown.AddOptions(AvaiableWeapons);

        // TODO: Fix this delegate such that it works
        // TODO: Find another way to get the selected option from the dropdown menu
        myDropdown.onValueChanged.AddListener(delegate {
            SelectWeapon(myDropdown);
        });
        **/
    }
	


	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown("space"))
        {
            Instantiate(cannonWeaponPrefab, transform.position, Quaternion.identity);
            Debug.Log("Space pressed");
        }
	}

    #region Weapon Handling
    // Weapon instantiate
    public void CreateWeapon()
    {
        Instantiate(SelectedWeapon, transform.position, Quaternion.identity);
        Debug.Log("Created Arrow Weapon");
    }

    // Get the GameObj of the selected option in the dropdown
    // TODO: Fix this, either wrong way to do it or wrong values retrieved
    public void SelectWeapon(Dropdown WeaponDropDown)
    {
        Debug.Log("Selected " + WeaponDropDown.GetComponent<GUIText>());
        SelectedWeapon = GetComponent(WeaponDropDown.name).gameObject;
    }
    #endregion

    // From Damage interface
    public float damage
    {
        set { TakeDamage(value); }
    }

    // Take damage from enemies
    private void TakeDamage(float damage)
    {
        health -= damage;

        //Deactiavte towerobject if health is below zero
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
