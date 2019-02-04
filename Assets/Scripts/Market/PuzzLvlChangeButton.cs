using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzLvlChangeButton : MonoBehaviour 
{
	public MarketPuzzleEngine marketPuzzScript;
	public Button thisButton;
	public int levelToLoad;
	 

	void Start () 
	{
		thisButton.onClick.AddListener(TryToChangeLevel);
	}
	

	void TryToChangeLevel () 
	{  
		// Technically dont need to check if: crateScript.curntLvl != levelToLoad && grabItemScript.maxLvl >= levelToLoad.  Because the buttons will un-interactable or the GameObject inactive.
		if (marketPuzzScript.canPlay && marketPuzzScript.chngLvlTimer >= marketPuzzScript.setupLvlWaitTime && marketPuzzScript.curntLvl != levelToLoad && marketPuzzScript.maxLvl >= levelToLoad)
		{ 
			thisButton.interactable = false;
			marketPuzzScript.lvlToLoad = levelToLoad; 
			marketPuzzScript.chngLvlTimer = 0f;
			marketPuzzScript.ChangeLevelSetup();
		}
	}
}