using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MonoBehaviour 
{
	private Button button;
	public string sceneName;
	public SceneTapEnabler sceneTapEnaScript;


	void Start () 
	{
		button = this.GetComponent<Button>();
		button.onClick.AddListener(OpenScene);
	}


	void Update ()
	{
		if (sceneTapEnaScript != null && !sceneTapEnaScript.canTapPauseBtn)
		{
			button.interactable = false;
		}
		else
		{
			button.interactable = true;
		}
	}


	public void OpenScene ()
	{
		if (sceneName == GlobalVariables.globVarScript.menuName)
		{
			GlobalVariables.globVarScript.toHub = true;
		}
		SceneFade.SwitchScene(sceneName);
	}
}
