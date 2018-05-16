using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverEnergyGraph : MonoBehaviour {

	public Oscillator oscillatorScript;

	public Rigidbody potentialMover;
	public Rigidbody kineticMover;
	public Rigidbody totalMover;

	private Vector3 initPos = new Vector3 (-9f, 0f, 0f);

	private float initHeight;

	// Use this for initialization
	void OnEnable () {
		potentialMover.GetComponent<TrailRenderer> ().Clear ();
		kineticMover.GetComponent<TrailRenderer> ().Clear ();
		totalMover.GetComponent<TrailRenderer> ().Clear ();

		potentialMover.position = initPos;
		kineticMover.position = initPos;
		totalMover.position = initPos;

		potentialMover.velocity = 2f * Vector3.right;
		kineticMover.velocity = 2f * Vector3.right;
		totalMover.velocity = 2f * Vector3.right;

		initHeight = transform.position.y;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (potentialMover.velocity != Vector3.zero) {
			potentialMover.position = new Vector3 (potentialMover.position.x, initHeight + 4f * oscillatorScript.Ep / oscillatorScript.Et, 0f);
			kineticMover.position = new Vector3 (kineticMover.position.x, initHeight + 4f * (oscillatorScript.Ec / oscillatorScript.Et), 0f);
			totalMover.position = new Vector3 (totalMover.position.x, initHeight + 4f, 0f);
		}
		if (totalMover.position.x > 12f) {
			potentialMover.velocity = Vector3.zero;
			kineticMover.velocity = Vector3.zero;
			totalMover.velocity = Vector3.zero;
			this.enabled = false;

		}
	}
		
}
