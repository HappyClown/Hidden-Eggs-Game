using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleteBagAnimEvents : MonoBehaviour {
	[Header ("Settings")]
	public float backShaftsMaxScale;
	public float overlayDur, glowDur;
	[Header ("References")]
	public LevelCompleteEggBag lvlCompEggBagScript;
	public ParticleSystem flashFX, sparkleDustFX, backShaftsFX, popFX, afterSparkleFX;
	[Header ("Info")]
	private float backShaftsNewScale;

	
	void FadeInWhiteOverlay() {
		lvlCompEggBagScript.whiteOverlayScript.FadeIn();
	}

	void FadeOutWhiteOverlay() {
		lvlCompEggBagScript.whiteOverlayScript.fadeDuration = overlayDur;
		lvlCompEggBagScript.whiteOverlayScript.FadeOut();
	}

	void FadeOutBagGlow() {
		lvlCompEggBagScript.curGlowFade.fadeDuration = glowDur;
		lvlCompEggBagScript.curGlowFade.FadeOut();
	}

	void PlayFlashFX() {
		flashFX.gameObject.SetActive(true);
		flashFX.Play();
	}

	void PlaySparkleDustFX() {
		sparkleDustFX.gameObject.SetActive(true);
		sparkleDustFX.Play();
	}

	void PlayPopFX() {
		popFX.gameObject.SetActive(true);
		popFX.Play();
	}

	void PlayAfterSparkleFX() {
		afterSparkleFX.gameObject.SetActive(true);
		afterSparkleFX.Play();
	}

	void PlayBackShaftsFX() {
		backShaftsFX.gameObject.SetActive(true);
		backShaftsNewScale = backShaftsFX.transform.localScale.x + ((backShaftsMaxScale - backShaftsFX.transform.localScale.x) * (lvlCompEggBagScript.levelsCompleted+1) / 13);
		backShaftsFX.transform.localScale = new Vector3(backShaftsNewScale, backShaftsNewScale ,backShaftsNewScale);
		backShaftsFX.Play();
	}

	void NewBag() {
		lvlCompEggBagScript.MakeNewBagAppear();
	}
}
