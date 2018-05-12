using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuPlay : MonoBehaviour 
{
	public Button playBtn;
	
	public List<MoveCloud> cloudsToMove;
	public FadeInOut titleFade;
	public FadeInOut solidBGFade;
	public SpriteRenderer solidBGSprite;

	public bool fadeBtn;
	public float fadeSpeed;
	public float btnAlpha;
	public Image btnImg;
	public TextMeshProUGUI btnTMP;

	public bool enableLevelSelection;



	void Start () 
	{
		playBtn.onClick.AddListener(MoveClouds);
	}
	


	void Update ()
	{
		if (fadeBtn)
		{
			if (btnAlpha > 0) { btnAlpha -= fadeSpeed; }
			btnImg.color = new Color(1, 1, 1, Mathf.SmoothStep(0f, 1f, btnAlpha));
			btnTMP.color = new Color(0.03f, 0.03f, 0.03f, Mathf.SmoothStep(0f, 1f, btnAlpha));
		}

		if(solidBGSprite.color.a <= 0.1f)
		{
			enableLevelSelection = true;
		}
	}


	void MoveClouds () 
	{
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
