using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class RotatingCube2 : MonoBehaviour {

	public float initLength = 5f;
	public float mass = 1f;
	public float springConstant = 10f;
	public Rotator rotatorScript;

	public InputField initLengthInput;
	public InputField initPosInput;
	public InputField massInput;
	public InputField KInput;

	public Text radiusText;
	public Text rVelocityText;
	public Text rAccelerationText;
	public Text elongationText;
	public Text xVelocityText;
	public Text xAccelerationText;
	public Text playButtonText;

	private Rigidbody rigidbody;
	private TrailRenderer trailRenderer;

	private Vector3 origin = new Vector3(0f, 0f, 0f);

	private float lastVelocity = 0f;
	private float lastPosition;
	private float omega;

	private UnityAction resetListener;
	private UnityAction timeScaleListener;


	void Awake()
	{
		resetListener = new UnityAction (Reset);
		timeScaleListener = new UnityAction (ToggleTimeScale);
	}

	void OnEnable()
	{
		EventManager.StartListening ("reset", resetListener);
		EventManager.StartListening ("timeScaleChange", timeScaleListener);

	}

	void OnDisable()
	{
		EventManager.StopListening ("reset", resetListener);
		EventManager.StopListening ("timeScaleChange", timeScaleListener);

	}

	// Use this for initialization
	void Start () {
		rigidbody = GetComponent<Rigidbody> ();
		trailRenderer = GetComponent<TrailRenderer> ();
		Reset ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float radius = Vector3.Distance (transform.position, origin);
		float acceleration = -(springConstant/mass) * (radius - initLength) + omega * omega * radius;
		lastVelocity += acceleration * Time.fixedDeltaTime;
		lastPosition += lastVelocity * Time.fixedDeltaTime;
		rigidbody.transform.localPosition = Vector3.right * lastPosition;

		float rVelocity = omega * radius;
		float rAcceleration = rVelocity * omega;

		elongationText.text = "x = " + lastPosition.ToString ("F");
		xVelocityText.text = "v(x) = " + lastVelocity.ToString ("F");
		xAccelerationText.text = "a(x) = " + acceleration.ToString ("F");

		radiusText.text = "R = " + radius.ToString ("F");
		rVelocityText.text = "v(R) = " + rVelocity.ToString ("F");
		rAccelerationText.text = "a(R) = " + rAcceleration.ToString ("F");


	}

	public void Reset()
	{
		this.enabled = false;
		rotatorScript.enabled = false;

		omega = rotatorScript.angularFrequenecy * Mathf.Deg2Rad;

		initLength = float.Parse(initLengthInput.text);
		lastPosition = float.Parse(initPosInput.text);

		if (lastPosition - initLength > initLength) {
			initLength = lastPosition * 0.5f;
			initLengthInput.text = initLength.ToString ();
		}
		
		springConstant = float.Parse(KInput.text);
		mass = float.Parse(massInput.text);

		transform.position = new Vector3 (lastPosition, 0f, 0f);
		lastVelocity = 0f;
		rotatorScript.transform.rotation = Quaternion.identity;

		this.enabled = true;
		rotatorScript.enabled = true;
		Invoke ("ClearTrail", Time.deltaTime);

	}

	void ToggleTimeScale()
	{
		if (Time.timeScale == 1) 
		{
			Time.timeScale = 0;
			playButtonText.text = "Start";
		} 
		else 
		{
			Time.timeScale = 1;
			playButtonText.text = "Stop";
		}
	}

	void ClearTrail()
	{
		trailRenderer.Clear ();
	}
}
