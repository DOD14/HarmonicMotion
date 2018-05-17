using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DampedOscillations : MonoBehaviour {

	public float amplitude = 2f;
	public float viscuousDamping = 3f;
	public float k = 10f;

	public InputField amplitudeInputField;
	public InputField viscuousDampingInputField;
	public InputField pulsationInputField;
	public InputField massInputField;
	public InputField KInputField;

	public Toggle pulsationToggle;
	public Toggle massToggle;
	public Toggle KToggle;

	public Text playButtonText;

	[HideInInspector]
	public Rigidbody rigidbody;

	private float pulsation;
	public float mass = 1f;

	private UnityAction valueChangedListener;
	private UnityAction timeScaleListener;



	void Awake ()
	{
		valueChangedListener = new UnityAction (ValueChanged);
		timeScaleListener = new UnityAction (ToggleTimeScale);
	}

	void OnEnable ()
	{
		EventManager.StartListening ("timeScaleChange", timeScaleListener);
		EventManager.StartListening ("reset", valueChangedListener);
	}

	void OnDisable ()
	{
		EventManager.StopListening ("timeScaleChange", timeScaleListener);
		EventManager.StopListening ("reset", valueChangedListener);
	}

	// Use this for initialization
	void Start () {
		rigidbody = GetComponent<Rigidbody> ();
		ValueChanged ();
	}

	void Reset()
	{
		rigidbody.position = new Vector3 (amplitude, 0f, 0f);
		rigidbody.velocity = Vector3.zero;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//float x = amplitude * Mathf.Sin (pulsation * Time.time);
		//rigidbody.position = new Vector3 (x, 0f, 0f);

		float x = rigidbody.position.x;
		float force = -k * x - viscuousDamping * rigidbody.velocity.x;
		rigidbody.AddForce (new Vector3(force, 0f, 0f));
	}

	void ValueChanged()
	{
		amplitude = float.Parse (amplitudeInputField.text);
		viscuousDamping = float.Parse (viscuousDampingInputField.text);

		if (pulsationToggle.isOn) {
			mass = float.Parse (massInputField.text);
			rigidbody.mass = mass;
			k = float.Parse (KInputField.text);
			pulsation = Mathf.Sqrt (mass / k);
			pulsationInputField.text = pulsation.ToString ();
		}

		else if (massToggle.isOn) {
			pulsation = float.Parse (pulsationInputField.text);
			k = float.Parse (KInputField.text);
			mass = pulsation * pulsation * k;
			rigidbody.mass = mass;
			massInputField.text = mass.ToString ();
		}

		else if (KToggle.isOn) {
			pulsation = float.Parse (pulsationInputField.text);
			mass = float.Parse (massInputField.text);
			rigidbody.mass = mass;
			k = pulsation * pulsation * mass;
			KInputField.text = k.ToString ();
		}

		Reset ();
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
