using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideInHelpBird : MonoBehaviour 
{
	[Header("Shadow")]
	public Image shadow;
	public float shadowAlpha;
	[Header("Text Bubble")]
	public Image txtBubble;
	private float txtBubAlpha;
	public float txtBubFadeTime;
	public bool introDone, txtBubFadedIn;
	public bool birdUpTrigger;
	// [Header("Riddle")]
	// public GameObject riddleBtnObj;
	// private Button riddleBtn;
	// private Image riddleImg;
	// private float riddleBtnAlpha;
	// public float riddleBtnFadeTime;
	// private bool riddleTextShow;
	// private GameObject riddleCurntActive;
	// public List<GameObject> riddleHints;
	// private bool riddleBtnOn;
	// [Header("Hint")]
	// public GameObject hintBtnObj;
	// public Button hintBtn;
	// private Image hintImg;
	// public HintManager hintManScript;
	// private bool hintBtnOn;
	[Header("Zones")]
	public GameObject closeMenuOnClick;
	public GameObject blockClickingOnEggs;
	public GameObject dontCloseMenu;
	private float allowClickTimer;
	[Header("Bird Movement")]
	public float duration;
	private float newDuration;
	public bool moveUp, moveDown, isUp, isDown = true;
	public Transform helpBirdTrans, hiddenHelpBirdPos, shownHelpBirdPos;
	public Vector3 curHelpBirdPos;
	private float totalDist;
	private float distLeft;
	private float distPercent;
	private float lerpValue;
	public AnimationCurve animCur;
	[Header("Script References")]
	public SceneTapEnabler scenTapEnabScript;
	//public ClickOnEggs clickOnEggsScript;
	public LevelTapMannager lvlTapManScript;
	public HelpIntroText helpIntroTxtScript;


	void Start ()
	{
		// if the intro has been seen already, introDone = true;
		totalDist = Vector2.Distance(hiddenHelpBirdPos.position, shownHelpBirdPos.position);
		// riddleBtn = riddleBtnObj.GetComponent<Button>();
		// riddleImg = riddleBtnObj.GetComponent<Image>();
		// riddleBtn.onClick.AddListener(ShowRiddleText);
		// hintBtn = hintBtnObj.GetComponent<Button>();
		// hintImg = hintBtnObj.GetComponent<Image>();
		// hintBtn.onClick.AddListener(StartHint);
	}


	void Update ()
	{
		distLeft = Vector2.Distance(helpBirdTrans.position, shownHelpBirdPos.position);
		distPercent = (totalDist - distLeft) / totalDist;

		#region Up
		if (moveUp)
		{
			lerpValue += Time.deltaTime / newDuration;
			helpBirdTrans.position = Vector3.Lerp(curHelpBirdPos, shownHelpBirdPos.position, animCur.Evaluate(lerpValue));

			if (shadowAlpha < 1) { shadowAlpha = distPercent; }
			shadow.color = new Color(1,1,1, shadowAlpha);

			// Continue to fade out the text buble even if the bird is going up.
			if (txtBubAlpha > 0) { txtBubAlpha -= Time.deltaTime / txtBubFadeTime; }
			txtBubble.color = new Color(1,1,1, txtBubAlpha);
		}

		// Finished moving up
		if (moveUp && lerpValue >= 1)
		{
			lerpValue = 1;
			isUp = true;
			moveUp = false;
			birdUpTrigger = true;
			helpBirdTrans.position = shownHelpBirdPos.position;
		}
		// Is all the way up
		if (isUp)
		{
			blockClickingOnEggs.SetActive(true);
			// Fade in text bubble
			if (txtBubAlpha < 1) {
				txtBubAlpha += Time.deltaTime / txtBubFadeTime; 
				txtBubble.color = new Color(1,1,1, txtBubAlpha);
			}
			else { // Bird parchment has fully appeared
				if (!txtBubFadedIn) {
					// if (introDone) {
					// 	// Make the riddle button appear
					// 	// RiddleButton();
					// 	// Make the hint button appear
					// 	// HintButton();
					// }
					if(!introDone) {
						helpIntroTxtScript.ShowIntroText();
					}
					txtBubFadedIn = true;
				}
			}
		}
		#endregion

		#region Down
		if (moveDown)
		{
			//Debug.Log("Move down was true here. RIP");
			closeMenuOnClick.SetActive(false);
			dontCloseMenu.SetActive(false);

			lerpValue += Time.deltaTime / newDuration;
			helpBirdTrans.position = Vector3.Lerp(curHelpBirdPos, hiddenHelpBirdPos.position, animCur.Evaluate(lerpValue));

			//if (riddleCurntActive) { riddleCurntActive.SetActive(false); }

			if (shadowAlpha > 0) { shadowAlpha = distPercent; }
			shadow.color = new Color(1,1,1, shadowAlpha);

			// Fade out text bubble
			if (txtBubAlpha > 0) { txtBubAlpha -= Time.deltaTime / txtBubFadeTime; }
			txtBubble.color = new Color(1,1,1, txtBubAlpha);
			//if (riddleBtnObj.activeSelf) { riddleBtnObj.SetActive(false); }
			//if (hintBtnObj.activeSelf) { hintBtnObj.SetActive(false); }
		}

		// Finished moving down
		if (moveDown && lerpValue >= 1)
		{
			blockClickingOnEggs.SetActive(false);
			moveDown = false;
			isDown = true;
			helpBirdTrans.position = hiddenHelpBirdPos.position;
		}

		// Allow Clicking on eggs, puzzle, etc
		if (moveDown || isDown) {
			if (allowClickTimer > 0.05f) { // Skipping 2 frames to avoid disabling the close help button and tapping on an egg at the same time
				scenTapEnabScript.canTapEggRidPanPuz = true; 
				allowClickTimer = 0;
			}
			if (!scenTapEnabScript.canTapEggRidPanPuz) {
				allowClickTimer += Time.deltaTime;
			}
		}
		#endregion

		// Fade out both buttons
		// if (riddleBtnAlpha > 0f)
		// {
		// 	riddleBtnAlpha -= Time.deltaTime / riddleBtnFadeTime;
		// 	//riddleImg.color = new Color(1,1,1, riddleBtnAlpha);
		// 	//hintImg.color = new Color(1,1,1, riddleBtnAlpha);
		// }
		//else if (riddleTextShow && riddleBtnAlpha <= 0)
		//{ // Turn everything off
			//riddleTextShow = false;
			//int random = Random.Range(0, riddleHints.Count);
			//riddleHints[random].SetActive(true);
			//riddleCurntActive = riddleHints[random];
			//dontCloseMenu.SetActive(false);
			//riddleBtn.interactable = false;
			//riddleImg.raycastTarget = false;
			//hintBtn.interactable = false;
			//hintImg.raycastTarget = false;
		//}
	}

	#region Methods
	public void MoveBirdUpDown() 
	{
		if(scenTapEnabScript.canTapHelpBird)
		{
			curHelpBirdPos = helpBirdTrans.position;
			
			if (moveDown || isDown)
			{ // 1 is not necessary since 1 is always the lerp's max value, just there to visualize the rule of three. 
				if (!isDown) { newDuration = duration * lerpValue / 1; } else { newDuration = duration; } 
				lerpValue = 0;
				moveDown = false;
				isDown = false;
				moveUp = true;
				scenTapEnabScript.canTapEggRidPanPuz = false;
				lvlTapManScript.ZoomOutCameraReset();
				if (introDone) { closeMenuOnClick.SetActive(true); }
				//dontCloseMenu.SetActive(true);
				return;
			}
			if (moveUp || isUp)
			{
				if (!isUp) { newDuration = duration * lerpValue / 1; } else { newDuration = duration; }
				lerpValue = 0;
				moveUp = false;
				isUp = false;
				moveDown = true; 
				txtBubFadedIn = false;
			}
		}
	}

	// public void RiddleButton()
	// {
	// 	if (!riddleBtnObj.activeSelf) 
	// 	{
	// 		riddleBtnObj.SetActive(true);
	// 		riddleBtn.enabled = true;
	// 		riddleBtnOn = true;

	// 		if (clickOnEggsScript.goldenEggFound > 0)
	// 		{
	// 			riddleBtn.interactable = false;
	// 		}
	// 		else 
	// 		{
	// 			riddleBtn.interactable = true;
	// 			riddleImg.raycastTarget = true;
	// 		}
	// 	}
	// }

	// public void HintButton()
	// {
	// 	if (!hintBtnObj.activeSelf) 
	// 	{
	// 		hintBtnObj.SetActive(true);
	// 		hintBtnOn = true;
	// 		if (clickOnEggsScript.eggsFound >= clickOnEggsScript.eggs.Count - 1) // -1 for the golden egg
	// 		{
	// 			hintBtn.interactable = false;
	// 		}
	// 		else
	// 		{
	// 			hintBtn.interactable = true;
	// 			hintImg.raycastTarget = true;
	// 		}
	// 	}
	// }

	// public void ShowRiddleText()
	// {
	// 	if (!riddleTextShow)
	// 	{
	// 		riddleBtnAlpha = 1.05f;
	// 		riddleBtn.enabled = false;	
	// 		riddleTextShow = true;
	// 		dontCloseMenu.SetActive(false);
	// 	}
	// }

	// public void StartHint()
	// {
	// 	hintManScript.startHint = true;
	// 	MoveBirdUpDown();
	// }
	#endregion
}
