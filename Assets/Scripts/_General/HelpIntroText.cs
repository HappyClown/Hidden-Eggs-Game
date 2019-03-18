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
	private bool inTxtTransition, introOn;
	private float fadeOutDur;
	[Header ("Script References")]
	public BirdIntroSave birdIntroSaveScript;
	public SlideInHelpBird slideInHelpScript;
	public AudioSceneGeneral audioSceneGenScript;

	void Start () {
		nextBtnObj.GetComponent<Button>().onClick.AddListener(NextIntroText);
		if (!audioSceneGenScript) {
			audioSceneGenScript = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSceneGeneral>();
		}
	}

	void Update () {
		if (slideInHelpScript.isUp && !slideInHelpScript.introDone) {
			ShowIntroText();
		}

		if (inTxtTransition) {
			fadeOutDur -= Time.deltaTime;
			if (fadeOutDur <= 0) {
				inTxtTransition = false;
				sentenceCount++;
				CheckIfDone();
			}
		}
	}

	public void ShowIntroText() {
		if (!introOn) { 
			introTMPs[0].gameObject.SetActive(true);
			introOn = true;
		}
		if (!slideInHelpScript.introDone) {
			nextBtnObj.SetActive(true);
		}
		else {
			nextBtnObj.SetActive(false);
		}
	}

	public void NextIntroText() {
		introTMPs[sentenceCount].FadeOut();	
		fadeOutDur = introTMPs[sentenceCount].fadeDuration;
		inTxtTransition = true;
	}

	void CheckIfDone() {
		if (sentenceCount >= maxSentence) { // check if done
			slideInHelpScript.introDone = true;
			slideInHelpScript.closeMenuOnClick.SetActive(true);
			nextBtnObj.SetActive(false);
			TurnOnHelpBtns();
			birdIntroSaveScript.SaveBirdIntro();
		}
		else { // if not show next
			introTMPs[sentenceCount].FadeIn();
		}
	}

	public void TurnOnHelpBtns() {
		foreach (Button btn in helpBirdBtns)
		{
			btn.enabled = true;
		}
	}
}