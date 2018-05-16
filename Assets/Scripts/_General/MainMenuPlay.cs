using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuPlay : MonoBehaviour 
{
	

	[Header("Reset Button")]
	public Button resetBtn;
	public Image resetBtnImg;
	public TextMeshProUGUI resetBtnTMP;
	
	[Header("Background Stuff")]
	public List<MoveCloud> cloudsToMove;
	public FadeInOut titleFade;
	public FadeInOut solidBGFade;
	public SpriteRenderer solidBGSprite;

	[Header("Play Button")]
	public Button playBtn;
	public Image btnImg;
	public TextMeshProUGUI btnTMP;

	[Header("Button Fade Attributes")]
	public bool fadeBtn;
	public float fadeSpeed;
	public float btnAlpha;

	[Header("What To Do Bools")]
	public bool enableLevelSelection;



	void Start () 
	{
		playBtn.onClick.AddListener(MoveClouds);
	}
	


	void Update ()
	{
		if (fadeBtn)
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
			enableLevelSelection = true;
		}
	}


	void MoveClouds () 
	{
		Debug.Log("Presssing Play Button");
		// - MAKE THE CLOUDS PART - //
		foreach(MoveCloud cloud in cloudsToMove)
		{
			cloud.doIMove = true;
		}

		// - FADE TITLE - //
		titleFade.FadeOut();

		// - FADE SOLID BACKGROUND - //
		solidBGFade.FadeOut();

		// - FADE THIS BUTTON - //
		fadeBtn = true;
	}
}
