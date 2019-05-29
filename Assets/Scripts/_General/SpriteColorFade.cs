using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteColorFade : MonoBehaviour {
	[Header ("Basic")]
	public Material colorFlashMat;
	public float minFlashAlpha, maxFlashAlpha;
	public float flashInDur, flashOutDur;
	public AnimationCurve animCurve;
	//private MaterialPropertyBlock _matPropBLock;
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
			//colorFlashMat.color = new Color(colorFlashMat.color.r, colorFlashMat.color.g, colorFlashMat.color.b, Mathf.SmoothStep(0f, 1f * maxFlashAlpha, t));
			colorFlashMat.SetFloat("_FlashAmount", Mathf.SmoothStep(0, maxFlashAlpha, animCurve.Evaluate(t)));
			if (t >= 1f) {
				t = 0f;
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
			//colorFlashMat.color = new Color(colorFlashMat.color.r, colorFlashMat.color.g, colorFlashMat.color.b, Mathf.SmoothStep(1f * maxFlashAlpha, 0f, t));
			colorFlashMat.SetFloat("_FlashAmount", Mathf.SmoothStep(maxFlashAlpha, 0, animCurve.Evaluate(t)));
			if (t >= 1f) {
				t = 0f;
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
		t = 0f;
		colorFlashMat.color = new Color(1f, 1f, 1f, value);
		if (value == maxFlashAlpha || value == 1f) {
			flashAtMax = true;
		}
		if (value == minFlashAlpha || value == 0) {
			flashAtMin = true;
		}
	}

	// For inspector use.
	public void GetMaterialRef() {
		//var tempMat = new Material(renderer.sharedMaterial);
		colorFlashMat = null;
		colorFlashMat = new Material(this.gameObject.GetComponent<SpriteRenderer>().sharedMaterial);
		this.gameObject.GetComponent<SpriteRenderer>().sharedMaterial = colorFlashMat;
		//_matPropBLock = new MaterialPropertyBlock();
	}
}
