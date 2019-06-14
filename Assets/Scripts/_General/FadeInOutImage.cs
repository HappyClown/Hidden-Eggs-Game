using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOutImage : MonoBehaviour 
{
	[Header("Values")]
	public float fadeDuration;
	public float fadeDelayDur;
	[Range(0f, 1f)]
	public float maxAlpha = 1f;

	[Header("Options")]
	public bool inactiveOnFadeOut = true;
	public bool fadeInOnStart = true;
	public bool fadeDelay;
	//[Header("Eyes only 	٩(｡•́‿•̀｡)۶")]
	public Image img;
	//[HideInInspector]
	[Header("State")]
	public StartState myStartState;
	public bool fadingOut, fadingIn, hidden, shown;
	//[HideInInspector]
	public float t;
	public enum StartState {
		startShown, startHidden
	}

	void Start () {
		img = this.gameObject.GetComponent<Image>();
		if (myStartState == StartState.startShown) {
			img.color = new Color(img.color.r, img.color.g, img.color.b, maxAlpha);
			if(!fadingOut && !fadingIn)
			shown = true;
		}
		else if (myStartState == StartState.startHidden) {
			img.color = new Color(img.color.r, img.color.g, img.color.b, 0f);
			if(!fadingOut && !fadingIn)
			hidden = true;
		}
		if (maxAlpha <= 0f) { 
			maxAlpha = 1f; 
		}
		if (fadeInOnStart) {  
			FadeIn();  
		}
	}

	void Update () {
		if (fadingOut == true) {
			t += Time.deltaTime / fadeDuration;
			img.color = new Color(img.color.r, img.color.g, img.color.b, Mathf.SmoothStep(1f * maxAlpha, 0f, t));
			if (t >= 1f) {
				if (inactiveOnFadeOut) {
					this.gameObject.SetActive(false);
				}
				fadingOut = false;
				hidden = true;
			}
		}

		if (fadingIn == true) {
			t += Time.deltaTime / fadeDuration;
			img.color = new Color(img.color.r, img.color.g, img.color.b, Mathf.SmoothStep(0f, 1f * maxAlpha, t));
			if (t >= 1f) {
				shown = true;
				fadingIn = false;
				hidden = false;
			}
		}
	}

	public void FadeOut () {
		if (!fadingOut && !hidden/*  && img.color.a >= 0.01f */) { // Potentially implement a waitmode, to wait until it is faded in/out to fade it in/out.
			fadingIn = false;
			fadingOut = true;
			if(fadeDelay) { t = 0f - fadeDelayDur; }
			else { t = 0f; }
			shown = false;
		}
	}

	public void FadeIn () {
		if (!this.gameObject.activeSelf) {
			this.gameObject.SetActive(true);
		}
		
		if (!fadingIn && !shown) {
			fadingOut = false;
			fadingIn = true;
			if(fadeDelay) { t = 0f - fadeDelayDur; }
			else { t = 0f; }
			hidden = false;
		}
	}
}