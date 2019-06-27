using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleteEggBag : MonoBehaviour {
	[Header("Settings")]
	public float newBagDelay;
	public float bagFadeDuration;
	[Header("References")]
	public List<FadeInOutSprite> eggBags;
	public AudioSceneGeneral audioSceneGenScript;
	[Header ("Info")]
	public int levelsCompleted;
	private float newBagTimer;
	private bool newBagOn;
	
	public void Start() {
		if (!audioSceneGenScript) {
			audioSceneGenScript = GameObject.FindWithTag("Audio").GetComponent<AudioSceneGeneral>();
		}
	}

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
		eggBags[levelsCompleted].fadeDuration = bagFadeDuration;
		eggBags[levelsCompleted + 1].fadeDuration = bagFadeDuration;
	}

	public void SaveLevelsCompleted() {
		GlobalVariables.globVarScript.levelsCompleted = levelsCompleted;
		GlobalVariables.globVarScript.SaveGeneralData();
	}
}
