using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FadeInOutTMP : MonoBehaviour 
{
	[Header("Values")]
	public float fadeDuration;
	public float fadeDelayDur;
	[Range(0f, 1f)]
	public float maxAlpha = 1f;

	[HideInInspector]
	public bool fadingOut, fadingIn, hidden, shown;
	public float t;

	[Header("Options")]
	public bool inactiveOnFadeOut = true;
	public bool fadeInOnStart = true;
	public bool fadeDelay;
	public enum StartState {
		startShown, startHidden
	}
	public StartState myStartState;
	
	[HideInInspector]
	public TextMeshProUGUI tmp;

	void Start () {
		tmp = this.gameObject.GetComponent<TextMeshProUGUI>();
		if (myStartState == StartState.startShown) {
			tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, 1f);
			if(!fadingIn && !fadingOut)
			shown = true;
		}
		else if (myStartState == StartState.startHidden) {
			tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, 0f);
			if(!fadingIn && !fadingOut)
			hidden = true;
		}
		if (maxAlpha <= 0f) { 
			maxAlpha = 1f; 
		}
		if (fadeInOnStart) { 
			FadeIn();  
			new Color(tmp.color.r, tmp.color.g, tmp.color.b, 0f); 
		}
	}


	void Update () {
		if (fadingOut == true) {
			t += Time.deltaTime / fadeDuration;
			tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, Mathf.SmoothStep(1f * maxAlpha, 0f, t));
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
			tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, Mathf.SmoothStep(0f, 1f * maxAlpha, t));
			if (t >= 1f) {
				fadingIn = false;
				shown = true;
			}
		}
	}


	public void FadeOut() {
		if (fadingOut == false/*  && tmp.color.a >= 0.01f */) { // Potentially implement a waitmode, to wait until it is faded in/out to fade it in/out.
			fadingIn = false;
			fadingOut = true;
			if(fadeDelay) { t = 0f - fadeDelayDur; }
			else { t = 0f; }
			shown = false;
		}
	}


	public void FadeIn() {
		if (!this.gameObject.activeSelf){
			this.gameObject.SetActive(true);
		}
		
		if (fadingIn == false){
			fadingOut = false;
			fadingIn = true;
			if(fadeDelay) { t = 0f - fadeDelayDur; }
			else { t = 0f; }
			hidden = false;
		}
	}

	//pour tests audio
	public bool getFadingOut() {
		return fadingOut;
	}
}