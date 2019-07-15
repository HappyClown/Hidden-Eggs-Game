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
	public int levelsCompleted;
	public FadeInOutSprite curGlowFadeScript, curEggbagFadeScript, nextEggbagFadeScript;
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
				curEggbagFadeScript.FadeOut();
				nextEggbagFadeScript.gameObject.SetActive(true);
				nextEggbagFadeScript.FadeIn();
				newBagOn = false;
			}
		}
	}
	
	public void MakeCurrentBagAppear() {
		levelsCompleted = GlobalVariables.globVarScript.levelsCompleted;
		// Assign local references
		curEggbagFadeScript = eggBags[levelsCompleted];
		nextEggbagFadeScript = eggBags[levelsCompleted + 1];
		curEggbagFadeScript.gameObject.SetActive(true);
		curEggbagFadeScript.FadeIn();
	}

	public void StartCurrentBagGlow() {
		// Assign local references
		levelsCompleted = GlobalVariables.globVarScript.levelsCompleted;
		curGlowFadeScript = bagGlowsScripts[levelsCompleted];
		curGlowFadeScript.gameObject.SetActive(true);
		curGlowFadeScript.FadeIn();
	}

	public void MakeNewBagFadeIn() {
		newBagOn = true;
		curEggbagFadeScript.fadeDuration = bagFadeDuration;
		nextEggbagFadeScript.fadeDuration = bagFadeDuration;
	}

	public void MakeNewBagAppear() {
		curEggbagFadeScript.gameObject.SetActive(false);
		nextEggbagFadeScript.gameObject.SetActive(true);
		nextEggbagFadeScript.fadeDuration = 0.05f;
		nextEggbagFadeScript.FadeIn();
	}

	public void SaveLevelsCompleted() {
		GlobalVariables.globVarScript.levelsCompleted = levelsCompleted;
		GlobalVariables.globVarScript.SaveGeneralData();
	}
}
