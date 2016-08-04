using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseMenuScript : MonoBehaviour {

    [SerializeField]
    private GameObject pausePanel;
    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private Button pauseButton;

    private bool isPaused;
    [SerializeField]
    private MenuHandler Menu;


    // TODO: MOVE TO MENUHANDLER
	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (Input.GetKeyDown(KeyCode.P))
        {
            ChangePauseState();            
            if (isPaused)
            {
                Menu.ChangeMenu(pausePanel);                
            }
            else
            {
                Menu.ChangeMenu(mainMenu);
            }
        }

        if (isPaused)
        {
            if (Input.GetKeyDown(KeyCode.Escape)) { Quit(); }
        }
	}

    public void ChangePauseState()
    {
        // If paused at call - unpause
        if (isPaused)
        {
            isPaused = false;
            Time.timeScale = 1;
            pauseButton.gameObject.SetActive(true);
        }
        else
        {
            isPaused = true;
            Time.timeScale = 0;
            pauseButton.gameObject.SetActive(false);
        }
    }

    public void Quit()
    {
        Debug.Log("we quit");
        Application.Quit();
    }


}
