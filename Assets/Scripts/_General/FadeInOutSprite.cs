using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOutSprite : MonoBehaviour {
	//[HideInInspector]
	public bool fadingOut, fadingIn, hidden, shown;
	private float t, iniVal;
	public float fadeDelayDur;
	[Range(0f, 1f)]
	public float maxAlpha = 1f;
	public float fadeDuration;
	public bool fadeOnStart = true;
	public bool fadeDelay, disableOnFadeOut;
	public SpriteRenderer sprite;
	public enum StartState {
		startShown, startHidden
	}
	public StartState myStartState;

	void Start () {
		//sprite = this.gameObject.GetComponent<SpriteRenderer>();
		if (myStartState == StartState.startShown) {
			sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, sprite.color.a);
			shown = true;
		}
		else if (myStartState == StartState.startHidden) {
			sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0f);
			hidden = true;
		}
		if (maxAlpha <= 0f) { 
			maxAlpha = 1f; 
		}
		if (fadeOnStart) { 
			FadeIn();  
			new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0f); 
		}
	}

	void Update () {
		if (fadingOut == true) {
			t += Time.deltaTime / fadeDuration;
			sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, Mathf.SmoothStep(maxAlpha, iniVal, t));
			if (t >= 1f) {
				fadingOut = false;
				hidden = true;
				if(shown)
				shown = false;				
				if(disableOnFadeOut) {
					this.gameObject.SetActive(false);
				}
			}
		}

		if (fadingIn == true) {
			t += Time.deltaTime / fadeDuration;
			sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, Mathf.SmoothStep(iniVal, maxAlpha, t));
			if (t >= 1f) {
				shown = true;
				fadingIn = false;
				if(hidden)
				hidden = false;
			}
		}
	}

	public void FadeOut (float startVal = 0f) {
		if (fadingOut == false) { // Potentially implement a waitmode, to wait until it is faded in/out to fade it in/out.
			iniVal = startVal;
			fadingIn = false;
			fadingOut = true;
			if(fadeDelay) { t = 0f - fadeDelayDur; }
			else { t = 0f; }
			shown = false;
		}
	}

	public void FadeIn (float startVal = 0f) {
		if (!this.gameObject.activeSelf) {
			this.gameObject.SetActive(true);
		}
		if (fadingIn == false) {
			iniVal = startVal;
			fadingOut = false;
			fadingIn = true;
			if(fadeDelay) { t = 0f - fadeDelayDur; }
			else { t = 0f; }
			hidden = false;
		}
	}
	public void ResetAlpha(float value){
		sprite.color = new Color(1f, 1f, 1f, value);
		if (value == maxAlpha || value == 1f) {
			shown = true;
		}
		if (value == 0) {
			hidden = true;
		}
	}

	// For Editor script
	public void GetSpriteRef() {
		sprite = this.gameObject.GetComponent<SpriteRenderer>();
	}
}