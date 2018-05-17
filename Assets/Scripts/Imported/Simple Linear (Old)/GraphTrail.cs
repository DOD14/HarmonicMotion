using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphTrail : MonoBehaviour {

	public int resolution = 400;

	public Oscillator oscillatorScript;

	private int currentResolution;
	private ParticleSystem.Particle[] points;
	private ParticleSystem particleSystem;

	void Start () {

		resolution = 400;
		particleSystem = GetComponent<ParticleSystem> ();
		CreatePoints();
	}

	private void CreatePoints () {

		currentResolution = resolution;
		points = new ParticleSystem.Particle[resolution];
		float increment = 4f / (resolution - 1);
		for(int i = 0; i < resolution; i++){
			float y = i * increment;
			points[i].position = new Vector3(0f, y, 0f);
			points[i].startColor = new Color(0f, y, 0f);
			points[i].startSize = .1f;
		}
	}

	void Update () {
		if (currentResolution != resolution|| points == null) {
			CreatePoints();
		}

		for (int i = 0; i < resolution; i++) 
		{
			Vector3 p = points[i].position;
			p.x = Sine(p.y);
			points[i].position = p;

			Color c = points[i].startColor;
			c.r = p.x;
			points[i].startColor = c;
		}

		particleSystem.SetParticles(points, points.Length);
	}
		

	private float Sine (float x){
		return oscillatorScript.amplitude * 0.5f * Mathf.Sin(0.5f * Mathf.PI * x + oscillatorScript.pulsation * Time.time + oscillatorScript.initPhase);
	}

	public void ToggleActive()
	{
		gameObject.SetActive (!gameObject.activeSelf);
	}
}
