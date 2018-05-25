using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PositionByTime : MonoBehaviour {

	public DampedOscillations cubeScript;
	public Text amplitudeText;

	private float currentTime =0f;


	private TrailRenderer trailRenderer; 
	private UnityAction resetListener;
	private bool ready=true;


	void Awake ()
	{
		resetListener = new UnityAction (Test);
		trailRenderer = GetComponent<TrailRenderer> ();
	}

	void OnEnable ()
	{
		EventManager.StartListening ("reset", resetListener);
	}

	void OnDisable ()
	{
		EventManager.StopListening ("reset", resetListener);
	}
	// Use this for initialization
	void Start () {
		Test ();	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (transform.position.x < 4f) {
			currentTime += Time.fixedDeltaTime;
			transform.localPosition = new Vector3 (currentTime * 0.4f, cubeScript.transform.position.x, 0f);
			if (ready) {
				transform.localPosition = new Vector3 (0f, float.Parse(amplitudeText.text), 0f);
				trailRenderer.Clear ();
				ready = false;
			}
		}
	}
		

	void Test()
	{
		currentTime = 0f;
		ready = true;
	}
}
