using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CircleWaves : MonoBehaviour {


	public int resolution = 100;

	public InputField amplitudeInputField;
	public InputField velocityInputField;
	public InputField pulsationInputField;
	public InputField lambdaInputField;
	public InputField periodInputField;
	public InputField radiusInputField;

	public Toggle velocityToggle;
	public Toggle lambdaToggle;
	public Toggle periodToggle;

	public Text playButtonText;

	public float width = 5f;
	public float amplitude = 1f;
	public float velocity = 1f;
	public float pulsation = 1f;

	private float period;
	private float lambda;
	private float initRadius;

	private int currentResolution;
	private ParticleSystem.Particle[] points;
	private ParticleSystem particleSystem;

	private UnityAction amplitudeListener;
	private UnityAction velocityListener;
	private UnityAction periodListener;
	private UnityAction pulsationListener;
	private UnityAction lambdaListener;
	private UnityAction timeScaleListener;
	private UnityAction radiusListener;


	void Awake()
	{
		amplitudeListener = new UnityAction (ChangeAmplitude);
		velocityListener = new UnityAction (ChangeVelocity);
		periodListener = new UnityAction (ChangePeriod);
		pulsationListener = new UnityAction (ChangePulsation);
		lambdaListener = new UnityAction (ChangeLambda);
		timeScaleListener = new UnityAction (ToggleTimeScale);
		radiusListener = new UnityAction (ChangeRadius);


		ChangeAmplitude ();
		ChangePulsation ();
		ChangePeriod ();
		ChangeVelocity ();
		ChangeLambda ();
		ChangeRadius ();

	}

	void OnEnable(){
		EventManager.StartListening ("amplitudeChange", amplitudeListener);
		EventManager.StartListening ("pulsationChange", pulsationListener);
		EventManager.StartListening ("periodChange", periodListener);
		EventManager.StartListening ("lambdaChange", lambdaListener);
		EventManager.StartListening ("velocityChange", velocityListener);
		EventManager.StartListening ("timeScaleChange", timeScaleListener);
		EventManager.StartListening ("radiusChange", radiusListener);


	}

	void OnDisable(){
		EventManager.StopListening ("amplitudeChange", amplitudeListener);
		EventManager.StopListening ("pulsationChange", pulsationListener);
		EventManager.StopListening ("periodChange", periodListener);
		EventManager.StopListening ("lambdaChange", lambdaListener);
		EventManager.StopListening ("velocityChange", velocityListener);
		EventManager.StopListening ("timeScaleChange", timeScaleListener);
		EventManager.StopListening ("radiusChange", radiusListener);

	}

	void Start () {

		particleSystem = GetComponent<ParticleSystem> ();
		CreatePoints();
	}

	private void CreatePoints () {

		currentResolution = resolution;
		points = new ParticleSystem.Particle[resolution * resolution];
		float increment = width / (resolution - 1);
		int i = 0;
		for (int x = 0; x < resolution; x++) {
			for (int z = 0; z < resolution; z++) {
				Vector3 p = new Vector3(-0.5f * width + x * increment, 0f, -0.5f * width + z * increment);
				points[i].position = p;
				points[i].startColor = Color.gray + new Color(p.x/width, 0f, p.z/width);
				points[i++].startSize = 0.1f;
			}
		}
	}

	void Update () {
		if (currentResolution != resolution|| points == null) {
			CreatePoints();
		}
			
		for (int i = 0; i < points.Length; i++) 
		{
			Vector3 p = points[i].position;
			float localRadius = Mathf.Sqrt (p.x * p.x + p.z * p.z);
			float localAmplitude = amplitude *  Mathf.Sqrt (initRadius/localRadius);
			p.y = 0.1f * localAmplitude * Mathf.Sin (pulsation*(Time.time - (localRadius/velocity)));
			points[i].position = p;

			Color c = points[i].startColor;
			c.g = 5f * p.y / Mathf.Sqrt(localRadius);
			points[i].startColor = c;
		}

		particleSystem.SetParticles(points, points.Length);
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


	void ChangeRadius()
	{
		initRadius = float.Parse (radiusInputField.text);
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
