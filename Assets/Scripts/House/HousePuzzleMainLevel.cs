using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HousePuzzleMainLevel : MonoBehaviour {

	public HousePuzzleLevel[] mylvls;
	public PuzzleCell[] allCells;
	public bool LevelReady, levelComplete;
	public int currentLevel = 0;
	// Use this for initialization
	
	void Update(){
		if(LevelReady){
			StartLevel();
			LevelReady = false;
		}
		if(!levelComplete){
			if(mylvls[currentLevel].levelComplete){
				ResetCurrentLevel();
				currentLevel ++;
				if(currentLevel >= mylvls.Length){
					levelComplete = true;
					currentLevel = 0;
				}
				else{
					StartLevel();				
				}
			}
		}
	}
	public void StartLevel(){
		mylvls[currentLevel].SetUpLevel();
	}
	public void ResetLevel(){
		foreach (HousePuzzleLevel lvl in mylvls)
		{
			lvl.ResetLevel();
		}
		levelComplete = false;
	}	
	public void ResetCurrentLevel(){
		mylvls[currentLevel].ResetLevel();
	}
	public void SetUpLevel(){
		LevelReady = true;
	}
}
