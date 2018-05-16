using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RotateByRads : MonoBehaviour {

	public Oscillator oscillatorScript;
	public InputField phaseInput;

	private float angFrequency;
	private Vector3 rotateVector;
	private Quaternion initialRotation;
	private UnityAction resetterListener;
	// Use this for initialization

	void Awake()
	{
		initialRotation = transform.rotation;
		resetterListener = new UnityAction (SetAngularFrequency);
	}

	void OnEnable () {
		SetAngularFrequency ();
        EventManager.StartListening("reset", resetterListener);
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (rotateVector * Time.deltaTime);
	}
		

	void OnDisable()
	{
        EventManager.StopListening("reset", resetterListener);

	}

	void SetAngularFrequency()
	{
		angFrequency = oscillatorScript.pulsation * Mathf.Rad2Deg;
		rotateVector = new Vector3 (0f, 0f, angFrequency);
		transform.rotation = initialRotation;
        Debug.Log("here");
		transform.Rotate(new Vector3(0f, 0f, float.Parse(phaseInput.text) * Mathf.PI * Mathf.Rad2Deg));
	}
}
