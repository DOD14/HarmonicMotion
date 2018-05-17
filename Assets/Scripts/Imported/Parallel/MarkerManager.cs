using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerManager : MonoBehaviour {

	public Parallel oscillatorScript;

	public Transform xMarker;
	public Transform yMarker;
	public Transform rMarker;

	public float xheight;
	public float yheight;
	public float rheight;

	private LineRenderer xlineRenderer;
	private LineRenderer ylineRenderer;
	private LineRenderer rlineRenderer;

	// Use this for initialization
	void OnEnable () {
		xlineRenderer = xMarker.GetComponent<LineRenderer>();
		xlineRenderer.SetPosition(0, new Vector3(0f, xheight, 0f));
		xMarker.transform.position = new Vector3 (oscillatorScript.xPos, 0f);
		xlineRenderer.SetPosition(1, xMarker.transform.position);

		ylineRenderer = yMarker.GetComponent<LineRenderer>();
		ylineRenderer.SetPosition(0, new Vector3(0f, yheight, 0f));
		yMarker.transform.position = new Vector3 (oscillatorScript.yPos, 0f);
		ylineRenderer.SetPosition(1, yMarker.transform.position);

		rlineRenderer = rMarker.GetComponent<LineRenderer>();
		rlineRenderer.SetPosition(0, new Vector3(0f, rheight, 0f));
		rMarker.transform.position = new Vector3 (oscillatorScript.transform.position.x, 0f);
		rlineRenderer.SetPosition(1, rMarker.transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		xMarker.transform.position = new Vector3 (oscillatorScript.xPos, xheight);
		xlineRenderer.SetPosition(1, xMarker.transform.position);

		yMarker.transform.position = new Vector3 (oscillatorScript.yPos, yheight);
		ylineRenderer.SetPosition(1, yMarker.transform.position);

		rMarker.transform.position = new Vector3 (oscillatorScript.transform.position.x, rheight);
		rlineRenderer.SetPosition(1, rMarker.transform.position);
	}
}
