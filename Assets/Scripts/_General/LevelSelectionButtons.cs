using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionButtons : MonoBehaviour {

	[Header("Level Selection Buttons")]
	[Tooltip("GameObjects - The level selection buttons, in ascending order. ( 1 - 0, 2 - 1, etc.")] 
	public GameObject[] lvlSelectButtons;
	[Tooltip("Scripts - The FadeInOutImage scripts, in ascending order. ( 1 - 0, 2 - 1, etc.")] 
	public FadeInOutImage[] lvlSelectFades;
	[Tooltip("Scripts - The Scaler scripts, in ascending order. ( 1 - 0, 2 - 1, etc.")] 
	public Scaler[] lvlSelectScalers;
	public bool noFadeDelay;
	public bool buttonsOff;
	public bool buttonPressed;
	public int lvlToLoad;

	 /// <summary>
     /// Enable the level button Dots.
     /// </summary>
     /// <param name="maxLvl">the max level in the puzzle</param>
     /// <returns>Activate the level selection dots according with the max level number.</returns>
	public void EnabledThreeDots(int maxLvl)
	{
		if (maxLvl == 0) { lvlSelectButtons[0].SetActive(true); }
		for(int i = 0; i < maxLvl && i < lvlSelectButtons.Length; i++)
		{ 
			if (!lvlSelectButtons[i].activeSelf) 
			{ lvlSelectButtons[i].SetActive(true); } 
		}
	}

	/// <summary>
    /// Define  Which of the three dots can be interacted with, also adjust the scale accordingly.
    /// </summary>
    /// <param name="maxLvl">the max level in the puzzle</param>
	/// <param name="curntLvl">Current level</param>
	public void InteractableThreeDots(int maxLvl, int curntLvl)
	{
		if (maxLvl == 0) { lvlSelectScalers[0].ScaleUp(); }
		for (int i = 0; i < maxLvl && i < lvlSelectButtons.Length; i++)
		{
			if (lvlSelectButtons[i] == lvlSelectButtons[curntLvl - 1])
			{
				lvlSelectButtons[i].GetComponent<Button>().interactable = false;
				lvlSelectScalers[i].ScaleUp();
			}
			else 
			{
				lvlSelectButtons[i].GetComponent<Button>().interactable = true; 
				lvlSelectScalers[i].ScaleDown();
			}
		}
	}
 	/// <summary>
    /// Make level select buttons uninteractable between levels.
    /// </summary>
    /// <param name="maxLvl">the max level in the puzzle</param>
	public void UninteractableThreeDots()
	{
		foreach(GameObject lvlButton in lvlSelectButtons)
		{
			if (lvlButton.activeSelf) { lvlButton.GetComponent<Button>().interactable = false; }	
		}
	}
	public void TurnFadeDelayOff()
	{
		foreach(FadeInOutImage fadeImgScpt in lvlSelectFades)
		{ fadeImgScpt.fadeDelay = false; }
	}
	public void ButtonPressed(int ButnNum){
		buttonPressed = true;
		lvlToLoad = ButnNum;
	}
}
