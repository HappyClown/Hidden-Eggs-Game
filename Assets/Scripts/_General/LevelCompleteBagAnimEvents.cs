using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleteBagAnimEvents : MonoBehaviour {
	[Header ("Settings")]
	public float backShaftsMaxScale;
	public float overlayDur, glowDur;
	[Header ("References")]
	public LevelCompleteEggBag lvlCompEggBagScript;
	public ParticleSystem flashFX, sparkleDustFX, backShaftsFX;
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
		lvlCompEggBagScript.curGlowScript.fadeDuration = glowDur;
		lvlCompEggBagScript.curGlowScript.FadeOut();
	}

	void PlayFlashFX() {
		flashFX.Play();
	}

	void PlaySparkleDustFX() {
		sparkleDustFX.Play();
	}

	void PlayBackShaftsFX() {
		backShaftsNewScale = backShaftsFX.transform.localScale.x + ((backShaftsMaxScale - backShaftsFX.transform.localScale.x) * (lvlCompEggBagScript.levelsCompleted+1) / 13);
		backShaftsFX.transform.localScale = new Vector3(backShaftsNewScale, backShaftsNewScale ,backShaftsNewScale);
		backShaftsFX.Play();
	}

	void NewBag() {
		lvlCompEggBagScript.MakeNewBagAppear();
	}
}
