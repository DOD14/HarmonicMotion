using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderLineToMe : MonoBehaviour {

    public Transform firstPosition;

    private LineRenderer lineRenderer;

	// Use this for initialization
	void Start () {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, firstPosition.position);
	}
	
	// Update is called once per frame
	void Update () {
        lineRenderer.SetPosition(1, transform.position);

	}
}
