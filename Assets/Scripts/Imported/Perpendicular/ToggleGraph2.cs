using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleGraph2 : MonoBehaviour {


	public GameObject graphUIHolder;
	public GameObject graphHolder;
	public GameObject system;
	public GameObject cube;
	public GameObject markerHolder;

	public GraphPos2X xPosGraph;
	public GraphPos2Y yPosGraph;

	public Text buttonText;

	public Toggle trailToggle;
	public Toggle markersToggle;

	public void ToggleGraphs()
	{
		graphUIHolder.SetActive (!graphUIHolder.activeSelf);
		graphHolder.SetActive (!graphHolder.activeSelf);
		system.SetActive (!system.activeSelf);
		markerHolder.SetActive (!graphHolder.activeSelf&&markersToggle.isOn);

		MeshRenderer cubeRenderer = cube.GetComponent<MeshRenderer> ();
		cubeRenderer.enabled = !cubeRenderer.enabled;
		TrailRenderer cubeLine = cube.GetComponent<TrailRenderer> ();
		cubeLine.enabled = cubeRenderer.enabled && trailToggle.isOn;

		if (system.activeSelf)
		{
			buttonText.text = "View Graphs";

			trailToggle.enabled = true;
			markersToggle.enabled = true;
		}
		else 
		{
			buttonText.text = "View System";

			xPosGraph.enabled = true;
			yPosGraph.enabled = true;

			trailToggle.enabled = false;
			markersToggle.enabled = false;
		}
	}
}
