using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Parallel: MonoBehaviour {

	public float xAmplitude = 5;
	public float xPulsation = 4;
	public float xInitPhase = 0;

	public float yAmplitude = 5;
	public float yPulsation = 2;
	public float yInitPhase = 0;

	[HideInInspector]
	public float xPos;
	[HideInInspector]
	public float yPos;
	[HideInInspector]
	public float position;
	[HideInInspector]
	public float fullAmplitude;

	public InputField xAmplitudeInput;
	public InputField xPulsationInput;
	public InputField xPhaseInput; 

	public InputField yAmplitudeInput;
	public InputField yPulsationInput;
	public InputField yPhaseInput; 

	public Text positionText;
	public Text velocityText;
	public Text accelerationText;

	public Text xPositionText;
	public Text xVelocityText;
	public Text xAccelerationText;

	public Text yPositionText;
	public Text yVelocityText;
	public Text yAccelerationText;

	public Text playButtonText;

	public Text posGraphTopValue;
	public Text posGraphBottomValue;

	public Toggle xPIToggle;
	public Toggle yPIToggle;

	private Rigidbody body;
	private LineRenderer lineRenderer;

	private UnityAction timeScaleListener;
	private UnityAction valueChangedListener;

	//private UnityAction xAmplitudeListener;
	//private UnityAction yAmplitudeListener;
	//private UnityAction xPhaseListener;
	//private UnityAction yPhaseListener;
	//private UnityAction xPulsationListener;
	//private UnityAction yPulsationListener;

	private float currentTime = 0f;


	void Awake ()
	{
		timeScaleListener = new UnityAction (ToggleTimeScale);
		valueChangedListener = new UnityAction (Reset);

		//xAmplitudeListener = new UnityAction (ChangeXAmplitude);
		//yAmplitudeListener = new UnityAction (ChangeYAmplitude);
		//xPhaseListener = new UnityAction (ChangeXPhase);
		//yPhaseListener = new UnityAction (ChangeYPhase);
		//xPulsationListener = new UnityAction (ChangeXPulsation);
		//yPulsationListener = new UnityAction (ChangeYPulsation);
	}

	void OnEnable ()
	{
		EventManager.StartListening ("timeScaleChange", timeScaleListener);
		EventManager.StartListening ("reset", valueChangedListener);

		//EventManager.StartListening ("xAmplitudeChange", xAmplitudeListener);
		//EventManager.StartListening ("yAmplitudeChange", yAmplitudeListener);
		//EventManager.StartListening ("xPhaseChange", xPhaseListener);
		//EventManager.StartListening ("yPhaseChange", yPhaseListener);
		//EventManager.StartListening ("xPulsationChange", xPulsationListener);
		//EventManager.StartListening ("yPulsationChange", yPulsationListener);
	}

	void OnDisable ()
	{
		EventManager.StopListening ("reset", valueChangedListener);

		//EventManager.StopListening ("xAmplitudeChange", xAmplitudeListener);
		//EventManager.StopListening ("yAmplitudeChange", yAmplitudeListener);
		//EventManager.StopListening ("xPhaseChange", xPhaseListener);
		//EventManager.StopListening ("yPhaseChange", yPhaseListener);
		//EventManager.StopListening ("xPulsationChange", xPulsationListener);
		//EventManager.StopListening ("yPulsationChange", yPulsationListener);
	}

	// Use this for initialization
	void Start () 
	{
		body = GetComponent<Rigidbody> ();
		lineRenderer = GetComponent<LineRenderer> ();

		lineRenderer.SetPosition (0, new Vector3(-10f, 0f, 0f));
		lineRenderer.SetPosition (1, body.transform.position);

		Reset ();

		fullAmplitude = xAmplitude + yAmplitude;

	}
	
	// Update is called once per frame
	void Update () {
		currentTime += Time.deltaTime;

		xPos = xAmplitude * Mathf.Sin (xPulsation * currentTime + xInitPhase * Mathf.PI);
		float xVelocity = xAmplitude * xPulsation * Mathf.Cos(xPulsation * currentTime + xInitPhase * Mathf.PI);
		float xAcceleration = -xPos * xPulsation * xPulsation;

		yPos = yAmplitude * Mathf.Sin (yPulsation * currentTime + yInitPhase * Mathf.PI);
		float yVelocity = yAmplitude * yPulsation * Mathf.Cos(yPulsation * currentTime + yInitPhase * Mathf.PI);
		float yAcceleration = -yPos * yPulsation * yPulsation;

		position = xPos + yPos;
		float velocity = xVelocity + yVelocity;
		float acceleration = xAcceleration + yAcceleration;

		body.transform.position = new Vector3 (position, 0f, 0f);
		lineRenderer.SetPosition (1, body.transform.position);

		xPositionText.text = "x = " + xPos.ToString("F");
		xVelocityText.text = "v = " + xVelocity.ToString ("F");
		xAccelerationText.text = "a = " + xAcceleration.ToString ("F");

		yPositionText.text = "x = " + yPos.ToString("F");
		yVelocityText.text = "v = " + yVelocity.ToString ("F");
		yAccelerationText.text = "a = " + yAcceleration.ToString ("F");

		positionText.text = "x = " + position.ToString("F");
		velocityText.text = "v = " + velocity.ToString ("F");
		accelerationText.text = "a = " + acceleration.ToString ("F");
	}

	void ChangeXAmplitude()
	{
		xAmplitude = float.Parse (xAmplitudeInput.text);
		fullAmplitude = xAmplitude + yAmplitude;
		posGraphTopValue.text = fullAmplitude.ToString ();
		posGraphBottomValue.text = (-fullAmplitude).ToString ();

	}

	void ChangeYAmplitude()
	{
		yAmplitude = float.Parse (yAmplitudeInput.text);
		fullAmplitude = xAmplitude + yAmplitude;

		//posGraphTopValue.text = amplitude.ToString ();
		//posGraphBottomValue.text = (-amplitude).ToString ();
	}
		
	void ChangeXPhase()
	{
		xInitPhase = float.Parse (xPhaseInput.text);

	}

	void ChangeYPhase()
	{
		yInitPhase = float.Parse (yPhaseInput.text);

	}

	void ChangeXPulsation()
	{
		xPulsation = float.Parse (xPulsationInput.text);
		if (xPIToggle.isOn)
			xPulsation *= Mathf.PI;

	}

	void ChangeYPulsation()
	{
		yPulsation = float.Parse (yPulsationInput.text);
		if (yPIToggle.isOn)
			yPulsation *= Mathf.PI;

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
		
	void ResetTime()
	{
		currentTime = 0f;
	}

	void Reset()
	{
		ChangeXAmplitude ();
		ChangeYAmplitude ();
		ChangeXPulsation ();
		ChangeYPulsation ();
		ChangeXPhase ();
		ChangeYPhase ();
        body.transform.position = Vector3.zero;
		ResetTime ();
	}

}
