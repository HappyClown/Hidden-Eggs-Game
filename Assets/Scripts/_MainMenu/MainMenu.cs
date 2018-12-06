using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour 
{
	[Header("Background Stuff")]
	public List<MoveCloud> cloudsToMove;
	public FadeInOutImage titleFade;
	public FadeInOutSprite solidBGFade;

	[Header("Play Button")]
	public Button playBtn;
	public FadeInOutImage playBtnFadeScript;

	[Header("Reset Button")]
	public Button rstBtn;
	public FadeInOutImage rstBtnFadeScript;

	[Header("Story")]
	public Button storyBtn;
	public TextMeshProUGUI storyTMP;
	public FadeInOutTMP fadeTMPScript;
	private bool storyAppearing, storyFullyOn, moveClouds;

	[Header("References")]
	public Hub hubScript;
	public HubEggcounts hubEggCountsScript;
	public inputDetector inputDetScript;


	void Start () 
	{
		playBtn.onClick.AddListener(PlayBtn);
		rstBtn.onClick.AddListener(DeleteSaveFile);
		rstBtn.onClick.AddListener(NewGameBtn);

		if (GlobalVariables.globVarScript.toHub) { // Goes straight to hub
			//this.gameObject.SetActive(false); 
			PlayBtn();
		}
	}


	void Update ()
	{
		if (inputDetScript.Tapped)
		{
			//Debug.Log("tapped! and : " + storyAppearing);
			if (storyAppearing || storyFullyOn)
			{
				StorySkipContinue();
			}
		}

		if (storyAppearing)
		{
			if (storyTMP.color.a >= 1)
			{
				storyAppearing = false;
				storyFullyOn = true;
			}
		}

		if (storyFullyOn && moveClouds)
		{
			if (storyTMP.color.a <= 0.15f)
			{
				MoveClouds();
				solidBGFade.FadeOut();
				storyFullyOn = false;
			}
		}

	}


	void PlayBtn() 
	{
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
		rstBtnFadeScript.FadeOut();

		storyBtn.gameObject.SetActive(false);

		// Starts countdown timer to doing Village stuff 
		hubScript.startHubActive = true;
	}

	void NewGameBtn() 
	{
		// - FADE OUT TITLE - //
		titleFade.FadeOut();

		// - FADE OUT MENU BUTTONS - //
		playBtnFadeScript.FadeOut();
		rstBtnFadeScript.FadeOut();

		StoryTextAppears();
	}

	void MoveClouds()
	{
		// - MAKE THE CLOUDS PART - //
		foreach(MoveCloud cloud in cloudsToMove)
		{
			cloud.moveOut = true;
		}
	}

	void StoryTextAppears()
	{
		//ResetStory();
		//storyBtn.interactable = true;
		storyAppearing = true;
		storyTMP.gameObject.SetActive(true);
		fadeTMPScript.FadeIn();
	}

	void StorySkipContinue()
	{
		if (storyFullyOn)
		{
			fadeTMPScript.fadeDelay = false;
			fadeTMPScript.FadeOut();
			storyBtn.gameObject.SetActive(false);
			moveClouds = true;
			hubScript.startHubActive = true;
		}
		if (storyAppearing)
		{
			//Debug.Log("Should make story appear.");
			fadeTMPScript.t = 1;
			titleFade.t = 1;
			//storyFullyOn = true;
			//moveClouds = true;
		}
	}

	public void ResetStory()
	{
		storyFullyOn = false;
		storyAppearing = false;
		moveClouds = false;
		storyTMP.gameObject.SetActive(false);
		storyBtn.gameObject.SetActive(true);
		
		storyBtn.interactable = false;
		fadeTMPScript.fadeDelay = true;

	}

	public void DeleteSaveFile()
	{
		GlobalVariables.globVarScript.DeleteAllData();

		hubEggCountsScript.AdjustTotEggCount();

		for (int i = 0; i < GlobalVariables.globVarScript.dissSeasonsBools.Count; i++)
		{
			GlobalVariables.globVarScript.dissSeasonsBools[i] = false;
		}

		GlobalVariables.globVarScript.LoadHubDissolve();
	}

}
