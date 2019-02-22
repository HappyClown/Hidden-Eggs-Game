using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleteEggBag : MonoBehaviour {
	public int levelsCompleted;
	public List<FadeInOutSprite> eggBags;
	private float newBagTimer;
	private bool newBagOn;
	public float newBagDelay;

	void Update () {
		if (newBagOn) {
			newBagTimer += Time.deltaTime;
			if (newBagTimer > newBagDelay) {
				eggBags[levelsCompleted].FadeOut();
				eggBags[levelsCompleted + 1].gameObject.SetActive(true);
				eggBags[levelsCompleted + 1].FadeIn();
				newBagOn = false;
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
