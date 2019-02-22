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
	public ClickOnEggs clickOnEggsScript;
	public FadeInOutImage hintFadeScript, riddFadeScript;
	public FadeInOutTMP riddTextFadeScript;

	void Start () {
		riddleBtn = riddleBtnObj.GetComponent<Button>();
		riddleImg = riddleBtnObj.GetComponent<Image>();
		riddleBtn.onClick.AddListener(ShowRiddleText);
		riddTextFadeScript.fadeDelayDur = riddFadeScript.fadeDuration;
	}
	
	void Update () {
		if (slideInScript.isUp && slideInScript.introDone && !riddBtnOn) {
			riddFadeScript.FadeIn();
			if (clickOnEggsScript.goldenEggFound == 1) {
				riddleBtn.interactable = false;
			}
			else {
				riddleBtn.interactable = true;
			}
			riddBtnOn = true;

			dontCloseMenu.SetActive(true);
		}

		if (!slideInScript.isUp && riddBtnOn) {
			riddFadeScript.FadeOut();
			riddleBtn.interactable = false;
			riddBtnOn = false;
		}

		if (slideInScript.moveDown && riddTextOn) {
			riddTextFadeScript.FadeOut();
		}
	}

	public void ShowRiddleText() {
		int random = Random.Range(0, riddleHints.Count);
		riddleHints[random].SetActive(true);
		riddleCurntActive = riddleHints[random];
		riddTextFadeScript.FadeIn();
		riddTextOn = true;

		dontCloseMenu.SetActive(false);

		hintFadeScript.FadeOut();
		hintBtn.interactable = false;
		riddFadeScript.FadeOut();
		riddleBtn.interactable = false;
	}
}
