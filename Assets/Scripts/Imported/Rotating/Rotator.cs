using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rotator : MonoBehaviour {

	public float angularFrequenecy = 10f;
	public RotatingCube2 cubeScript;
	public InputField omegaInputField;

	private LineRenderer lineRenderer;

	// Use this for initialization
	void Start () {
		angularFrequenecy = float.Parse (omegaInputField.text) * Mathf.Rad2Deg;
		lineRenderer = GetComponent<LineRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(new Vector3(0f, 0f, angularFrequenecy) * Time.deltaTime);
	}

	public void SetAngularFrequency()
	{
		angularFrequenecy = float.Parse (omegaInputField.text) * Mathf.Rad2Deg;
		cubeScript.Reset ();
	}

	public void ToggleLineRenderer()
	{
		lineRenderer.enabled = !lineRenderer.enabled;
	}
}
