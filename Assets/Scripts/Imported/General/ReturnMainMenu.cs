using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnMainMenu : MonoBehaviour {

	public GameObject loadingImage;

	public void ReturnToMainMenu()
	{
		loadingImage.SetActive(true);
		SceneManager.LoadScene(0);
	}

	//to do: input panel text sizes, phase times PI
}
