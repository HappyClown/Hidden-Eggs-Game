using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleteEggBag : MonoBehaviour {
	[Header("Settings")]
	public float newBagDelay;
	public float bagFadeDuration;
	[Header("References")]
	public Animator bagAnim;
	public FadeInOutSprite whiteOverlayScript;
	public List<FadeInOutSprite> eggBags;
	public List<FadeInOutSprite> bagGlowsScripts;
	public AudioSceneGeneral audioSceneGenScript;
	[Header ("Info")]
	public FadeInOutSprite curGlowScript;
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
	
	public void MakeCurrentBagAppear() {
		levelsCompleted = GlobalVariables.globVarScript.levelsCompleted;
		eggBags[levelsCompleted].gameObject.SetActive(true);
		eggBags[levelsCompleted].FadeIn();
	}

	public void StartCurrentBagGlow() {
	levelsCompleted = GlobalVariables.globVarScript.levelsCompleted;
	curGlowScript = bagGlowsScripts[levelsCompleted];
	curGlowScript.gameObject.SetActive(true);
	curGlowScript.FadeIn();
	}

	public void MakeNewBagFadeIn() {
		newBagOn = true;
		eggBags[levelsCompleted].fadeDuration = bagFadeDuration;
		eggBags[levelsCompleted + 1].fadeDuration = bagFadeDuration;
	}

	public void MakeNewBagAppear() {
		eggBags[levelsCompleted].gameObject.SetActive(false);
		eggBags[levelsCompleted + 1].gameObject.SetActive(true);
		eggBags[levelsCompleted + 1].fadeDuration = 0.05f;
		eggBags[levelsCompleted + 1].FadeIn();
	}

	public void SaveLevelsCompleted() {
		GlobalVariables.globVarScript.levelsCompleted = levelsCompleted;
		GlobalVariables.globVarScript.SaveGeneralData();
	}
}
