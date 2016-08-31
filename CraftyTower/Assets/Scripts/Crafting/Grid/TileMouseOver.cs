using UnityEngine;
using System.Collections;

//Update and move + set the color of our highlight cube
public class TileMouseOver : MonoBehaviour {
	
	public Color highlightColor;
	Color normalColor; 
	
    //Store normal color at start
	void Start() {
		normalColor = GetComponent<Renderer>().material.color;
	}

    //Update color according to conditions
    void Update () {
		
		Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
		RaycastHit hitInfo;
		
		if( GetComponent<Collider>().Raycast( ray, out hitInfo, Mathf.Infinity ) ) {
			GetComponent<Renderer>().material.color = highlightColor;
		}
		else {
			GetComponent<Renderer>().material.color = normalColor;
		}
	}
}
