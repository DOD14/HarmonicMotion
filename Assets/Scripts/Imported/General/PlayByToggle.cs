using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayByToggle : MonoBehaviour {

	private ParticleSystem system;
	// Use this for initialization
	void Start () {
		system = GetComponent<ParticleSystem> ();
	}
	
	// Update is called once per frame
	public void ToggleSystem()
	{
		if (!system.isPlaying)
			system.Play ();
		else if (system.isPlaying)
			system.Stop ();
	}
}
