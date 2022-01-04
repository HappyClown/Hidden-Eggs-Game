using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HelpIntroText : MonoBehaviour {
	[Header ("Intro Text")]
	public List<FadeInOutTMP> introTMPs;
	public GameObject nextBtnObj;
	private int sentenceCount;
	public int maxSentence;
	public List<Button> helpBirdBtns;
	private bool inTxtTransition, introOn, canGoNext;
	private float fadeOutDur;
	//private WaitForSeconds waitForSeconds;
	[Header ("Script References")]
	public BirdIntroSave birdIntroSaveScript;
	public SlideInHelpBird slideInHelpScript;
	public inputDetector inputDetScript;
	public AudioSceneGeneral audioSceneGenScript;

	void Start () {
		if (!audioSceneGenScript) {
			audioSceneGenScript = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSceneGeneral>();
		}
	}

	void Update () {
		if (inputDetScript.Tapped && !inTxtTransition) {
			inTxtTransition = true;
			StartCoroutine(HideIntroText());
			audioSceneGenScript.birdHelpSound();
		}
	}

	public void StartIntroText() {
		//introOn = true;
		inTxtTransition = true;
		// Enable this script to allow it to run its Update method.
		this.enabled = true;
		StartCoroutine(ShowIntroText());
	}
	IEnumerator ShowIntroText() {
		introTMPs[sentenceCount].FadeIn();
		float timer = 0f;
		while (timer < introTMPs[sentenceCount].fadeDuration) {
			timer += Time.deltaTime;
			yield return null;
		}
		inTxtTransition = false;
		//yield return new WaitForSeconds(introTMPs[sentenceCount].fadeDuration);
	}
	IEnumerator HideIntroText() {
		fadeOutDur = introTMPs[sentenceCount].fadeDuration;
		introTMPs[sentenceCount].FadeOut();	
		while (fadeOutDur > 0) {
			fadeOutDur -= Time.deltaTime;
			yield return null;
		}
		sentenceCount++;
		CheckIfDone();
		inTxtTransition = false;
	}
	void CheckIfDone() {
		if (sentenceCount >= maxSentence) {
			slideInHelpScript.introDone = true;
			birdIntroSaveScript.SaveBirdIntro();
			slideInHelpScript.closeMenuOnClick.SetActive(true);
			TurnOnHelpBtns();
			sentenceCount = 0;
			//introOn = false;
			slideInHelpScript.ShowButtonsAfterIntro();
			// Disable the script to avoid running the Update method for no reason.
			this.enabled = false;
			// Sequence finished.
			QueueSequenceManager.SequenceComplete();
		}
		else {
			StartCoroutine(ShowIntroText());
		}
	}

	public void TurnOnHelpBtns() {
		foreach (Button btn in helpBirdBtns)
		{
			btn.enabled = true;
		}
	}
}