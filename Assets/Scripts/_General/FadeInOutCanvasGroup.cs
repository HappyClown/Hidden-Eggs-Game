using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOutCanvasGroup : MonoBehaviour {
	[Header("Values")]
	public float fadeDuration;
	private float t;
	public float fadeDelayDur;
	[Range(0f, 1f)]
	public float maxAlpha = 1f;
	[Header("Options")]
	public bool inactiveOnFadeOut;
	public bool fadeOnStart = true;
	public bool fadeDelay;
	public CanvasGroup canvasG;
	[Header("Canvas Group Options")]
	public bool cgOptionsOnFadeStart;
	public bool interactable, blocksRaycasts, ignoreParentGroups;
	[Header("State")]
	public StartState myStartState;
	public enum StartState {
		startShown, startHidden
	}
	public bool fadingOut, fadingIn, hidden, shown;
	private Coroutine activeRoutine;

	void Start () {
		canvasG = this.gameObject.GetComponent<CanvasGroup>();
		if (myStartState == StartState.startShown) {
			canvasG.alpha = 1f;
			shown = true;
		}
		else if (myStartState == StartState.startHidden) {
			canvasG.alpha = 0f;
			hidden = true;
			if (!cgOptionsOnFadeStart) SetCanvasOptions(false);
		}
		if (maxAlpha <= 0f) { 
			maxAlpha = 1f; 
		}
		if (fadeOnStart) { 
			FadeIn();  
			canvasG.alpha = 0f;
		}
	}

	IEnumerator FadingOut () {
		while (fadingOut) {
			t += Time.deltaTime / fadeDuration;
			canvasG.alpha = Mathf.SmoothStep(maxAlpha, 0f, t);
			if (t >= 1f) {
				fadingOut = false;
				hidden = true;
				if(shown)
				shown = false;				
				if(inactiveOnFadeOut) {
					this.gameObject.SetActive(false);
				}
			}
			yield return null;
		}
		activeRoutine = null;
	}

	IEnumerator FadingIn() {
		while (fadingIn) {
			t += Time.deltaTime / fadeDuration;
			canvasG.alpha = Mathf.SmoothStep(0f, maxAlpha, t);
			if (t >= 1f) {
				shown = true;
				fadingIn = false;
				if(hidden)
				hidden = false;
			}
			yield return null;
		}
		SetCanvasOptions(true);
		activeRoutine = null;
	}

	public void FadeOut () {
		if (fadingOut == false) { // Potentially implement a waitmode, to wait until it is faded in/out to fade it in/out.
			fadingIn = false;
			fadingOut = true;
			if(fadeDelay) { t = 0f - fadeDelayDur; }
			else { t = 0f; }
			shown = false;
			SetCanvasOptions(false);
		}
		if (activeRoutine != null) {
			StopCoroutine(activeRoutine);
		}
		activeRoutine = StartCoroutine(FadingOut());
	}

	public void FadeIn () {
		if (!this.gameObject.activeSelf) {
			this.gameObject.SetActive(true);
		}
		if (fadingIn == false) {
			fadingOut = false;
			fadingIn = true;
			if(fadeDelay) { t = 0f - fadeDelayDur; }
			else { t = 0f; }
			hidden = false;
			if (cgOptionsOnFadeStart) {
				SetCanvasOptions(true);
			}
		}
		if (activeRoutine != null) {
				StopCoroutine(activeRoutine);
			}
		activeRoutine = StartCoroutine(FadingIn());
	}

	public void ResetAplpha(float value){
		canvasG.alpha = value;
		if (value == maxAlpha || value == 1f) {
			shown = true;
		}
		if (value == 0) {
			hidden = true;
		}
	}

	void SetCanvasOptions(bool enabled) {
		if (interactable) {
			if (enabled) {
				canvasG.interactable = true;
			}
			else {
				canvasG.interactable = false;
			}
		}
		if (blocksRaycasts) {
			if (enabled) {
				canvasG.blocksRaycasts = true;
			}
			else {
				canvasG.blocksRaycasts = false;
			}
		}
		if (ignoreParentGroups) {
			if (enabled) {
				canvasG.ignoreParentGroups = true;
			}
			else {
				canvasG.ignoreParentGroups = false;
			}
		}
	}
}