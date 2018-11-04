using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzLvlChangeButton : MonoBehaviour 
{
	public GrabItem grabItemScript;
	public Crate crateScript;
	public Button thisButton;
	public int levelToLoad;
	 

	void Start () 
	{
		thisButton.onClick.AddListener(TryToChangeLevel);
	}
	

	void TryToChangeLevel () 
	{  
		// Technically dont need to check if: crateScript.curntLvl != levelToLoad && grabItemScript.maxLvl >= levelToLoad.  Because the buttons will un-interactable or the GameObject inactive.
		if (grabItemScript.canPlay && grabItemScript.chngLvlTimer >= grabItemScript.setupLvlWaitTime && crateScript.curntLvl != levelToLoad && grabItemScript.maxLvl >= levelToLoad)
		{ 
			thisButton.interactable = false;
			grabItemScript.lvlToLoad = levelToLoad; 
			grabItemScript.chngLvlTimer = 0f;
			grabItemScript.ChangeLevelSetup();
		}
	}
}