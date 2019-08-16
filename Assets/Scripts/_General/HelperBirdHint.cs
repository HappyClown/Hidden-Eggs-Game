using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelperBirdHint : MonoBehaviour {
	[Header ("Hint")]
	public GameObject hintBtnObj;
	public Button hintBtn;
	private Image hintImg;
	public bool showHint;
	[Header ("Script References")]
	public HintManager hintManScript;
	public SlideInHelpBird slideInScript;
	public ClickOnEggs clickOnEggsScript;
	public FadeInOutCanvasGroup hintCGFadeScript, riddCGFadeScript;
	public AudioHelperBird audioHelperBirdScript;

	void Start () {
		hintBtn = hintBtnObj.GetComponent<Button>();
		hintImg = hintBtnObj.GetComponent<Image>();
		hintBtn.onClick.AddListener(StartHint);

		if(!audioHelperBirdScript){audioHelperBirdScript= GameObject.Find("Audio").GetComponent<AudioHelperBird>();}
	}

	void Update () {
		if (slideInScript.isUp && slideInScript.introDone && !showHint) {
			if (clickOnEggsScript.eggsFound < clickOnEggsScript.totalRegEggs) {
				hintBtn.interactable = true;
			}
			else {
				hintBtn.interactable = false;
				hintCGFadeScript.maxAlpha = 0.5f;
			}
			hintCGFadeScript.FadeIn();
			showHint = true;

		}

		if (!slideInScript.isUp && showHint) {
			showHint = false;
			if (!hintCGFadeScript.hidden && !hintCGFadeScript.fadingOut) {
				hintBtn.interactable = false;
				hintCGFadeScript.FadeOut();
			}
		}
	}

	public void StartHint() {
		if (hintManScript.hintAvailable) {
			hintManScript.startHint = true;
			//sound long
			audioHelperBirdScript.hintSndOnLong();
		}
		slideInScript.MoveBirdUpDown();
		hintCGFadeScript.FadeOut();
	}
}
