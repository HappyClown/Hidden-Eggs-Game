using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GlobalVariables : MonoBehaviour 
{
	public static GlobalVariables globVarScript;

	public string previousScene;
	public string currentScene;



	void OnEnable () 
	{
		if (globVarScript == null)
		{
			globVarScript = this;
			DontDestroyOnLoad(this.gameObject);
		} 
		else if (globVarScript != this.gameObject)
		{
			Destroy(this.gameObject);
			return;
		}
	}
	


	void Update () 
	{
		if (currentScene != SceneManager.GetActiveScene().name)
		{
			previousScene = currentScene;
		}

		currentScene = SceneManager.GetActiveScene().name;
	}
}
