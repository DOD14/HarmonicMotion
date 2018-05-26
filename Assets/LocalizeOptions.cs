using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizeOptions : MonoBehaviour {

    public string[] myKeys;

    private Dropdown myDropdown;

	// Use this for initialization
	void Start () 
    {
        myDropdown = GetComponent<Dropdown>();

        for (int i = 0; i < myDropdown.options.Count; i++)
        {
            myDropdown.options[i].text = LocalizationManager.instance.GetLocalizedValue(myKeys[i]);
        }
    }

}
