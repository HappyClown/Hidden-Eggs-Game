using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenEggAnimEvents : MonoBehaviour {
	public ParticleSystem fallSparkles, landingBurst, flash, shafts, /* glow, */ backSparkle, /* shineSparkle, */ burstSparkles;
	public Animator anim;
	public SpriteRenderer overlaySprite;

	// void Update () {
	// 	if (Input.GetKeyDown("space")) {
	// 		anim.SetTrigger("FallAnim");
	// 	}
	// }

	public void PlayFallSparkles() {
		if (fallSparkles.isPlaying) {
			fallSparkles.Stop();
		}
		else {
			fallSparkles.Play();
		}
	}
	
	public void PlayLandingBurst() {
		landingBurst.gameObject.SetActive(true);
		landingBurst.Play();
	}

	public void PlayFlash() {
		flash.gameObject.SetActive(true);
		flash.Play();
	}

	public void PlayShafts() {
		shafts.gameObject.SetActive(true);
		shafts.Play();
	}

	// public void PlayGlow() {
	// 	glow.gameObject.SetActive(true);
	// 	glow.Play();
	// }

	public void PlayBackSparkles() {
		backSparkle.gameObject.SetActive(true);
		backSparkle.Play();
	}

	// public void PlayShineSparkle() {
	// 	shineSparkle.gameObject.SetActive(true);
	// 	shineSparkle.Play();
	// }

	public void PlayBurstSparkles() {
		burstSparkles.gameObject.SetActive(true);
		burstSparkles.Play();
	}
}
