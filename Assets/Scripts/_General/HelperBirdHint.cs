﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelperBirdHint : MonoBehaviour {
	[Header ("Hint")]
	private bool showHint;
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
		if (slideInScript.isUp && slideInScript.introDone && !showHint) {
			hintFadeInOutScript.FadeIn();
			if (clickOnEggsScript.eggsFound < clickOnEggsScript.totalRegEggs) {
				hintBtn.interactable = true;
			}
			else {
				hintBtn.interactable = false;
			}
			showHint = true;
		}

		if (!slideInScript.isUp && showHint) {
			hintFadeInOutScript.FadeOut();
			hintBtn.interactable = false;
			showHint = false;
		}
	}

	public void StartHint() {
		if (hintManScript.hintAvailable) {
			hintManScript.startHint = true;
		}
		slideInScript.MoveBirdUpDown();
	}
}
