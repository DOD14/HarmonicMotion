using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ToggleInput : MonoBehaviour {


	public Toggle myToggle;

	private InputField thisInputField;

	// Use this for initialization
	void Start () {
		thisInputField = GetComponent<InputField> ();
	}
	
	public void ToggleMe()
	{
		if (myToggle.isOn)
			thisInputField.interactable = false;
		else
			thisInputField.interactable = true;
	}
}
