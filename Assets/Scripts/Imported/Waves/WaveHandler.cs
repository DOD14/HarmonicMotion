using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class WaveHandler : MonoBehaviour {

	public int resolution = 400;

	public ParticleSystem xParticleSystem;
	public ParticleSystem yParticleSystem;
	private ParticleSystem rParticleSystem;

	public InputField xAmplitudeInputField;
	public InputField xVelocityInputField;
	public InputField xPulsationInputField;
	public InputField xLambdaInputField;
	public InputField xPeriodInputField;

	public InputField yAmplitudeInputField;
	public InputField yVelocityInputField;
	public InputField yPulsationInputField;
	public InputField yLambdaInputField;
	public InputField yPeriodInputField;

	public Toggle xVelocityToggle;
	public Toggle xLambdaToggle;
	public Toggle xPeriodToggle;

	public Toggle yVelocityToggle;
	public Toggle yLambdaToggle;
	public Toggle yPeriodToggle;

	public Text playButtonText;

	public Color xColor = Color.red;
	public Color yColor = Color.green;
	public Color rColor = Color.yellow;

	private int currentResolution;

	public float xAmplitude = 1f;
	public float xVelocity = 1f;
	public float xPulsation = 1f;
	private float xPeriod;
	private float xLambda;

	public float yAmplitude = 1f;
	public float yVelocity = 1f;
	public float yPulsation = 1f;
	private float yPeriod;
	private float yLambda;

	private ParticleSystem.Particle[] xPoints;
	private ParticleSystem.Particle[] yPoints;
	private ParticleSystem.Particle[] rPoints;

	private Vector3 origin;

	private UnityAction amplitudeListener;
	private UnityAction velocityListener;
	private UnityAction periodListener;
	private UnityAction pulsationListener;
	private UnityAction lambdaListener;
	private UnityAction timeScaleListener;


	void Awake()
	{
		amplitudeListener = new UnityAction (ChangeAmplitude);
		velocityListener = new UnityAction (ChangeVelocity);
		periodListener = new UnityAction (ChangePeriod);
		pulsationListener = new UnityAction (ChangePulsation);
		lambdaListener = new UnityAction (ChangeLambda);
		timeScaleListener = new UnityAction (ToggleTimeScale);


		ChangeAmplitude ();
		ChangePulsation ();
		ChangePeriod ();
		ChangeVelocity ();
		ChangeLambda ();

	}

	void OnEnable(){
		EventManager.StartListening ("amplitudeChange", amplitudeListener);
		EventManager.StartListening ("pulsationChange", pulsationListener);
		EventManager.StartListening ("periodChange", periodListener);
		EventManager.StartListening ("lambdaChange", lambdaListener);
		EventManager.StartListening ("velocityChange", velocityListener);
		EventManager.StartListening ("timeScaleChange", timeScaleListener);


	}

	void OnDisable(){
		EventManager.StopListening ("amplitudeChange", amplitudeListener);
		EventManager.StopListening ("pulsationChange", pulsationListener);
		EventManager.StopListening ("periodChange", periodListener);
		EventManager.StopListening ("lambdaChange", lambdaListener);
		EventManager.StopListening ("velocityChange", velocityListener);
		EventManager.StopListening ("timeScaleChange", timeScaleListener);

	}

	void Start () {

		resolution = 400;
		CreatePoints();
		origin = transform.position;
	}

	private void CreatePoints () {

		currentResolution = resolution;
		xPoints = new ParticleSystem.Particle[resolution];
		float increment = 5f / (resolution - 1);
		for(int i = 0; i < resolution; i++){
			float x = i * increment;
			xPoints[i].position = new Vector3(x-2.5f, 0.5f, 0f);
			xPoints [i].startColor = xColor;
			yPoints[i].startSize = .05f;
		}



		yPoints = new ParticleSystem.Particle[resolution];
		for(int i = 0; i < resolution; i++){
			float y = i * increment;
			yPoints[i].position = new Vector3(y-2.5f, 0.5f, 0f);
			yPoints [i].startColor = yColor;
			yPoints[i].startSize = .05f;
		}

		rPoints = new ParticleSystem.Particle[resolution];
		for(int i = 0; i < resolution; i++){
			float r = i * increment;
			rPoints[i].position = new Vector3(r-2.5f, 0.5f, 0f);
			rPoints [i].startColor = rColor;
			rPoints[i].startSize = .05f;
		}
	}

	void Update () {
		if (currentResolution != resolution|| xPoints == null) {
			CreatePoints();
		}

		for (int i = 0; i < resolution; i++) 
		{
			Vector3 p = xPoints[i].position;
			p.y = xAmplitude * (Mathf.Sin (xPulsation * (Time.time - p.x / xVelocity)));
			xPoints[i].position = p;
			xPoints [i].position += origin;

			p = yPoints[i].position;
			p.y = yAmplitude * (Mathf.Sin (yPulsation * (Time.time - p.x / yVelocity)));
			yPoints[i].position = p;
			yPoints [i].position += origin;

			p = rPoints[i].position;
			p.y = xPoints [i].position.y + yPoints [i].position.y;
			rPoints[i].position = p;
			rPoints [i].position += origin;
		}
			
		xParticleSystem.SetParticles(xPoints, xPoints.Length);
		yParticleSystem.SetParticles(yPoints, yPoints.Length);
		rParticleSystem.SetParticles(rPoints, rPoints.Length);
	}



	void ChangeVelocity()
	{
		xVelocity = float.Parse (xVelocityInputField.text);

		if (xPeriodToggle.isOn) {
			xPeriod = xLambda / xVelocity;
			xPeriodInputField.text = xPeriod.ToString("F");
			xPulsation = 2 * Mathf.PI / xPeriod;
			xPulsationInputField.text = xPulsation.ToString("F");
		} 

		else if (xLambdaToggle.isOn) {
			xLambda = xVelocity * xPeriod;
			xLambdaInputField.text = xLambda.ToString("F");
		}

		yVelocity = float.Parse (yVelocityInputField.text);

		if (yPeriodToggle.isOn) {
			yPeriod = yLambda / yVelocity;
			yPeriodInputField.text = yPeriod.ToString("F");
			yPulsation = 2 * Mathf.PI / yPeriod;
			yPulsationInputField.text = yPulsation.ToString("F");
		} 

		else if (yLambdaToggle.isOn) {
			yLambda = yVelocity * yPeriod;
			yLambdaInputField.text = yLambda.ToString("F");
		}
	}

	void ChangeAmplitude()
	{
		xAmplitude = float.Parse (xAmplitudeInputField.text);
		yAmplitude = float.Parse (yAmplitudeInputField.text);

	}

	void ChangePulsation()
	{
		xPulsation = float.Parse (xPulsationInputField.text);
		xPeriod = 2 * Mathf.PI / xPulsation;
		xPeriodInputField.text = xPeriod.ToString("F");

		yPulsation = float.Parse (yPulsationInputField.text);
		yPeriod = 2 * Mathf.PI / yPulsation;
		yPeriodInputField.text = yPeriod.ToString("F");
	}

	void ChangePeriod()
	{
		xPeriod = float.Parse (xPeriodInputField.text);
		xPulsation = 2 * Mathf.PI / xPeriod;
		xPulsationInputField.text = xPulsation.ToString("F");

		if (xVelocityToggle.isOn) {
			xVelocity = xLambda / xPeriod;
			xVelocityInputField.text = xVelocity.ToString("F");
		}

		else if (xLambdaToggle.isOn) {
			xLambda = xVelocity * xPeriod;
			xLambdaInputField.text = xLambda.ToString("F");
		}


		yPeriod = float.Parse (yPeriodInputField.text);
		yPulsation = 2 * Mathf.PI / yPeriod;
		yPulsationInputField.text = yPulsation.ToString("F");

		if (yVelocityToggle.isOn) {
			yVelocity = yLambda / yPeriod;
			yVelocityInputField.text = yVelocity.ToString("F");
		}

		else if (yLambdaToggle.isOn) {
			yLambda = yVelocity * yPeriod;
			yLambdaInputField.text = yLambda.ToString("F");
		}
	}

	void ChangeLambda()
	{
		xLambda = float.Parse (xLambdaInputField.text);

		if (xVelocityToggle.isOn) {
			xVelocity = xLambda / xPeriod;
			xVelocityInputField.text = xVelocity.ToString("F");
		}

		else if (xPeriodToggle.isOn) {
			xPeriod = xLambda / xVelocity;
			xPeriodInputField.text = xPeriod.ToString("F");
			xPulsation = 2 * Mathf.PI / xPeriod;
			xPulsationInputField.text = xPulsation.ToString("F");
		} 


		yLambda = float.Parse (yLambdaInputField.text);

		if (yVelocityToggle.isOn) {
			yVelocity = yLambda / yPeriod;
			yVelocityInputField.text = yVelocity.ToString("F");
		}

		else if (yPeriodToggle.isOn) {
			yPeriod = yLambda / yVelocity;
			yPeriodInputField.text = yPeriod.ToString("F");
			yPulsation = 2 * Mathf.PI / yPeriod;
			yPulsationInputField.text = yPulsation.ToString("F");
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




