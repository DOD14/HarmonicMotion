﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageMobileInput : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
            if (SceneManager.GetActiveScene().buildIndex == 0)
                Application.Quit();
            else SceneManager.LoadScene(0);
	}
}
