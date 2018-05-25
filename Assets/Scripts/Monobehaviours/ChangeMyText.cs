using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeMyText : MonoBehaviour {
    
    private bool playing = false;
    public Text myText;

	public void ChangeText()
    {
        playing = !playing;


        if (playing) myText.text = "Stop";
        else myText.text = "Play";
    }
}
