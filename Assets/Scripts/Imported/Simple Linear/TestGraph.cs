using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGraph : MonoBehaviour {

	public int resolution = 100;
	public Oscillator2 oscillatorScript;
	public float deltaTime = 0.25f;

	private ParticleSystem.Particle[] points;
	private ParticleSystem particleSystem;
	private int i = 0;
	private float lastTime;

	void Start () {

		resolution = 100;
		particleSystem = GetComponent<ParticleSystem> ();
		points = new ParticleSystem.Particle[resolution];
		lastTime = Time.time;

	}

	void FixedUpdate () {

		if (Time.time - lastTime > deltaTime) {
			points [i].position = new Vector3 (-10+i*0.01f, oscillatorScript.yPos, 0f);
			points [i].startColor = Color.red;
			points [i].startSize = 0.2f;
			points [i].velocity = new Vector3 (-5f, 0f, 0f);
			i++;
			particleSystem.SetParticles (points, i);
		}
		if (i > resolution)
			this.enabled = false;
	}
}
