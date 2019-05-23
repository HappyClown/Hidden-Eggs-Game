using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteColorFade : MonoBehaviour {
	[Header ("Basic")]
	public SpriteRenderer sprite;
	public float minFlashAlpha, maxFlashAlpha;
	public float flashInDur, flashOutDur;
	[Header ("Info")]
	public bool flashAtMin;
	public bool flashAtMax;
	private bool flashingIn, flashingOut;
	private float t;
	[Header ("Options")]
	public bool autoFlashOut;
	public float autoFlashOutDelay;
	private bool delayToFlashOut;
	private float delayTimer;

	void Update () {
		if (flashingIn) {
			t += Time.deltaTime / flashInDur;
			sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, Mathf.SmoothStep(0f, 1f * maxFlashAlpha, t));
			if (t >= 1f) {
				flashAtMax = true;
				flashingIn = false;
				if(flashAtMin)
				flashAtMin = false;
				if (autoFlashOut) {
					delayToFlashOut = true;
					delayTimer = autoFlashOutDelay;
				}
			}
		}

		if (flashingOut) {
			t += Time.deltaTime / flashOutDur;
			sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, Mathf.SmoothStep(1f * maxFlashAlpha, 0f, t));
			if (t >= 1f) {
				flashingOut = false;
				flashAtMin = true;
				if(flashAtMax) {
					flashAtMax = false;	
				}
			}
		}

		if (delayToFlashOut) {
			delayTimer -= Time.deltaTime;
			if (delayTimer <= 0f) {
				delayToFlashOut = false;
				FlashOut();
			}
		}
	}

	public void FlashIn () {
		if (!this.gameObject.activeSelf) {
			this.gameObject.SetActive(true);
		}
		if (flashingIn == false) {
			flashingOut = false;
			flashingIn = true;
			flashAtMin = false;
		}
	}

	public void FlashOut () {
		if (!flashingOut) {
			flashingIn = false;
			flashingOut = true;
			flashAtMax = false;
		}
	}

	public void ResetAplpha(float value){
		sprite.color = new Color(1f, 1f, 1f, value);
		if (value == maxFlashAlpha || value == 1f) {
			flashAtMax = true;
		}
		if (value == 0) {
			flashAtMin = true;
		}
	}

	// For inspector use.
	public void GetMySpriteRenderer() {
		sprite = this.GetComponent<SpriteRenderer>();
	}
}
