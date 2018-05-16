using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Oscillator2 : MonoBehaviour {

	public float xAmplitude = 5;
	public float xPulsation = 4;
	public float xInitPhase = 0;

	public float yAmplitude = 5;
	public float yPulsation = 4;
	public float yInitPhase = 0;

	[HideInInspector]
	public float xPos;
	[HideInInspector]
	public float yPos;

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

	private UnityAction timeScaleListener;
	private UnityAction resetListener;

	private float currentTime;

	//private UnityAction xAmplitudeListener;
	//private UnityAction yAmplitudeListener;
	//private UnityAction xPhaseListener;
	//private UnityAction yPhaseListener;
	//private UnityAction xPulsationListener;
	//private UnityAction yPulsationListener;


	void Awake ()
	{
		timeScaleListener = new UnityAction (ToggleTimeScale);
		resetListener = new UnityAction (Reset);

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
		EventManager.StartListening ("reset", resetListener);

		//EventManager.StartListening ("xAmplitudeChange", xAmplitudeListener);
		//EventManager.StartListening ("yAmplitudeChange", yAmplitudeListener);
		//EventManager.StartListening ("xPhaseChange", xPhaseListener);
		//EventManager.StartListening ("yPhaseChange", yPhaseListener);
		//EventManager.StartListening ("xPulsationChange", xPulsationListener);
		//EventManager.StartListening ("yPulsationChange", yPulsationListener);
	}

	void OnDisable ()
	{

		EventManager.StopListening ("timeScaleChange", timeScaleListener);
		EventManager.StopListening ("reset", resetListener);

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
		Reset ();
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

		float position = Mathf.Sqrt (xPos * xPos + yPos * yPos);
		float velocity = Mathf.Sqrt (xVelocity * xVelocity + yVelocity * yVelocity);
		float acceleration = Mathf.Sqrt (xAcceleration * xAcceleration + yAcceleration * yAcceleration);

		body.transform.position = new Vector3 (xPos, yPos, 0);

		xPositionText.text = "x = " + xPos.ToString("F");
		xVelocityText.text = "v(x) = " + xVelocity.ToString ("F");
		xAccelerationText.text = "a(x) = " + xAcceleration.ToString ("F");

		yPositionText.text = "y = " + yPos.ToString("F");
		yVelocityText.text = "v(y) = " + yVelocity.ToString ("F");
		yAccelerationText.text = "a(y) = " + yAcceleration.ToString ("F");

		positionText.text = "R = " + position.ToString("F");
		velocityText.text = "v = " + velocity.ToString ("F");
		accelerationText.text = "a = " + acceleration.ToString ("F");
	}

	void ChangeXAmplitude()
	{
		xAmplitude = float.Parse (xAmplitudeInput.text);
		posGraphTopValue.text = xAmplitude.ToString ();
		posGraphBottomValue.text = (-xAmplitude).ToString ();
	}

	void ChangeYAmplitude()
	{
		yAmplitude = float.Parse (yAmplitudeInput.text);
		//posGraphTopValue.text = amplitude.ToString ();
		//posGraphBottomValue.text = (-amplitude).ToString ();
	}
		
	void ChangeXPhase()
	{
		xInitPhase = float.Parse (xPhaseInput.text);
		if (xPIToggle.isOn)
			xPulsation *= Mathf.PI;
	}

	void ChangeYPhase()
	{
		yInitPhase = float.Parse (yPhaseInput.text);
		if (yPIToggle.isOn)
			yPulsation *= Mathf.PI;
	}

	void ChangeXPulsation()
	{
		xPulsation = float.Parse (xPulsationInput.text);
	}

	void ChangeYPulsation()
	{
		yPulsation = float.Parse (yPulsationInput.text);
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

	public void ResetTime()
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
		ResetTime ();
	}

	
		
}
