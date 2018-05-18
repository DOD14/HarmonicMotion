using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimescalerSlider : MonoBehaviour {

	private Slider slider;

	// Use this for initialization
	void Start () {
		slider = GetComponent<Slider> ();
	}
	
	// Update is called once per frame
	public void TimeScaler()
	{
		Time.timeScale = slider.value;
		Time.fixedDeltaTime = 0.02f * Time.timeScale;
	}
}
