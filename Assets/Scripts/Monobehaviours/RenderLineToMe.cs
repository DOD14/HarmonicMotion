using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderLineToMe : MonoBehaviour {

    public Transform firstPosition;
    public Vector3 firstPosV3;
    public bool useTransform = true;
    public bool dynamic=false;

    private LineRenderer lineRenderer;

	// Use this for initialization
	void Awake () {
        lineRenderer = GetComponent<LineRenderer>();

        if(useTransform)
        lineRenderer.SetPosition(0, firstPosition.position);
        else 
            lineRenderer.SetPosition(0, firstPosV3);

	}
	
	// Update is called once per frame
	void Update () {
        if (dynamic)
            lineRenderer.SetPosition(0, firstPosition.position);
        lineRenderer.SetPosition(1, transform.position);

	}
}
