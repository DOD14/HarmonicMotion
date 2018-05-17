using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleColor : MonoBehaviour {

	public Color onColor;
	public Color offColor;
	private Toggle toggle;
	private ColorBlock colorBlock;
	// Use this for initialization
	void Awake () {
		toggle = GetComponent<Toggle> ();
		colorBlock = toggle.colors;
	}
	
	// Update is called once per frame
	public void ChangeColor()
	{
		colorBlock.normalColor = toggle.isOn ? onColor : offColor;
		colorBlock.highlightedColor = toggle.isOn ? onColor : offColor;
		colorBlock.pressedColor = toggle.isOn ? onColor : offColor;

		toggle.colors = colorBlock;
	}
}
