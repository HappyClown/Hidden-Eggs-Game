using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour 
{
	[Header("Background Stuff")]
	public List<MoveCloud> cloudsToMove;
	public FadeInOutBoth titleFade;
	public FadeInOutSprite solidBGFade;

	[Header("Reset Button")]
	public Button resetBtn;
	public Image resetBtnImg;
	public TextMeshProUGUI resetBtnTMP;

	[Header("Play Button")]
	public Button playBtn;
	public Image btnImg;
	public TextMeshProUGUI btnTMP;

	[Header("Story")]
	public Button storyBtn;
	public TextMeshProUGUI storyTMP;
	public FadeInOutTMP fadeTMPScript;
	public bool storyAppearing, storyFullyOn, moveClouds;
	public float storyTimer, storyTime;

	[Header("Button Fade Attributes")]
	public bool fadeBtnOut;
	public float fadeSpeed;
	public float btnAlpha;

	[Header("References")]
	public Hub hubScript;
	public HubEggcounts hubEggCountsScript;
	public bool menuReady;
	public inputDetector inputDetScript;


	void Awake()
	{
		hubEggCountsScript.AdjustTotEggCount();
	}


	void Start () 
	{
		playBtn.onClick.AddListener(PlayBtn);

		resetBtn.onClick.AddListener(DeleteSaveFile);
		resetBtn.onClick.AddListener(NewGameBtn);
		//resetBtn.onClick.AddListener(StoryTextAppears);

		//storyBtn.onClick.AddListener(StorySkipContinue);

		if (GlobalVariables.globVarScript.toHub) { PlayBtn(); } // Goes straight to hub
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

		// -- Fade Buttons Out -- //
		if (fadeBtnOut)
		{
			if (btnAlpha > 0) 
			{ 
				btnAlpha -= fadeSpeed; 

				// - Play Button Fade Out - //
				btnImg.color = new Color(1, 1, 1, Mathf.SmoothStep(0f, 1f, btnAlpha));
				btnTMP.color = new Color(0.03f, 0.03f, 0.03f, Mathf.SmoothStep(0f, 1f, btnAlpha));

				// - Reset Button Fade Out - //
				resetBtnImg.color = new Color(1, 1, 1, Mathf.SmoothStep(0f, 1f, btnAlpha));
				resetBtnTMP.color = new Color(0.03f, 0.03f, 0.03f, Mathf.SmoothStep(0f, 1f, btnAlpha));

				if (btnAlpha <= 0)
				{
					playBtn.enabled = false;
					resetBtn.enabled = false;
					fadeBtnOut = false;
					StoryTextAppears();
				}
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
		// - Check If I Can Interact With The Menu - //
	}


	void PlayBtn () 
	{
		// - MAKE THE CLOUDS PART - //
		 foreach(MoveCloud cloud in cloudsToMove)
		{
			cloud.moveOut = true;
		}

		// - FADE OUT TITLE - //
		titleFade.FadeOut();

		// - FADE OUT SOLID BACKGROUND - //
		solidBGFade.FadeOut();

		// - FADE OUT ALL MENU BUTTONS - //
		fadeBtnOut = true;

		storyBtn.gameObject.SetActive(false);
	}

	void NewGameBtn () 
	{
		// - MAKE THE CLOUDS PART - //
		// foreach(MoveCloud cloud in cloudsToMove)
		// {
		// 	cloud.moveOut = true;
		// }

		// - FADE OUT TITLE - //
		titleFade.FadeOut();

		// - FADE OUT SOLID BACKGROUND - //
		//solidBGFade.FadeOut();

		// - FADE OUT ALL MENU BUTTONS - //
		fadeBtnOut = true;
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

	public void DeleteSaveFile ()
	{
		GlobalVariables.globVarScript.DeleteEggData();

		hubEggCountsScript.AdjustTotEggCount();
	}

}
