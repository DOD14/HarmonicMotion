using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Wave : MonoBehaviour {

	public int resolution = 400;

	public InputField amplitudeInputField;
	public InputField velocityInputField;
	public InputField pulsationInputField;
	public InputField lambdaInputField;
	public InputField periodInputField;
	public InputField initPhaseInputField;

	public Toggle velocityToggle;
	public Toggle lambdaToggle;
	public Toggle periodToggle;

	public Text playButtonText;

	public Color color = Color.red;


	private int currentResolution;

	public float amplitude = 1f;
	public float velocity = 1f;
	public float pulsation = 1f;
	public float initPhase = 0f;
	private float period;
	private float lambda;
	//private float tension;
	//private float rho;

	[HideInInspector]
	public ParticleSystem.Particle[] points;
	private ParticleSystem particleSystem;

	private Vector3 origin;

	private UnityAction amplitudeListener;
	private UnityAction velocityListener;
	private UnityAction periodListener;
	private UnityAction pulsationListener;
	private UnityAction lambdaListener;
	private UnityAction initPhaseListener;
	private UnityAction timeScaleListener;


	void Awake()
	{
		amplitudeListener = new UnityAction (ChangeAmplitude);
		velocityListener = new UnityAction (ChangeVelocity);
		periodListener = new UnityAction (ChangePeriod);
		pulsationListener = new UnityAction (ChangePulsation);
		lambdaListener = new UnityAction (ChangeLambda);
		initPhaseListener = new UnityAction (ChangeInitPhase);
		timeScaleListener = new UnityAction (ToggleTimeScale);


		ChangeAmplitude ();
		ChangePulsation ();
		ChangePeriod ();
		ChangeVelocity ();
		ChangeLambda ();
		ChangeInitPhase ();

	}

	void OnEnable(){
		EventManager.StartListening ("amplitudeChange", amplitudeListener);
		EventManager.StartListening ("pulsationChange", pulsationListener);
		EventManager.StartListening ("periodChange", periodListener);
		EventManager.StartListening ("lambdaChange", lambdaListener);
		EventManager.StartListening ("velocityChange", velocityListener);
		EventManager.StartListening ("initPhaseChange", initPhaseListener);
		EventManager.StartListening ("timeScaleChange", timeScaleListener);


	}

	void OnDisable(){
		EventManager.StopListening ("amplitudeChange", amplitudeListener);
		EventManager.StopListening ("pulsationChange", pulsationListener);
		EventManager.StopListening ("periodChange", periodListener);
		EventManager.StopListening ("lambdaChange", lambdaListener);
		EventManager.StopListening ("velocityChange", velocityListener);
		EventManager.StopListening ("initPhaseChange", initPhaseListener);
		EventManager.StopListening ("timeScaleChange", timeScaleListener);

	}

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
			p.y = Sine(p.x);
			points[i].position = p;
			points [i].position += origin;
		}

		particleSystem.SetParticles(points, points.Length);
	}


	private float Sine (float x){
		return amplitude * (Mathf.Sin (Mathf.PI * initPhase + pulsation * (Time.time - x / velocity)));
	}

	void ChangeVelocity()
	{
		velocity = float.Parse (velocityInputField.text);

		if (periodToggle.isOn) {
			period = lambda / velocity;
			periodInputField.text = period.ToString("F");
			pulsation = 2 * Mathf.PI / period;
			pulsationInputField.text = pulsation.ToString("F");
		} 

		else if (lambdaToggle.isOn) {
			lambda = velocity * period;
			lambdaInputField.text = lambda.ToString("F");
		}
	}

	void ChangeAmplitude()
	{
		amplitude = float.Parse (amplitudeInputField.text);

	}

	void ChangeInitPhase()
	{
		initPhase = float.Parse (initPhaseInputField.text);

	}

	void ChangePulsation()
	{
		pulsation = float.Parse (pulsationInputField.text);

		period = 2 * Mathf.PI / pulsation;
		periodInputField.text = period.ToString("F");
	}

	void ChangePeriod()
	{
		period = float.Parse (periodInputField.text);
		pulsation = 2 * Mathf.PI / period;
		pulsationInputField.text = pulsation.ToString("F");

		if (velocityToggle.isOn) {
			velocity = lambda / period;
			velocityInputField.text = velocity.ToString("F");
		}

		else if (lambdaToggle.isOn) {
			lambda = velocity * period;
			lambdaInputField.text = lambda.ToString("F");
		}
	}

	void ChangeLambda()
	{
		lambda = float.Parse (lambdaInputField.text);

		if (velocityToggle.isOn) {
			velocity = lambda / period;
			velocityInputField.text = velocity.ToString("F");
		}

		else if (periodToggle.isOn) {
			period = lambda / velocity;
			periodInputField.text = period.ToString("F");
			pulsation = 2 * Mathf.PI / period;
			pulsationInputField.text = pulsation.ToString("F");
		} 
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

}


	/*
	void SetValues()
	{
		amplitude = float.Parse (amplitudeInputField.text);
		pulsation = float.Parse (pulsationInputField.text);
		velocity = float.Parse (velocityInputField.text);
		lambda = float.Parse (lambdaInputField.text);
		//tension = float.Parse (tensionInputField.text);
		//rho = float.Parse (rhoInputField.text);
		period = float.Parse (periodInputField.text);

		period = 2f * Mathf.PI / pulsation;

		velocity = Mathf.Sqrt (tension / rho);

		lambda = velocity * period;

		//if i change pulsation => period, either lambda or velocity then either rho or tension
		//if i change velocity => lambda, either tension or rho
		//if i change lambda => either velocity then (either tension or rho), or period and then pulsation
		//if i change tension => either rho or (velocity, then either lambda or (period, then pulsation))
		//if i change rho => either tension or (velocity, then either lambda or (period, then pulsation))
		//if i change period => pulsation, either lambda or velocity then rho or tension
		}
*/




