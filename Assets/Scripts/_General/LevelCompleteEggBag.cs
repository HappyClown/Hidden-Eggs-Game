using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleteEggBag : MonoBehaviour {
	[Header("Settings")]
	public float newBagDelay;
	public float bagFadeDuration;
	public float riseMaxY;
	public float riseDur;
	[Header("References")]
	public Animator bagAnim;
	public FadeInOutSprite whiteOverlayScript;
	public List<FadeInOutSprite> eggBags;
	public List<FadeInOutSprite> bagGlowsScripts;

	[Header ("Info")]
	public int levelsCompleted;
	public FadeInOutSprite curGlowFadeScript, curEggbagFadeScript, nextEggbagFadeScript, nextGlowFadeScript;
	public Transform curEggBagTrans;
	private float newBagTimer, riseLerpTimer;
	private bool newBagOn;
	public bool bagRise;
	private float newY;
	public float iniYPos;
	
	public void Start() {
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
		if (bagRise) {
			riseLerpTimer += Time.deltaTime / riseDur;
			newY = Mathf.Lerp(iniYPos, riseMaxY, riseLerpTimer);
			curEggBagTrans.position = new Vector3(curEggBagTrans.position.x, newY, curEggBagTrans.position.z);
			if (riseLerpTimer >= 1f) {
				bagRise = false;
				riseLerpTimer = 0f;
			}
		}
	}
	// Make first bag appear.
	public void MakeCurrentBagAppear() {
		GetCurrentReferences();
		curEggbagFadeScript.gameObject.SetActive(true);
		curEggbagFadeScript.FadeIn();
		bagAnim.SetTrigger("ComeIn");
		curGlowFadeScript.gameObject.SetActive(true);
		curGlowFadeScript.maxAlpha = 0.2f;
		curGlowFadeScript.fadeDuration = 1f;
		curGlowFadeScript.FadeIn();
	}

	public void StartCurrentBagGlow() {
		curGlowFadeScript.gameObject.SetActive(true);
		curGlowFadeScript.maxAlpha = 1f;
		curGlowFadeScript.fadeDuration = 5f;
		curGlowFadeScript.FadeIn(0.2f);
	}

	public void MakeNewBagFadeIn() {
		newBagOn = true;
		curEggbagFadeScript.fadeDuration = bagFadeDuration;
		nextEggbagFadeScript.fadeDuration = bagFadeDuration;
	}

	public void MakeNewBagAppear() {
		curEggbagFadeScript.gameObject.SetActive(false);
		nextEggbagFadeScript.gameObject.SetActive(true);
		nextGlowFadeScript.fadeDuration = 0.05f;
		nextGlowFadeScript.FadeIn();
		nextEggbagFadeScript.fadeDuration = 0.05f;
		nextEggbagFadeScript.FadeIn();
	}

	public void SaveLevelsCompleted() {
		GlobalVariables.globVarScript.levelsCompleted = levelsCompleted;
		GlobalVariables.globVarScript.SaveGeneralData();
	}

	void GetCurrentReferences() {
		levelsCompleted = GlobalVariables.globVarScript.levelsCompleted;
		// Assign local cur bag references
		curEggbagFadeScript = eggBags[levelsCompleted];
		nextEggbagFadeScript = eggBags[levelsCompleted + 1];
		curEggBagTrans = curEggbagFadeScript.transform;
		// Assign local cur glow references
		curGlowFadeScript = bagGlowsScripts[levelsCompleted];
		nextGlowFadeScript = bagGlowsScripts[levelsCompleted + 1];
	}
}
