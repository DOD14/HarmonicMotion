using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverGraph : MonoBehaviour {

	public Oscillator2 oscillatorScript;

	private Rigidbody rigidbody;

	// Use this for initialization
	void OnEnable () {
		rigidbody.GetComponent<TrailRenderer> ().Clear ();
		rigidbody = GetComponent<Rigidbody> ();
		rigidbody.velocity = 2f * Vector3.right;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(rigidbody.velocity!=Vector3.zero)
		rigidbody.position = new Vector3 (rigidbody.position.x, oscillatorScript.xPos, 0f);
		if (rigidbody.position.x > 12f) {
			rigidbody.velocity = Vector3.zero;
			this.enabled = false;
		}
	}
}
