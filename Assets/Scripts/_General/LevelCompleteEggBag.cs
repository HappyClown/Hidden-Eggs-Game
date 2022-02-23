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
	public GameObject eggBagsAnimParent;
	// public List<FadeInOutSprite> eggBags;
	// public List<FadeInOutSprite> bagGlowsScripts;
	public Sprite[] allBagSprites, allBagGlowSprites;
	public SpriteRenderer curEggBagSR, curGlowSR, nextEggBagSR, nextGlowSR;
	public FadeInOutSprite curEggbagFade, curGlowFade, nextEggbagFade, nextGlowFade;
	public Transform curEggBagTrans;

	[Header ("Info")]
	public int levelsCompleted;
	private float newBagTimer, riseLerpTimer;
	private bool newBagOn;
	public bool bagRise;
	private float newY;
	public float iniYPos;

	void Update () {
		if (newBagOn) {
			newBagTimer += Time.deltaTime;
			if (newBagTimer > newBagDelay) {
				curEggbagFade.FadeOut();
				nextEggbagFade.gameObject.SetActive(true);
				nextEggbagFade.FadeIn();
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
		eggBagsAnimParent.SetActive(true);
		GetCurrentReferences();
		curEggbagFade.gameObject.SetActive(true);
		curEggbagFade.FadeIn();
		bagAnim.SetTrigger("ComeIn");
		curGlowFade.gameObject.SetActive(true);
		curGlowFade.maxAlpha = 0.2f;
		curGlowFade.fadeDuration = 1f;
		curGlowFade.FadeIn();
	}

	public void StartCurrentBagGlow() {
		curGlowFade.gameObject.SetActive(true);
		curGlowFade.maxAlpha = 1f;
		curGlowFade.fadeDuration = 5f;
		curGlowFade.FadeIn(0.2f);
	}

	public void MakeNewBagFadeIn() {
		newBagOn = true;
		curEggbagFade.fadeDuration = bagFadeDuration;
		nextEggbagFade.fadeDuration = bagFadeDuration;
	}

	public void MakeNewBagAppear() {
		curEggbagFade.gameObject.SetActive(false);
		nextEggbagFade.gameObject.SetActive(true);
		nextGlowFade.fadeDuration = 0.05f;
		nextGlowFade.FadeIn();
		nextEggbagFade.fadeDuration = 0.05f;
		nextEggbagFade.FadeIn();
	}

	public void SaveLevelsCompleted() {
		GlobalVariables.globVarScript.levelsCompleted = levelsCompleted;
		GlobalVariables.globVarScript.SaveGeneralData();
	}

	void GetCurrentReferences() {
		levelsCompleted = GlobalVariables.globVarScript.levelsCompleted;
		// Assign the current bag's sprites.
		curEggBagSR.sprite = allBagSprites[levelsCompleted];
		curGlowSR.sprite = allBagGlowSprites[levelsCompleted];
		curEggBagTrans = curEggbagFade.transform;
		// Assign the next bag's sprites.
		nextEggBagSR.sprite = allBagSprites[levelsCompleted + 1];
		nextGlowSR.sprite = allBagGlowSprites[levelsCompleted + 1];
	}
}