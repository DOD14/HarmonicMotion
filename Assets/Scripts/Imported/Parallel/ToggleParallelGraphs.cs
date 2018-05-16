using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleParallelGraphs : MonoBehaviour {


	public GameObject graphUIHolder;
	public GameObject graphHolder;
	public GameObject system;
	public GameObject cube;
	public GameObject markerHolder;
	public GameObject phasorHolder;

	public GraphComponent graphScript;

	public Text buttonText;

	public Toggle trailToggle;
	public Toggle markersToggle;
	public Toggle phasorToggle;

	public void ToggleGraphs()
	{
		graphUIHolder.SetActive (!graphUIHolder.activeSelf);
		graphHolder.SetActive (!graphHolder.activeSelf);
		system.SetActive (!system.activeSelf);
		markerHolder.SetActive (!graphHolder.activeSelf && markersToggle.isOn);

		MeshRenderer cubeRenderer = cube.GetComponent<MeshRenderer> ();
		cubeRenderer.enabled = !cubeRenderer.enabled;
		LineRenderer cubeLine = cube.GetComponent<LineRenderer> ();
		cubeLine.enabled = cubeRenderer.enabled;

		if (system.activeSelf)
		{
			buttonText.text = "View Graphs";

			trailToggle.enabled = true;
			markersToggle.enabled = true;
			phasorToggle.enabled = true;

			if(trailToggle.isOn) cube.GetComponent<ParticleSystem> ().Play();

			phasorHolder.SetActive (phasorToggle.isOn);

		}
		else 
		{
			buttonText.text = "View System";

			graphScript.enabled = true;

			trailToggle.enabled = false;
			markersToggle.enabled = false;
			phasorToggle.enabled = false;

			phasorHolder.SetActive (false);

			cube.GetComponent<ParticleSystem> ().Stop();
			cube.GetComponent<ParticleSystem> ().Clear ();
			//cube.GetComponent<Parallel>().
		}
	}
}
