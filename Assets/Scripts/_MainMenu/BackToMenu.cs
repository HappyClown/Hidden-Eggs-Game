using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BackToMenu : MonoBehaviour 
{
	[Header("Background Stuff")]
	public List<MoveCloud> cloudsToMove;
	public FadeInOutImage titleFade;
	public FadeInOutSprite solidBGFade;

	[Header("Button Fade Attributes")]
	public bool fadeBtnIn;
	private float btnWaitTimer;
	public float btnFadeInWait;
	public FadeInOutImage rstBtnFadeScript;
	public FadeInOutImage playBtnFadeScript;
	public List<GameObject> levelButtons;

	[Header("References")]
	public Hub hubScript;
	public MainMenu mainMenuScript;
	public List<SeasonGlows> seasonGlowScripts;
	public List<SeasonLock> seasonLockScripts;
	public EdgeFireflies edgeFirefliesScript;

	[Header("Back To Menu Button")]
	public Button backToMenuBtn;
	public FadeInOutBoth backToMenuFadeScript;
	public FadeInOutImage backToMenuIconFadeScript;
	public FadeInOutCanvasGroup deleteSaveBtnCGFadeScript;

	void Start () 
	{
		backToMenuBtn.onClick.AddListener(GoToMenu);
	}

	void Update ()  
	{
		// -- Fade Menu Buttons In -- //
		if (fadeBtnIn)
		{
			btnWaitTimer += Time.deltaTime; 
			if (btnWaitTimer >= btnFadeInWait)
			{
				playBtnFadeScript.FadeIn();
				rstBtnFadeScript.FadeIn();
				deleteSaveBtnCGFadeScript.FadeIn();
				fadeBtnIn = false;
				hubScript.ResetHubSeasons();
				foreach(SeasonGlows seasonGlowScript in seasonGlowScripts)
				{
					seasonGlowScript.ResetGlowAlphas();
				}
				edgeFirefliesScript.StopFireflyFX();
				btnWaitTimer = 0f;
				//this.gameObject.SetActive(false);
				//backToMenuBtn.enabled = false;
			}
		}
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
		//backToMenuBtn.enabled = false;
		// - MAKE THE CLOUDS CLOSE - //
		foreach(MoveCloud cloud in cloudsToMove)
		{
			cloud.MoveIn();
		}
		// - FADE OUT BACKTOMENU BTN - //
		// backToMenuFadeScript.FadeOut();
		// backToMenuIconFadeScript.FadeOut();
		foreach (FadeInOutCanvasGroup hubCanvasGroupFadeScript in hubScript.hubCanvasGroupFadeScripts)
		{
			hubCanvasGroupFadeScript.FadeOut();
		}
		// Season Unlock
		foreach (SeasonLock seasonLockScript in seasonLockScripts)
		{
			seasonLockScript.BackToMenu();
		}
		// - FADE IN TITLE - //
		titleFade.FadeIn();
		// - FADE IN SOLID BACKGROUND - //
		solidBGFade.FadeIn();
		// - FADE IN ALL BUTTONS - //
		fadeBtnIn = true;
		mainMenuScript.ResetStory();	
	}
}
