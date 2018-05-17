using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphPos2X : MonoBehaviour {

	public Oscillator2 oscillatorScript;
	public float height = 4f;

	private Rigidbody rigidbody;
	private float initHeight;
	private Vector3 initPos = new Vector3 (-9f, 0f, 0f);


	// Use this for initialization
	void OnEnable () {
		rigidbody = GetComponent<Rigidbody> ();
		rigidbody.position = initPos;
		rigidbody.velocity = 2f * Vector3.right;
		rigidbody.GetComponent<TrailRenderer> ().Clear ();
		rigidbody.GetComponent<TrailRenderer> ().sortingLayerName = "Front";


		initHeight = transform.parent.position.y;
	}

	// Update is called once per frame
	void FixedUpdate () {
		if(rigidbody.velocity!=Vector3.zero)
			rigidbody.position = new Vector3 (rigidbody.position.x, initHeight + height * oscillatorScript.xPos/oscillatorScript.xAmplitude, 0f);
		if (rigidbody.position.x > 12f) {
			rigidbody.velocity = Vector3.zero;
			this.enabled = false;
		}
	}
}