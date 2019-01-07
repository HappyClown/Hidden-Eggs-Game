using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HelpIntroText : MonoBehaviour {
	public List<FadeInOutTMP> introTMPs;
	public GameObject nextBtnObj;
	private int sentenceCount;
	public int maxSentence;
	public List<Button> helpBirdBtns;
	public SlideInHelpBird slideInHelpScript;
	private bool inTxtTransition;
	private float fadeOutDur;
	public BirdIntroSave birdIntroSaveScript;

	void Start () {
		nextBtnObj.GetComponent<Button>().onClick.AddListener(NextIntroText);
	}

	void Update () {
		if (inTxtTransition) {
			fadeOutDur -= Time.deltaTime;
			if (fadeOutDur <= 0) {
				inTxtTransition = false;
				sentenceCount += 1;
				CheckIfDone();
			}
		}
	}

	public void ShowIntroText() {
		introTMPs[0].gameObject.SetActive(true);
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
			slideInHelpScript.RiddleButton();
			slideInHelpScript.HintButton();
			slideInHelpScript.closeMenuOnClick.SetActive(true);
			nextBtnObj.SetActive(false);
			TurnOnHelpBtns();
			birdIntroSaveScript.SaveBirdIntro();
		} else { // if not show next
			introTMPs[sentenceCount].FadeIn();
		}
	}

	public void TurnOnHelpBtns()
	{
		foreach (Button btn in helpBirdBtns)
		{
			btn.enabled = true;
		}
	}
}