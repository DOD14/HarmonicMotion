using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ComponentPhasors : MonoBehaviour {

	public GameObject rotator1;
	public GameObject rotator2;
	public GameObject pointer1;
	public GameObject pointer2;
	public GameObject pointerR;

	public Parallel oscillatorScript;
	public InputField phaseInput1;
	public InputField phaseInput2;
	public InputField amplitudeInputField1;
	public InputField amplitudeInputField2;


	private float angFrequency1;
	private float angFrequency2;

	private Vector3 rotateVector1;
	private Vector3 rotateVector2;

	private Quaternion initialRotation1;
	private Quaternion initialRotation2;

	private UnityAction resetterListener;

	// Use this for initialization

	void Awake()
	{
		initialRotation1 = rotator1.transform.rotation;
		initialRotation2 = rotator2.transform.rotation;

		resetterListener = new UnityAction (Reset);

	}

	void OnEnable () {

		Reset ();

		EventManager.StartListening ("reset", resetterListener);

	}

	// Update is called once per frame
	void Update () {
		rotator1.transform.Rotate (rotateVector1 * Time.deltaTime);
		rotator2.transform.Rotate (rotateVector2 * Time.deltaTime);
		pointerR.transform.position = pointer1.transform.position + pointer2.transform.position;
	}


	void OnDisable()
	{
		EventManager.StopListening ("reset", resetterListener);
	}

	void SetAngularFrequency1()
	{
		angFrequency1 = oscillatorScript.xPulsation * Mathf.Rad2Deg;
		rotateVector1 = new Vector3 (0f, 0f, angFrequency1);
		rotator1.transform.rotation = initialRotation1;
		rotator1.transform.Rotate(new Vector3(0f, 0f, float.Parse(phaseInput1.text) * Mathf.PI * Mathf.Rad2Deg));
	}

	void SetAngularFrequency2()
	{
		angFrequency2 = oscillatorScript.yPulsation * Mathf.Rad2Deg;
		rotateVector2 = new Vector3 (0f, 0f, angFrequency2);
		rotator2.transform.rotation = initialRotation2;
		rotator2.transform.Rotate(new Vector3(0f, 0f, float.Parse(phaseInput2.text) * Mathf.PI * Mathf.Rad2Deg));
	}


	void SetAmplitude1()
	{
		pointer1.transform.localPosition = new Vector3 (float.Parse (amplitudeInputField1.text), 0f, 0f);

	}

	void SetAmplitude2()
	{
		pointer2.transform.localPosition = new Vector3 (float.Parse (amplitudeInputField2.text), 0f, 0f);

	}

	void Reset()
	{
		SetAmplitude1 ();
		SetAmplitude2 ();
		SetAngularFrequency1 ();
		SetAngularFrequency2 ();
	}
}
