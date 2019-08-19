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
	public FadeInOutCanvasGroup hintCGFadeScript, riddCGFadeScript;
	public FadeInOutTMP riddTextFadeScript;
	public FadeInOutImage plainGoldEggFadeScript;

	void Start () {
		riddleBtn = riddleBtnObj.GetComponent<Button>();
		riddleImg = riddleBtnObj.GetComponent<Image>();
		riddleBtn.onClick.AddListener(ShowRiddleText);
		riddTextFadeScript.fadeDelayDur = riddCGFadeScript.fadeDuration;
		plainGoldEggFadeScript.fadeDelayDur = riddCGFadeScript.fadeDuration;
	}
	
	void Update () {
		if (slideInScript.isUp && slideInScript.introDone && !riddBtnOn) {
			if (clickOnEggsScript.goldenEggFound == 1) {
				riddleBtn.interactable = false;
				riddCGFadeScript.maxAlpha = 0.5f;
			}
			else {
				riddleBtn.interactable = true;
			}
			riddCGFadeScript.FadeIn();
			riddBtnOn = true;

			dontCloseMenu.SetActive(true);
		}

		if (!slideInScript.isUp && riddBtnOn) {
			riddBtnOn = false;
			if (!riddCGFadeScript.hidden && !riddCGFadeScript.fadingOut) {
				riddCGFadeScript.FadeOut();
				riddleBtn.interactable = false;
			}
		}

		if (slideInScript.moveDown && riddTextOn) {
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

		riddTextFadeScript.fadeDelayDur = riddCGFadeScript.fadeDuration;
		plainGoldEggFadeScript.fadeDelayDur = riddCGFadeScript.fadeDuration;
		riddTextFadeScript.FadeIn();
		plainGoldEggFadeScript.FadeIn();
		
		riddTextOn = true;

		dontCloseMenu.SetActive(false);

		hintCGFadeScript.FadeOut();
		hintBtn.interactable = false;
		riddCGFadeScript.FadeOut();
		riddleBtn.interactable = false;
	}
}
