using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class LoadSceneButton : MonoBehaviour 
{
	public Button chngScnButton;
	public string scene;

	void Start () 
	{
		chngScnButton = this.GetComponent<Button>();
		chngScnButton.onClick.AddListener(LoadScene);
	}
	
	public void LoadScene () 
	{
		SceneManager.LoadScene(scene);
	}
}
