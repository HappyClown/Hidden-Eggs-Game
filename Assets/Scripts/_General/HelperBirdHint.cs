using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelperBirdHint : MonoBehaviour {
	
	[Header ("Hint")]
	public GameObject hintBtnObj;
	public Button hintBtn;
	private Image hintImg;
	[Header ("Script References")]
	public HintManager hintManScript;
	public SlideInHelpBird slideInScript;
	public ClickOnEggs clickOnEggsScript;
	public FadeInOutImage hintFadeInOutScript, riddFadeInOutScript;

	void Start () {
		hintBtn = hintBtnObj.GetComponent<Button>();
		hintImg = hintBtnObj.GetComponent<Image>();
		hintBtn.onClick.AddListener(StartHint);
	}
	
	void Update () {
		if (slideInScript.isUp && !slideInScript.txtBubFadedIn && slideInScript.introDone) {
			ShowHintButton();
		}
		if (slideInScript.moveDown && hintBtnObj.activeSelf) {
				hintFadeInOutScript.FadeOut();
		}
	}

	public void StartHint() {
		hintManScript.startHint = true;
		slideInScript.MoveBirdUpDown();
		hintFadeInOutScript.FadeOut();
		riddFadeInOutScript.FadeOut();
	}

	public void ShowHintButton() {
		if (!hintBtnObj.activeSelf) {
			hintBtnObj.SetActive(true);

			if (clickOnEggsScript.eggsFound >= clickOnEggsScript.eggs.Count - 1) { // -1 for the golden egg
				hintBtn.interactable = false;
			}
			else {
				hintBtn.interactable = true;
				hintImg.raycastTarget = true;
			}
		}
	}
}
