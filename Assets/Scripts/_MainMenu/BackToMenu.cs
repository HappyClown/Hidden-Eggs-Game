using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BackToMenu : MonoBehaviour 
{
	// [Header("Background Stuff")]
	// public List<MoveCloud> cloudsToMove;
	// public FadeInOutImage titleFade;
	// public FadeInOutSprite solidBGFade;
	public LevelInfoPopup levelInfoPopup;

	[Header("Button Fade Attributes")]
	public float btnFadeInWait;
	public List<GameObject> levelButtons;

	[Header("References")]
	public Hub hubScript;
	public MainMenu mainMenuScript;
	public List<SeasonGlows> seasonGlowScripts;
	public List<SeasonLock> seasonLockScripts;
	public EdgeFireflies edgeFirefliesScript;

	[Header("Back To Menu Button")]
	public Button backToMenuBtn;

	void Start () 
	{
		backToMenuBtn.onClick.AddListener(GoToMenu);
	}

	void GoToMenu ()
	{
		GlobalVariables.globVarScript.toHub = false;
		hubScript.inHub = false;
		if (!mainMenuScript.gameObject.activeSelf) {
			mainMenuScript.gameObject.SetActive(true);
			// Put in variables to make the main menu be faded out.
		}
		// - TO TURN OFF IMMEDIATELY - //
		foreach(GameObject levelButton in levelButtons)
		{
			levelButton.SetActive(false);
		}
		foreach (FadeInOutCanvasGroup hubCanvasGroupFadeScript in hubScript.hubCanvasGroupFadeScripts)
		{
			hubCanvasGroupFadeScript.FadeOut();
		}
		// Season Unlock
		foreach (SeasonLock seasonLockScript in seasonLockScripts)
		{
			seasonLockScript.BackToMenu();
		}
		levelInfoPopup.CloseTitle();
		StartCoroutine(MainMenuTransition());
	}

	IEnumerator MainMenuTransition() {
		mainMenuScript.PartialMainMenuFade(true, true, false);
		float timer = 0f;
		while (timer < btnFadeInWait) {
			timer += Time.deltaTime;
			yield return null;
		}
		mainMenuScript.PartialMainMenuFade(true, false, true);
		hubScript.ResetHubSeasons();
		hubScript.TurnOffHubObjects();
		foreach(SeasonGlows seasonGlowScript in seasonGlowScripts)
		{
			seasonGlowScript.ResetGlowAlphas();
		}
		edgeFirefliesScript.StopFireflyFX();
	}
}
