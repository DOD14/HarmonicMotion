using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PhasorLength : MonoBehaviour {

	public InputField amplitudeInputField;

	private UnityAction amplitudeListener;

	void Awake()
	{
		amplitudeListener = new UnityAction (SetAmplitude);
	}

	void OnEnable()
	{
		transform.localPosition = new Vector3 (float.Parse (amplitudeInputField.text), 0f, 0f);
		EventManager.StartListening ("reset", amplitudeListener);

	}

	void OnDisable()
	{
		EventManager.StopListening ("reset", amplitudeListener);
	}

	void SetAmplitude()
	{
		
		transform.localPosition = new Vector3 (float.Parse (amplitudeInputField.text), 0f, 0f);

	}

}
