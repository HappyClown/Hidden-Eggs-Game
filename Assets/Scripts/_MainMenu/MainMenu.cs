using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour 
{
	

	[Header("Reset Button")]
	public Button resetBtn;
	public Image resetBtnImg;
	public TextMeshProUGUI resetBtnTMP;
	
	[Header("Background Stuff")]
	public List<MoveCloud> cloudsToMove;
	public FadeInOutImg titleFade;
	public FadeInOut solidBGFade;
	public SpriteRenderer solidBGSprite;

	[Header("Play Button")]
	public Button playBtn;
	public Image btnImg;
	public TextMeshProUGUI btnTMP;

	[Header("Button Fade Attributes")]
	public bool fadeBtnOut;
	public float fadeSpeed;
	public float btnAlpha;

	// [Header("What To Do Bools")]
	// public bool startSeasonDissolve;

	[Header("References")]
	public Hub hubScript;



	void Start () 
	{
		playBtn.onClick.AddListener(GoToHub);
	}
	


	void Update ()
	{
		if (fadeBtnOut)
		{
			if (btnAlpha > 0) 
			{ 
				btnAlpha -= fadeSpeed; 

				// - Play Button Fade - //
				btnImg.color = new Color(1, 1, 1, Mathf.SmoothStep(0f, 1f, btnAlpha));
				btnTMP.color = new Color(0.03f, 0.03f, 0.03f, Mathf.SmoothStep(0f, 1f, btnAlpha));

				// - Reset Button Fade - //
				resetBtnImg.color = new Color(1, 1, 1, Mathf.SmoothStep(0f, 1f, btnAlpha));
				resetBtnTMP.color = new Color(0.03f, 0.03f, 0.03f, Mathf.SmoothStep(0f, 1f, btnAlpha));

				if (btnAlpha <= 0)
				{
					playBtn.enabled = false;
					resetBtn.enabled = false;
				}
			}
			
			
		}

		if(solidBGSprite.color.a <= 0.1f)
		{
			hubScript.dissolving = true;
		}
	}


	void GoToHub () 
	{
		Debug.Log("Presssing Play Button");
		// - MAKE THE CLOUDS PART - //
		foreach(MoveCloud cloud in cloudsToMove)
		{
			cloud.moveOut = true;
		}

		// - FADE TITLE - //
		titleFade.FadeOut();

		// - FADE SOLID BACKGROUND - //
		solidBGFade.FadeOut();

		// - FADE ALL BUTTONS - //
		fadeBtnOut = true;
	}
}
