using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour 
{
	public Button btn01;
	public Button btn02;

	public string btn01SceneName;
	public string btn02SceneName;



	void Start () 
	{
		if (btn01) { btn01.onClick.AddListener(OpenScene01); }
		if (btn02) { btn02.onClick.AddListener(OpenScene02); }
	}
	


	public void OpenScene01 () 
	{
		SceneManager.LoadScene(btn01SceneName);
	}

	public void OpenScene02 () 
	{
		SceneManager.LoadScene(btn02SceneName);
	}
}
