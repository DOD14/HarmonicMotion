using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XMarker : MonoBehaviour {

	public Transform cube;

	private LineRenderer line;
	// Use this for initialization
	void Start () {
		line = GetComponent<LineRenderer> ();
		line.SetPosition (0, transform.position);
		line.SetPosition (1, cube.position);
		
	}
	
	// Update is called once per frame
	void Update () {

		transform.position = new Vector3 (cube.transform.position.x, transform.parent.position.y, 0f);
		line.SetPosition (0, transform.position);
		line.SetPosition (1, cube.position);
	}
}
