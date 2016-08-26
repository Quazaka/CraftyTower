using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour {


    // TODO: Have this handle only mainMenu and closebutton (back to mainMenu) setup
    // TODO: Have a script for each specific menu

    private GameObject activeMenu;
    private GameObject pauseMenu;
    private GameObject mainMenu;

    private bool isPaused;

    private Text currentHoverText;

    // Use this for initialization
    void Start ()
    {
        foreach (Canvas c in GetComponentsInChildren<Canvas>(true))
        {
            if (c.name == "MainMenu")
            {
                mainMenu = c.gameObject;
            }
            if (c.name == "PauseMenu")
            {
                pauseMenu = c.gameObject;
            }
        }
        ChangeMenu(mainMenu);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (currentHoverText != null && currentHoverText.enabled)
        {
            currentHoverText.transform.Rotate(Vector3.left, 40 * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!isPaused)
            {
                ChangeMenu(pauseMenu);
            }
            else
            {
                ChangeMenu(mainMenu);
            }
        }

        if (isPaused)
        {
            if (Input.GetKeyDown(KeyCode.Escape)) { Quit(); }
        }
    }

    // Change the menu by passing its gameobject (canvas)
    public void ChangeMenu(GameObject nextMenu)
    {
        if (nextMenu != null)
        {
            //disable the current active menu before assigning new one
            if (activeMenu != null)
            {
                // making sure the hoverText is disabled when we change menus from the mainMenu
                if (activeMenu == mainMenu)
                {
                    DisableHoverText();
                }

                // If we are going to the pause menu(pressing 'P' on any menu or pause button)
                // or to the main menu FROM the pausemenu
                if (nextMenu == pauseMenu || activeMenu == pauseMenu && nextMenu == mainMenu)
                {
                    ChangePauseState();
                }
                activeMenu.SetActive(false);
            }
                        
            activeMenu = nextMenu;
            Debug.Log(activeMenu.name + " is active");
            activeMenu.SetActive(true);
        }     
    }

    // Function to pause/unpause the game
    private void ChangePauseState()
    {
        // If paused at call - unpause
        if (isPaused)
        {
            isPaused = false;
            Time.timeScale = 1;
        }
        else
        {
            isPaused = true;
            Time.timeScale = 0;
        }
    }

    // toggle the specified Text object active or not
    public void ToggleHoverText(Text hoverText)
    {
        currentHoverText = hoverText;
        if (currentHoverText != null)
        {
            currentHoverText.enabled = !currentHoverText.enabled;
            if (!currentHoverText.enabled)
            {
                currentHoverText.transform.rotation = Quaternion.identity;
            }
        }
    }

    private void DisableHoverText()
    {
        if ( currentHoverText != null)
        {
            currentHoverText.enabled = false;
            currentHoverText.transform.rotation = Quaternion.identity;
        }        
    }

    public void Quit()
    {
        Debug.Log("we quit");
        Application.Quit();
    }

    public void LoadScene(int scene)
    {
        Debug.Log("Changing scene");
        SceneManager.LoadScene(scene);
    }
}
