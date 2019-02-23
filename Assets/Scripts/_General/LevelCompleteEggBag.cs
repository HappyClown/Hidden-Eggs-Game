using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleteEggBag : MonoBehaviour {
	public int levelsCompleted;
	public List<FadeInOutSprite> eggBags;
	private float newBagTimer;
	private bool newBagOn;
	public float newBagDelay;
	private bool endLevel;
	private float endLevelTimer;
	public float endLevelAfterBag;
	public ClickOnEggs clickOnEggsScript;

	void Update () {
		if (newBagOn) {
			newBagTimer += Time.deltaTime;
			if (newBagTimer > newBagDelay) {
				eggBags[levelsCompleted].FadeOut();
				eggBags[levelsCompleted + 1].gameObject.SetActive(true);
				eggBags[levelsCompleted + 1].FadeIn();
				newBagOn = false;
				endLevel = true;
			}
		}
		if (endLevel) {
			endLevelTimer += Time.deltaTime;
			if (endLevelTimer > endLevelAfterBag) {
				endLevel = false;
				endLevelTimer = 0f;
				clickOnEggsScript.levelComplete = true;
				clickOnEggsScript.SaveLevelComplete();
				levelsCompleted++;
				SaveLevelsCompleted();
				GlobalVariables.globVarScript.toHub = true;
				SceneFade.SwitchSceneWhiteFade(GlobalVariables.globVarScript.menuName);
			}
		} 
	}
	
	public void MakeFirstBagAppear() {
		levelsCompleted = GlobalVariables.globVarScript.levelsCompleted;
		eggBags[levelsCompleted].gameObject.SetActive(true);
		eggBags[levelsCompleted].FadeIn();
	}

	public void MakeNewBagAppear() {
		newBagOn = true;
	}

	public void SaveLevelsCompleted() {
		GlobalVariables.globVarScript.levelsCompleted = levelsCompleted;
		GlobalVariables.globVarScript.SaveEggState();
	}
}
