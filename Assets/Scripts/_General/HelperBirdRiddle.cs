using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelperBirdRiddle : MonoBehaviour {
	[Header("Riddle")]
	public GameObject riddleBtnObj;
	private Button riddleBtn;
	private Image riddleImg;
	private GameObject riddleCurntActive;
	public List<GameObject> riddleHints;
	private bool riddBtnOn, riddTextOn;
	[Header("Other")]
	public GameObject dontCloseMenu;
	public Button hintBtn;
	[Header ("Script References")]
	public SlideInHelpBird slideInScript;
	public HelperBirdHint helperBirdHintScript;
	public ClickOnEggs clickOnEggsScript;
	public FadeInOutCanvasGroup hintCGFadeScript, riddButtonCGFadeScript;
	public FadeInOutTMP riddTextFadeScript;
	public FadeInOutImage plainGoldEggFadeScript;

	void Start () {
		if (!riddleBtn) { riddleBtn = riddleBtnObj.GetComponent<Button>(); }
		if (!riddleImg) { riddleImg = riddleBtnObj.GetComponent<Image>(); }
		riddleBtn.onClick.AddListener(ShowRiddleText);
		riddTextFadeScript.fadeDelayDur = riddButtonCGFadeScript.fadeDuration;
		plainGoldEggFadeScript.fadeDelayDur = riddButtonCGFadeScript.fadeDuration;
	}

	public void ShowRiddleButton () {
		if (GlobalVariables.globVarScript.riddleSolved) {
			riddleBtn.interactable = false;
			riddButtonCGFadeScript.maxAlpha = 0.5f;
		}
		else {
			riddleBtn.interactable = true;
		}
		riddButtonCGFadeScript.FadeIn();
		riddBtnOn = true;
		dontCloseMenu.SetActive(true);
	}
	public void HideRiddleButton () {
		riddBtnOn = false;
		if (!riddButtonCGFadeScript.hidden && !riddButtonCGFadeScript.fadingOut) {
			riddButtonCGFadeScript.FadeOut();
			riddleBtn.interactable = false;
		}
	}
	public void HideRiddleText () {
		if (riddTextOn) {
			riddTextFadeScript.fadeDelayDur = 0f;
			plainGoldEggFadeScript.fadeDelayDur = 0f;
			riddTextFadeScript.FadeOut();
			plainGoldEggFadeScript.FadeOut();
		}
	}

	public void ShowRiddleText() {
		int random = Random.Range(0, riddleHints.Count);
		riddleHints[random].SetActive(true);
		riddleCurntActive = riddleHints[random];

		riddTextFadeScript.fadeDelayDur = riddButtonCGFadeScript.fadeDuration;
		plainGoldEggFadeScript.fadeDelayDur = riddButtonCGFadeScript.fadeDuration;
		riddTextFadeScript.FadeIn();
		plainGoldEggFadeScript.FadeIn();
		
		riddTextOn = true;

		dontCloseMenu.SetActive(false);

		hintCGFadeScript.FadeOut();
		hintBtn.interactable = false;
		riddButtonCGFadeScript.FadeOut();
		riddleBtn.interactable = false;
	}
}
