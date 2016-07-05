using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WaveToggle : MonoBehaviour {

    private Toggle waveToggle;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnEnable()
    {
        SpawnController.toggleWave += ToggleAutomatic;
    }

    void OnDisable()
    {
        SpawnController.toggleWave -= ToggleAutomatic;
    }

    void ToggleAutomatic()
    {
        waveToggle.enabled = !waveToggle.enabled;
    }
}
