using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphEnergy : MonoBehaviour {

	public int resolution = 400;
	public Oscillator oscillatorScript;
	public float piFactor = 0f;

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
		float increment = 10f / (resolution - 1);
		for(int i = 0; i < resolution; i++){
			float x = i * increment;
			points[i].position = new Vector3(x, 0f, 0f);
			if(piFactor==0) points [i].startColor = new Color (0f, x, 0f);
			else points [i].startColor = new Color (x, 0f, 0f);
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
			p.y = Sine(p.x);
			points[i].position = p;

		}

		particleSystem.SetParticles(points, points.Length);
	}


	private float Sine (float x){
		//return 1.25f + 1.25f * (Mathf.Sin(Mathf.PI * x + oscillatorScript.pulsation * 2 * Time.time + oscillatorScript.initPhase + (0.25f + piFactor) * Mathf.PI));
		return 1.25f - 1.25f * Mathf.Sin(2 * x * Mathf.PI + 2f * oscillatorScript.pulsation * Time.time + (0.5f + piFactor) * Mathf.PI + 2f * oscillatorScript.initPhase);
	}

	public void ToggleActive()
	{
		gameObject.SetActive (!gameObject.activeSelf);
	}
}
