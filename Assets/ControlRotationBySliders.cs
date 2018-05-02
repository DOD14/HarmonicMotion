using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlRotationBySliders : MonoBehaviour {

    public Slider[] sliders;

	private void Start()
	{
        AdjustRotation();
	}
	// Update is called once per frame
	public void AdjustRotation () {

        transform.rotation = Quaternion.Euler(sliders[0].value, sliders[1].value, 0f);
	}
}
