using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzLvlChangeBtn : MonoBehaviour 
{
	//public KitePuzzEngine kitePuzzEngineScript;
	public LevelSelectionButtons mySelect;
	public Button thisButton;
	public int levelToLoad;
	 

	void Start () {
		thisButton.onClick.AddListener(CamilloProSystem);
	}
	
	// void TryToChangeLevel () 
	// {  
	// 	// Technically dont need to check if: crateScript.curntLvl != levelToLoad && grabItemScript.maxLvl >= levelToLoad.  Because the buttons will un-interactable or the GameObject inactive.
	// 	if (kitePuzzEngineScript.canPlay && kitePuzzEngineScript.chngLvlTimer >= kitePuzzEngineScript.setupLvlWaitTime && kitePuzzEngineScript.curntLvl != levelToLoad && kitePuzzEngineScript.maxLvl >= levelToLoad)
	// 	{ 
	// 		thisButton.interactable = false;
	// 		kitePuzzEngineScript.lvlToLoad = levelToLoad; 
	// 		kitePuzzEngineScript.chngLvlTimer = 0f;
	// 		kitePuzzEngineScript.ChangeLevelSetup();
	// 	}
	// }
	
	void CamilloProSystem () {
		mySelect.ButtonPressed(levelToLoad);
	}
}