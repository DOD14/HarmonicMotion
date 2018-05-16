using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateBySlider : MonoBehaviour {

	public Transform rotateMe;
	public float step = 5f;

	private Slider slider;

	void Start()
	{
		slider = GetComponent<Slider> ();
	}

	public void ChangeRotation()
	{
		rotateMe.eulerAngles = new Vector3 (-90f * slider.value, 0f, 0f);
	}
}
