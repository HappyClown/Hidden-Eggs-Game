using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenEggAnimEvents : MonoBehaviour {
	public ParticleSystem fallSparkles, landingBurst, flash, shafts, glow, backSparkle, shineSparkle, burstSparkles;
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
		landingBurst.Play();
	}

	public void PlayFlash() {
		flash.Play();
	}

	public void PlayShafts() {
		shafts.Play();
	}

	public void PlayGlow() {
		glow.Play();
	}

	public void PlayBackSparkles() {
		backSparkle.Play();
	}

	public void PlayShineSparkle() {
		shineSparkle.Play();
	}

	public void PlayBurstSparkles() {
		burstSparkles.Play();
	}
}
