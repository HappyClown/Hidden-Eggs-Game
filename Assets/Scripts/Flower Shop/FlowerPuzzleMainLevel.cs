using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerPuzzleMainLevel : MonoBehaviour {

	public FlowerPuzzleLevel[] mylvls;
	public bool LevelReady, levelComplete, active, subLvlsComplete, switchinglvl;
	public int currentLevel = 0;
	public float transitionTime;
	private float timer;
	// Use this for initialization
	
	void Update(){
		if(LevelReady){
			StartLevel();
			LevelReady = false;
		}
		if(active){
			if(mylvls[currentLevel].levelComplete){
				currentLevel ++;
				if(currentLevel >= mylvls.Length){
					subLvlsComplete = true;
					active = false;
				}
				else{					
					active = false;	
					switchinglvl = true;	
				}
			}
		}else if(subLvlsComplete){
				EndingTransition();
		}else{
			StartLevel(transitionTime);
		}
		
	}
	public void StartLevel(float transTime = 0){
		if(timer >= transTime){
			if(switchinglvl){
				ResetCurrentLevel(currentLevel-1);
				switchinglvl = false;
			}
			mylvls[currentLevel].SetUpLevel();
			active = true;
			timer = 0;
		}else{
			timer+= Time.deltaTime;
		}
	}
	public void ResetLevel(){
		foreach (FlowerPuzzleLevel lvl in mylvls)
		{
			lvl.ResetLevel();
		}
		subLvlsComplete = switchinglvl = levelComplete = false;		
		timer = 0;
	}
	void EndingTransition(){
		if(timer >= transitionTime){
			ResetCurrentLevel(currentLevel-1);
			levelComplete = true;
			timer = 0;
		}else{
			timer+= Time.deltaTime;
		}
	}
	public void ResetCurrentLevel(int lvl){
		mylvls[lvl].ResetLevel();
	}
	public void SetUpLevel(){
		LevelReady = true;
		timer = 0;
	}
	public void CheckLevel(){
		mylvls[currentLevel].CheckLevelComplete();
	}
}
