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
		if (!hintBtn) { hintBtn = hintBtnObj.GetComponent<Button>(); }
		if (!hintImg) { hintImg = hintBtnObj.GetComponent<Image>(); }
		hintBtn.onClick.AddListener(StartHint);

		if(!audioHelperBirdScript){audioHelperBirdScript= GameObject.Find("Audio").GetComponent<AudioHelperBird>();}
	}
	public void ShowHintButton() {
		if (clickOnEggsScript.eggsFound < clickOnEggsScript.totalRegEggs) {
			hintBtn.interactable = true;
		}
		else {
			hintBtn.interactable = false;
			hintCGFadeScript.maxAlpha = 0.5f;
		}
		hintCGFadeScript.FadeIn();
	}
	public void HideHintButton() {
		if (!hintCGFadeScript.hidden && !hintCGFadeScript.fadingOut) {
			hintBtn.interactable = false;
			hintCGFadeScript.FadeOut();
		}
	}

	public void StartHint() {
		hintManScript.StartHint();
		audioHelperBirdScript.hintSndOnLong();
		// Else possible just restart the hint from teh bird, can happen if the hint ball is far away from the bird the player can have time to press the hint button before the hint ball comes back and then nothing happens, which isn't super bad either, but it feels weird to press the button only to have nothing happen.
		slideInScript.MoveBirdUpDown();
		hintCGFadeScript.FadeOut();
	}
}
