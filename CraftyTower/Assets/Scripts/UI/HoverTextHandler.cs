using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;

public class HoverTextHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    private Button currentButton;
    private Text currentHoverText;
    private GameObject lastHovered;

    void Start()
    {

    }

    void Update()
    {
        if (currentHoverText != null && currentHoverText.enabled)
        {
            currentHoverText.transform.Rotate(Vector3.left, 40 * Time.deltaTime);
        }
    }

    // OnClick event to be fired from buttons that use hover text
    public void DisableHoverText()
    {
        currentHoverText.enabled = false;
        currentHoverText.transform.rotation = Quaternion.identity;
    }

    private void ToggleHoverText()
    {
        if (currentHoverText != null)
        {
            currentHoverText.enabled = !currentHoverText.enabled;
            if (!currentHoverText.enabled)
            {
                currentHoverText.transform.rotation = Quaternion.identity;
            }
        }            
    }

    // When the mouse if over one the main buttons
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("ENTER");
        if (eventData.pointerEnter.name == "ButtonText")
        {
            // If we hover the text on the button, find its parent's name(this is the name of the button)
            currentButton = eventData.pointerEnter.GetComponentInParent<Button>();       
        }
        else
        {
            //Debug.Log("currenthover: " + eventData.pointerEnter.name + " lasthover: " + lastHovered.name);
            currentButton = eventData.pointerEnter.GetComponent<Button>();
        }
        lastHovered = currentButton.gameObject;
        currentHoverText = currentButton.GetComponentsInChildren<Text>().Where(btn => btn.CompareTag("HoverText")).SingleOrDefault();
        ToggleHoverText();
        //Debug.Log(currentHoverText);        
    }

    // When mouse exists one of the main buttons
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("EXIT");
        ToggleHoverText();
    }
}
