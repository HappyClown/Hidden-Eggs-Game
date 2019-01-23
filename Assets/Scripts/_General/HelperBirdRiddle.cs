using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelperBirdRiddle : MonoBehaviour {
	public GameObject dontCloseMenu;
	public Button hintBtn;
	[Header("Riddle")]
	public GameObject riddleBtnObj;
	private Button riddleBtn;
	private Image riddleImg;
	private GameObject riddleCurntActive;
	public List<GameObject> riddleHints;
	private bool riddBtnOn;
	[Header ("Script References")]
	public FadeInOutTMP riddTextFadeInOutScript;
	public SlideInHelpBird slideInScript;
	public ClickOnEggs clickOnEggsScript;
	public FadeInOutImage hintFadeInOutScript, riddFadeInOutScript;

	void Start () {
		riddleBtn = riddleBtnObj.GetComponent<Button>();
		riddleImg = riddleBtnObj.GetComponent<Image>();
		riddleBtn.onClick.AddListener(ShowRiddleText);
		riddTextFadeInOutScript.fadeDelayDur = riddFadeInOutScript.fadeDuration;
	}
	
	void Update () {
		if (slideInScript.isUp && slideInScript.introDone && !riddBtnOn) {
			ShowRiddleButton();
			if (slideInScript.birdUpTrigger) {
				dontCloseMenu.SetActive(true);
				slideInScript.birdUpTrigger = false;
				Debug.Log("Dontclosemenu is being activated.");
			}
			riddBtnOn = true;
		}
		if (slideInScript.moveDown) {
			if (riddleBtnObj.activeSelf) {
				riddFadeInOutScript.FadeOut();
			}
			if (riddleCurntActive) { 
				riddTextFadeInOutScript.FadeOut();
			}
			if (riddBtnOn) {
				riddBtnOn = false;
			}
		}
	}

	public void ShowRiddleText() {
		int random = Random.Range(0, riddleHints.Count);
		riddleHints[random].SetActive(true);
		riddleCurntActive = riddleHints[random];
		riddTextFadeInOutScript.FadeIn();

		dontCloseMenu.SetActive(false);
		riddleBtn.enabled = false;
		hintBtn.enabled = false;
		hintFadeInOutScript.FadeOut();
		riddFadeInOutScript.FadeOut();
	}

	public void ShowRiddleButton() {
		if (!riddleBtnObj.activeSelf) {
			riddleBtnObj.SetActive(true);
			//riddleBtn.enabled = true;

			if (clickOnEggsScript.goldenEggFound > 0) {
				riddleBtn.interactable = false;
			}
			else {
				riddleBtn.interactable = true;
				//riddleImg.raycastTarget = true;
			}
		}
	}
}
