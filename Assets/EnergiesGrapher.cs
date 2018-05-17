using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnergiesGrapher : MonoBehaviour {

	public DampedOscillations cubeScript;
	public float factor = 0.5f;

	private float currentTime =0f;
	public GameObject TETracer;
	public GameObject PETracer;
	public GameObject KETracer;

	public Text amplitudeText;
	public Text KText;


	private TrailRenderer TTrailRenderer; 
	private TrailRenderer PTrailRenderer; 
	private TrailRenderer KTrailRenderer; 
	private UnityAction resetListener;
	private bool ready=true;


	void Awake ()
	{
		resetListener = new UnityAction (Test);
		TTrailRenderer = TETracer.GetComponent<TrailRenderer> ();
		PTrailRenderer = PETracer.GetComponent<TrailRenderer> ();
		KTrailRenderer = KETracer.GetComponent<TrailRenderer> ();
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

		float cubeX = cubeScript.transform.position.x;
		float cubeV = cubeScript.rigidbody.velocity.x;

		float PE = cubeScript.k * cubeX * cubeX;
		float KE = cubeScript.mass * cubeV * cubeV;
		float TE = PE + KE;

		TETracer.transform.localPosition = new Vector3 (cubeX, factor * TE, 0f);
		PETracer.transform.localPosition = new Vector3 (cubeX, factor * PE, 0f);
		KETracer.transform.localPosition = new Vector3 (cubeX, factor * KE, 0f);

		if (ready) {

			float amplitude = float.Parse (amplitudeText.text);
			float initPE = factor * float.Parse (KText.text) * amplitude * amplitude;
			Debug.Log ("here");


			TETracer.transform.localPosition = new Vector3 (amplitude, initPE, 0f);
			PETracer.transform.localPosition = new Vector3 (amplitude, initPE, 0f);
			KETracer.transform.localPosition = new Vector3 (amplitude, 0f, 0f);

			TTrailRenderer.Clear ();
			PTrailRenderer.Clear ();
			KTrailRenderer.Clear ();


			ready = false;
		}

	}


	void Test()
	{
		ready = true;
		currentTime = 0f;
	}
}
	