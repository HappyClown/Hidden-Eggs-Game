﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour {
	public Color zeroAlpha;
	[Header("Background Stuff")]
	public List<MoveCloud> cloudsToMove;
	public FadeInOutImage titleFade;
	public Image titleImg;
	public FadeInOutSprite solidBGFade;

	[Header("Play Button")]
	public Button playBtn;
	public FadeInOutImage playBtnFadeScript;
	public Image playBtnImg;

	[Header("Reset Button")]
	public Button resetBtn;
	public FadeInOutImage resetBtnFadeScript;
	public Image resetBtnImg;

	[Header("Story")]
	public Button storyBtn;
	public TextMeshProUGUI storyTMP;
	public FadeInOutTMP fadeTMPScript;
	private bool storyAppearing, storyFullyOn, moveClouds, skipFrame;

	[Header("References")]
	public Hub hubScript;
	public HubEggcounts hubEggCountsScript;
	public inputDetector inputDetScript;
	public List<LevelTitleVillage> levelTitleScripts;
	public List<CustomButtonClick> customButtonScripts;

	void Start () {
		playBtn.onClick.AddListener(PlayBtn);
		resetBtn.onClick.AddListener(DeleteSaveFile);
		resetBtn.onClick.AddListener(NewGameBtn);
		// if (GlobalVariables.globVarScript.toHub) { // Goes straight to hub
		// 	ToHub();
		// }
	}

	void Update () {
		if (GlobalVariables.globVarScript.toHub) { // Goes straight to hub
			ToHub();
			GlobalVariables.globVarScript.toHub = false;
			Debug.Log("How many times do I go to hubbabubbaland!");
		}

		if (inputDetScript.Tapped && !skipFrame) {
			if (storyAppearing || storyFullyOn) {
				StorySkipContinue();
			}
		}
		if (storyAppearing) {
			if (storyTMP.color.a >= 0.95f) {
				storyAppearing = false;
				storyFullyOn = true;
			}
		}
		if (storyFullyOn && moveClouds) {
			if (storyTMP.color.a <= 0.15f) {
				MoveClouds();
				solidBGFade.FadeOut();
				storyFullyOn = false;
			}
		}
		skipFrame = false;
	}

	void PlayBtn() {
		// - MAKE THE CLOUDS PART - //
		 foreach(MoveCloud cloud in cloudsToMove)
		{
			cloud.MoveOut();
		}
		// - FADE OUT TITLE - //
		titleFade.FadeOut();
		// - FADE OUT SOLID BACKGROUND - //
		solidBGFade.FadeOut();
		// - FADE OUT MENU BUTTONS - //
		playBtnFadeScript.FadeOut();
		resetBtnFadeScript.FadeOut();
		//storyBtn.gameObject.SetActive(false);
		// Starts countdown timer to doing Village stuff 
		hubScript.startHubActive = true;
		foreach(LevelTitleVillage levelTitleScript in levelTitleScripts)
		{
			levelTitleScript.CloseTitle();
		}
		foreach(CustomButtonClick customButtonScript in customButtonScripts)
		{
			customButtonScript.levelSelected = false;
		}
	}

	void ToHub() {
		// - MAKE THE CLOUDS PART - //
		 foreach(MoveCloud cloud in cloudsToMove)
		{
			cloud.MoveOut();
		}
		// - FADE OUT SOLID BACKGROUND - //
		solidBGFade.FadeOut();
		// Deactivate Title & Buttons to avoid seeing them.
		playBtnImg.color = new Color(playBtnImg.color.r, playBtnImg.color.g, playBtnImg.color.b, 0);
		playBtnFadeScript.shown = false;
		playBtnFadeScript.hidden = true;
		resetBtnImg.color = new Color(resetBtnImg.color.r, resetBtnImg.color.g, resetBtnImg.color.b, 0);
		resetBtnFadeScript.shown = false;
		resetBtnFadeScript.hidden = true;
		titleImg.color = new Color(titleImg.color.r, titleImg.color.g, titleImg.color.b, 0);
		titleFade.shown = false;
		titleFade.hidden = true;
		// Starts countdown timer to doing Village stuff 
		hubScript.startHubActive = true;
	}

	void NewGameBtn() {
		// - FADE OUT TITLE - //
		titleFade.FadeOut();
		// - FADE OUT MENU BUTTONS - //
		playBtnFadeScript.FadeOut();
		resetBtnFadeScript.FadeOut();
		StoryTextAppears();
		foreach(LevelTitleVillage levelTitleScript in levelTitleScripts)
		{
			levelTitleScript.CloseTitle();
		}
		foreach(CustomButtonClick customButtonScript in customButtonScripts)
		{
			customButtonScript.levelSelected = false;
		}
	}

	void MoveClouds() {
		// - MAKE THE CLOUDS PART - //
		foreach(MoveCloud cloud in cloudsToMove)
		{
			cloud.MoveOut();
		}
	}

	public void FadeMainMenu() {
		titleFade.FadeOut();
		playBtnFadeScript.FadeOut();
		resetBtnFadeScript.FadeOut();
	}

	void StoryTextAppears() {
		storyAppearing = true;
		//storyTMP.gameObject.SetActive(true);
		fadeTMPScript.FadeIn();
		skipFrame = true;
	}

	void StorySkipContinue() {
		if (storyFullyOn) {
			fadeTMPScript.fadeDelay = false;
			fadeTMPScript.FadeOut();
			//storyBtn.gameObject.SetActive(false);
			moveClouds = true;
			hubScript.startHubActive = true;
		}
		if (storyAppearing) {
			fadeTMPScript.t = 1;
			titleFade.t = 1;
		}
	}

	public void ResetStory() {
		storyFullyOn = false;
		storyAppearing = false;
		moveClouds = false;
		storyTMP.gameObject.SetActive(false);
		//storyBtn.gameObject.SetActive(true);
		
		//storyBtn.interactable = false;
		fadeTMPScript.fadeDelay = true;
	}

	public void DeleteSaveFile() {
		GlobalVariables.globVarScript.DeleteAllData();
		hubEggCountsScript.AdjustTotEggCount();
		GlobalVariables.globVarScript.LoadHubDissolve();
		GlobalVariables.globVarScript.ResetParchmentEggs();
		foreach(LevelTitleVillage levelTitleScript in levelTitleScripts)
		{
			levelTitleScript.UpdateEggs();
		}
	}
}
