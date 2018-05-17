using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphComponent : MonoBehaviour {

	public Parallel oscillatorScript;

	public Rigidbody xMover;
	public Rigidbody yMover;
	public Rigidbody rMover;

	private float initHeight;

	private Vector3 initPos = new Vector3 (-10f, 0f, 0f);


	// Use this for initialization
	void OnEnable () {
		xMover.position = initPos;
		xMover.velocity = 2f * Vector3.right;
		xMover.GetComponent<TrailRenderer> ().Clear ();

		yMover.position = initPos;
		yMover.velocity = 2f * Vector3.right;
		yMover.GetComponent<TrailRenderer> ().Clear ();
		yMover.GetComponent<TrailRenderer> ().sortingLayerName = "Middle";


		rMover.position = initPos;
		rMover.velocity = 2f * Vector3.right;
		rMover.GetComponent<TrailRenderer> ().Clear ();
		rMover.GetComponent<TrailRenderer> ().sortingLayerName = "Front";

		initHeight = transform.position.y;
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (rMover.velocity != Vector3.zero) {
			xMover.position = new Vector3 (xMover.position.x, initHeight + 8f * oscillatorScript.xPos / oscillatorScript.fullAmplitude, 0f);
			yMover.position = new Vector3 (yMover.position.x, initHeight + 8f * oscillatorScript.yPos / oscillatorScript.fullAmplitude, 0f);
			rMover.position = new Vector3 (rMover.position.x, initHeight + 8f * oscillatorScript.position / oscillatorScript.fullAmplitude, 0f);

		}
		if (rMover.position.x > 12f) {
			xMover.velocity = Vector3.zero;
			yMover.velocity = Vector3.zero;
			rMover.velocity = Vector3.zero;

			this.enabled = false;
		}
	}
}