using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;
using System;

public class MenuHandler : MonoBehaviour {


    // TODO: Have this handle only mainMenu and closebutton (back to mainMenu) setup
    // TODO: Have a script for each specific menu

    private GameObject activeMenu;
    [SerializeField]
    private PauseMenuScript pauseMenu;

    // Use this for initialization
    void Start ()
    {
        foreach (Canvas c in GetComponentsInChildren<Canvas>(true))
        {
            if (c.name == "MainMenu")
            {
                ChangeMenu(c.gameObject);
                break;
            }
        }
	}
	
	// Update is called once per frame
	void Update ()
    {

    }

    public void ChangeMenu(GameObject currentMenu)
    {
        if (currentMenu != null)
        {
            //disable the current active menu before assigning new one
            if (activeMenu != null && activeMenu.name != "PauseMenu")
            {
                activeMenu.SetActive(false);
            }            
            activeMenu = currentMenu;
            Debug.Log(activeMenu.name + " is active");
            activeMenu.SetActive(true);
        }     
    }
}
