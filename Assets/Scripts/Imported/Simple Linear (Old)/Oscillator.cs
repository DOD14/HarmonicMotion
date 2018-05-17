using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Oscillator : MonoBehaviour {

	public float amplitude = 1;
	public float pulsation = 1;
	public float initPhase = 0;
	public float mass = 1;
	public float springConstant = 1;

	[HideInInspector]
	public float xPos;
	[HideInInspector]
	public float Et;
	[HideInInspector]
	public float Ep;
	[HideInInspector]
	public float Ec;

	public InputField amplitudeInput;
	public InputField pulsationInput;
	public InputField phaseInput; 
	public InputField massInput;
	public InputField KInput;

	public Text positionText;
	public Text velocityText;
	public Text accelerationText;

	public Text EtText;
	public Text EpText;
	public Text EcText;

	public Text playButtonText;

	public Text posGraphTopValue;
	public Text posGraphBottomValue;
	public Text energyGraphValue;

	public Toggle MassToggle;
	public Toggle PulsationToggle;
	public Toggle KToggle;
	public Toggle PIToggle;

	private Rigidbody body;
	private LineRenderer lineRenderer;

	private UnityAction valueChangedListener;
	private UnityAction timeScaleListener;

	//private UnityAction amplitudeListener;
	//private UnityAction phaseListener;
	//private UnityAction MKOmegaListener;

	private float currentTime = 0f;


	void Awake ()
	{
		valueChangedListener = new UnityAction (Reset);
		timeScaleListener = new UnityAction (ToggleTimeScale);

		//phaseListener = new UnityAction (ChangePhase);
		//MKOmegaListener = new UnityAction (ToggleMKOmega);
		//amplitudeListener = new UnityAction (ChangeAmplitude);

	}

	void OnEnable ()
	{
		EventManager.StartListening ("timeScaleChange", timeScaleListener);
		EventManager.StartListening ("reset", valueChangedListener);

		//EventManager.StartListening ("amplitudeChange", amplitudeListener);
		//EventManager.StartListening ("phaseChange", phaseListener);
		//EventManager.StartListening ("MKOmegaChange", MKOmegaListener);
	}

	void OnDisable ()
	{
		EventManager.StopListening ("timeScaleChange", timeScaleListener);
		EventManager.StopListening ("reset", valueChangedListener);

		//EventManager.StopListening ("phaseChange", phaseListener);
		//EventManager.StopListening ("MKOmegaChange", MKOmegaListener);
		//EventManager.StopListening ("amplitudeChange", amplitudeListener);

	}

	// Use this for initialization
	void Start () {

		Vector3 springOrigin = new Vector3 (-10, 0, 0);

		body = GetComponent<Rigidbody> ();
		lineRenderer = GetComponent<LineRenderer> ();

		lineRenderer.SetPosition (0, springOrigin);
		lineRenderer.SetPosition (1, body.transform.position);

		ToggleMKOmega ();
	}
	
	// Update is called once per frame
	void Update () {

		currentTime += Time.deltaTime;
		xPos = amplitude * Mathf.Sin (pulsation * currentTime + initPhase * Mathf.PI);
		float velocity = amplitude * pulsation * Mathf.Cos(pulsation * currentTime + initPhase * Mathf.PI);
		float acceleration = -xPos * pulsation * pulsation;
		Et = springConstant * amplitude * amplitude * 0.5f;
		Ep = springConstant * xPos * xPos * 0.5f;
		Ec = Et - Ep;

		body.transform.position = new Vector3 (xPos, 0, 0);
		lineRenderer.SetPosition (1, body.transform.position);

		positionText.text = "x = " + xPos.ToString("F");
		velocityText.text = "v = " + velocity.ToString ("F");
		accelerationText.text = "a = " + acceleration.ToString ("F");

		EtText.text = "Et = " + Et.ToString ("F");
		EpText.text = "Ep = " + Ep.ToString ("F");
		EcText.text = "Ec = " + Ec.ToString ("F");
	}

	void ChangeAmplitude()
	{
		amplitude = float.Parse (amplitudeInput.text);
		posGraphTopValue.text = amplitude.ToString ();
		posGraphBottomValue.text = (-amplitude).ToString ();
		Et = springConstant * amplitude * amplitude * 0.5f;
		energyGraphValue.text = Et.ToString();
	}
		
	void ChangePhase()
	{
		initPhase = float.Parse (phaseInput.text);

	}
/*
	public  void ChangeMass()
	{
		mass = float.Parse (massInput.text);
	}

	public  void ChangeK()
	{
		stringConstant = float.Parse (KInput.text);
	}
		
	public void ChangePulsation()
	{
		pulsation = float.Parse (pulsationInput.text);
	}
*/
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

	void ToggleMKOmega()
	{
		if (MassToggle.isOn) 
		{
			pulsation = float.Parse (pulsationInput.text);
			if (PIToggle.isOn)
				pulsation *= Mathf.PI;
			springConstant = float.Parse (KInput.text);
			mass = springConstant / (pulsation * pulsation);
			massInput.text = mass.ToString ();

			massInput.enabled = false;
			KInput.enabled = true;
			pulsationInput.enabled = true;

		}
		if (KToggle.isOn) 
		{
			mass = float.Parse (massInput.text);
			pulsation = float.Parse (pulsationInput.text);
			if (PIToggle.isOn)
				pulsation *= Mathf.PI;
			springConstant = mass * pulsation * pulsation;
			KInput.text = springConstant.ToString ();

			KInput.enabled = false;
			massInput.enabled = true;
			pulsationInput.enabled = true;
		}
		if (PulsationToggle.isOn) 
		{
			PIToggle.isOn = false;
			mass = float.Parse (massInput.text);
			springConstant = float.Parse (KInput.text);
			pulsation = Mathf.Sqrt (springConstant / mass);
			pulsationInput.text = pulsation.ToString ();

			pulsationInput.enabled = false;
			KInput.enabled = true;
			massInput.enabled = true;
		}

		Et = springConstant * amplitude * amplitude * 0.5f;
		energyGraphValue.text = Et.ToString ();
	}

	public void ResetTime()
	{
		currentTime = 0f;
	}

	void Reset()
	{
		ChangeAmplitude ();
		ChangePhase();
		ToggleMKOmega ();
		ResetTime ();
	}

}
