using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotatingCube : MonoBehaviour {

	public float springInitLength = 8;
	public float displacement = 1;
	public float mass = 1;
	public float springConstant = 1;

	[HideInInspector]
	public float xPos;

	public InputField amplitudeInput;
	public InputField massInput;
	public InputField KInput;

	public Text radiusText;
	public Text velocityText;
	public Text accelerationText;

	public Text playButtonText;

	public Text posGraphTopValue;
	public Text posGraphBottomValue;

	private Rigidbody body;
	private LineRenderer lineRenderer;

	private Rotator rotatorScript;


	// Use this for initialization
	void Start () 
	{
		body = GetComponent<Rigidbody> ();
		lineRenderer = GetComponent<LineRenderer> ();
		rotatorScript = transform.parent.GetComponent<Rotator> ();

		Vector3 springOrigin = new Vector3 (0, 0, 0);
		lineRenderer.SetPosition (0, springOrigin);

		SetCube ();
	}
	
	// Update is called once per frame
	void Update () {

		float radius = Vector3.Distance (transform.position, Vector3.zero);
		float angFrequencyRads = rotatorScript.angularFrequenecy * Mathf.Deg2Rad;
		float acceleration = (angFrequencyRads * angFrequencyRads) * radius - (springConstant / mass)*(radius - springInitLength);
		body.AddForce ((transform.position).normalized * acceleration, ForceMode.Acceleration);

		lineRenderer.SetPosition (1, transform.position);

		radiusText.text = radius.ToString();

	}

	public void ChangeAmplitude()
	{
		displacement = float.Parse (amplitudeInput.text);
		posGraphTopValue.text = displacement.ToString ();
		posGraphBottomValue.text = (-displacement).ToString ();
	}
		
	public  void ChangeMass()
	{
		mass = float.Parse (massInput.text);
	}

	public  void ChangeK()
	{
		springConstant = float.Parse (KInput.text);
	}
		

	public void ToggleTimeScale()
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

	void SetCube()
	{
		body.transform.position = new Vector3 (springInitLength + displacement, 0f, 0f);
		lineRenderer.SetPosition (1, body.transform.position);
	}

}
		