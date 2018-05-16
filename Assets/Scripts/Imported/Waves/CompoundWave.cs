using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CompoundWave : MonoBehaviour {

	public int resolution = 400;
	public Wave wave1;
	public Wave wave2;

	public Color color = Color.red;

	private int currentResolution;

	public float amplitude = 1f;
	public float velocity = 1f;
	public float pulsation = 1f;
	private float period;
	private float lambda;
	//private float tension;
	//private float rho;

	[HideInInspector]
	public ParticleSystem.Particle[] points;
	private ParticleSystem particleSystem;

	private Vector3 origin;


	void Start () {

		resolution = 400;
		particleSystem = GetComponent<ParticleSystem> ();
		CreatePoints();
		origin = transform.position;
	}

	private void CreatePoints () {

		currentResolution = resolution;
		points = new ParticleSystem.Particle[resolution];
		float increment = 5f / (resolution - 1);
		for(int i = 0; i < resolution; i++){
			float x = i * increment;
			points[i].position = new Vector3(x-2.5f, 0.5f, 0f);
			points [i].startColor = color;
			points[i].startSize = .05f;
		}
	}

	void Update () {
		if (currentResolution != resolution|| points == null) {
			CreatePoints();
		}

		for (int i = 0; i < resolution; i++) 
		{
			Vector3 p = points[i].position;
			p.y = wave1.points [i].position.y + wave2.points [i].position.y - 2 * origin.y;
			points[i].position = p;
			points [i].position += origin;
		}

		particleSystem.SetParticles(points, points.Length);
	}


	private float Sine (float x){
		return amplitude * (Mathf.Sin (pulsation * (Time.time - x / velocity)));
	}
}