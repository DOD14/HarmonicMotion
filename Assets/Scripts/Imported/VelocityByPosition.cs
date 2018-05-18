using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class VelocityByPosition : MonoBehaviour {

	public DampedOscillations cubeScript;
	public float factor = 0.5f;
	public Text amplitudeText;

	private float currentTime  = 0f;


	private TrailRenderer trailRenderer; 
	private UnityAction resetListener;
	private bool ready;


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
		currentTime += Time.fixedDeltaTime;
		transform.localPosition = new Vector3 (cubeScript.transform.position.x, factor * cubeScript.rigidbody.velocity.x, 0f);
		if(ready){
			transform.localPosition = new Vector3 (float.Parse(amplitudeText.text), 0f, 0f);
			trailRenderer.Clear ();
			ready = false;
		}
		}



	void Test()
	{
		ready = true;
		currentTime = 0f;
	}
}
