using UnityEngine;
using System.Collections;

public class ParticleLulz : MonoBehaviour {

    public ParticleSystem exp;

	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Explode()
    {
        exp.Play();
    }
}
