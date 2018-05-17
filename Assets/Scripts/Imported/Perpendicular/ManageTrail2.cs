using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageTrail2 : MonoBehaviour {

	public Slider trailLifeTimeSlider;

	private TrailRenderer trail;

	// Use this for initialization
	void Start () {
		trail = GetComponent<TrailRenderer> ();
	}

	public void ToggleTrail()
	{
		trail.enabled = !trail.enabled;
	}

	public void SetLifeTime()
	{
		trail.time = trailLifeTimeSlider.value;
	}
}
