using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleGraph : MonoBehaviour {


	public GameObject graphUIHolder;
	public GameObject graphHolder;
	public GameObject system;
	public GameObject cube;
	public GameObject phasorHolder;

	public MoverEnergyGraph energyScript;
	public SimplePosGraph posGraph;

	public Text buttonText;
	public Toggle phasorToggle;
	public Toggle trailToggle;

	public void ToggleGraphs()
	{
		graphUIHolder.SetActive (!graphUIHolder.activeSelf);
		graphHolder.SetActive (!graphHolder.activeSelf);
		system.SetActive (!system.activeSelf);

		MeshRenderer cubeRenderer = cube.GetComponent<MeshRenderer> ();
		cubeRenderer.enabled = !cubeRenderer.enabled;
		LineRenderer cubeLine = cube.GetComponent<LineRenderer> ();
		cubeLine.enabled = !cubeLine.enabled;

		if (system.activeSelf) {
			buttonText.text = "View Graphs";
			phasorHolder.SetActive (phasorToggle.isOn);
			cube.GetComponent<Oscillator> ().ResetTime ();
			if(trailToggle.isOn) cube.GetComponent<ParticleSystem> ().Play ();
			trailToggle.enabled = true;
			phasorToggle.enabled = true;

		} else {
			buttonText.text = "View System";

			energyScript.enabled = true;
			posGraph.enabled = true;
			phasorHolder.SetActive (false);
			trailToggle.enabled = false;
			phasorToggle.enabled = false;

			cube.GetComponent<ParticleSystem> ().Stop ();
			cube.GetComponent<ParticleSystem> ().Clear ();
		}
	}
}
