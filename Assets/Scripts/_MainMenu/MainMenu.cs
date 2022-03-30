using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour {
	[Header("Background Stuff")]
	public GameObject backgroundStuffParent;
	public List<MoveCloud> cloudsToMove;
	public FadeInOutSprite solidBGFade;

	[Header("Buttons")]
	public Button playBtn;
	public Button resetBtn;

	[Header("Delete Save Button")]
	public Button deleteSaveBtn;
	public FadeInOutCanvasGroup titleAndBtnsCGFadeScript;
	public FadeInOutCanvasGroup deleteSaveBtnCGFadeScript;

	[Header("Story")]
	public StoryIntro storyIntroScript;

	[Header("References")]
	public Hub hubScript;
	public HubEggcounts hubEggCountsScript;
	public List<LevelTitleVillage> levelTitleScripts;
	public List<LevelFireflies> levelFirefliesScripts;
	public List<CustomButtonClick> customButtonScripts;
	public List<SeasonLock> seasonLockScripts;

	void Start () {
		playBtn.onClick.AddListener(PlayBtn);
		resetBtn.onClick.AddListener(DeleteSaveFile);
		resetBtn.onClick.AddListener(NewGameBtn);
		deleteSaveBtn.onClick.AddListener(DeleteSaveFile);
		// if (GlobalVariables.globVarScript.toHub) { // Goes straight to hub
		// 	ToHub();
		// }
	}

	void Update () {
		// Goes to the Hub without going to through main menu.
		if (GlobalVariables.globVarScript.toHub) {
			ToHubDirectly(true);
			GlobalVariables.globVarScript.toHub = false;
			// Debug.Log("How many times do I go to hubbabubbaland!");
		}
	}

	void PlayBtn() {
		PartialMainMenuFade();
		// Starts countdown timer to doing Village stuff 
		hubScript.ActivateHub();

		foreach(LevelTitleVillage levelTitleScript in levelTitleScripts)
		{
			levelTitleScript.ResetTitle();
		}
		foreach(LevelFireflies levelFireflies in levelFirefliesScripts)
		{
			levelFireflies.StopLevelFireflies();
		}
		foreach(CustomButtonClick customButtonScript in customButtonScripts)
		{
			customButtonScript.levelSelected = false;
		}
	}

	void NewGameBtn() {
		// Fade out the main menu title, buttons, full blue background and part clouds.
		PartialMainMenuFade(false, false, true);
		storyIntroScript.gameObject.SetActive(true);
		storyIntroScript.inStoryIntro = true;

		foreach(LevelTitleVillage levelTitleScript in levelTitleScripts)
		{
			levelTitleScript.ResetTitle();
		}
		foreach(LevelFireflies levelFireflies in levelFirefliesScripts)
		{
			levelFireflies.StopLevelFireflies();
		}
		foreach(CustomButtonClick customButtonScript in customButtonScripts)
		{
			customButtonScript.levelSelected = false;
		}
	}

	void MoveClouds(bool moveCloudsIn = false) {
		if (moveCloudsIn) {
			foreach(MoveCloud cloud in cloudsToMove)
			{
				cloud.MoveIn();
			}
		}
		else {
			foreach(MoveCloud cloud in cloudsToMove)
			{
				cloud.MoveOut();
			}
		}
	}

	// public void FullMainMenuFade(bool fadeIn = false) {
	// 	if (fadeIn) {
	// 		titleAndBtnsCGFadeScript.FadeIn();
	// 		deleteSaveBtnCGFadeScript.FadeIn(); // Temporary
	// 		solidBGFade.FadeIn();
	// 		MoveClouds(true);
	// 	}
	// 	else {
	// 		titleAndBtnsCGFadeScript.FadeOut();
	// 		deleteSaveBtnCGFadeScript.FadeOut(); // Temporary
	// 		solidBGFade.FadeOut();
	// 		MoveClouds();
	// 	}
	// }

	public void PartialMainMenuFade(bool fadeIn = false, bool fadeBackground = true, bool fadeElements = true) {
		if (fadeBackground) {
			if (fadeIn) {
				solidBGFade.FadeIn();
				MoveClouds(true);
			}
			else {
				solidBGFade.FadeOut();
				MoveClouds();
			}
		}
		if (fadeElements) {
			if (fadeIn) {
				titleAndBtnsCGFadeScript.FadeIn();
				//deleteSaveBtnCGFadeScript.FadeIn();
			}
			else {
				titleAndBtnsCGFadeScript.FadeOut();
				//deleteSaveBtnCGFadeScript.FadeOut();
			}
		}
	}

	// Used to bypass the Main Menu and go directly to the Hub. 
	public void ToHubDirectly(bool startHubActive) {
		// Fade out the main menu background elements.
		PartialMainMenuFade(false, true, false);
		// Deactivate or in this case fade out the Title & Buttons instantly to avoid seeing them if they are on when the method is called.
		if (titleAndBtnsCGFadeScript.gameObject.activeSelf) {
			float ogDuration = titleAndBtnsCGFadeScript.fadeDuration;
			titleAndBtnsCGFadeScript.fadeDuration = 0f;
			titleAndBtnsCGFadeScript.FadeOut();
			titleAndBtnsCGFadeScript.fadeDuration = ogDuration;
		}
		// if (deleteSaveBtnCGFadeScript.gameObject.activeSelf) {
		// 	deleteSaveBtnCGFadeScript.gameObject.SetActive(false);
		// }
		if (startHubActive) {
			hubScript.ActivateHub();
		}
			//seasonLockScripts[0].StartSeasonUnlockChecks();
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
		// This probably should be a variable saved in the village save load manager.
		foreach(SeasonLock seasonLockScript in seasonLockScripts)
		{
			seasonLockScript.NewGame();
		}
	}
}
