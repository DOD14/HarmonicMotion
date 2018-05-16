using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderLineBetween : MonoBehaviour {
	
	public Vector3 lineOrigin = new Vector3 (0f, 0f, 0f);

	private LineRenderer lineRenderer;
	// Use this for initialization
	void Awake () {
		lineRenderer = GetComponent<LineRenderer> ();
		lineRenderer.SetPosition (0, lineOrigin);
		lineRenderer.SetPosition (1, transform.position);

	}
	
	// Update is called once per frame
	void Update () {
		lineRenderer.SetPosition (1, transform.position);
	}
}
