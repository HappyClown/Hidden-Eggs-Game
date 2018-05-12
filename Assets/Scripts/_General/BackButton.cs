﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour 
{
	private Button button;

	public string sceneName;


	void Start () 
	{
		button = this.GetComponent<Button>();
		button.onClick.AddListener(OpenScene);
	}


	public void OpenScene ()
	{
		SceneManager.LoadScene(sceneName);
	}

}
