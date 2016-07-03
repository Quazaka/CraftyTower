using UnityEngine;
using System.Collections;

public class ToggleBtn : MonoBehaviour {

    // Toggle the panel on/off
    // TODO: Add a button that opens the panel - add this script
	public void TogglePanel(GameObject panel)
    {
        panel.SetActive(!panel.activeSelf);
    }
}
