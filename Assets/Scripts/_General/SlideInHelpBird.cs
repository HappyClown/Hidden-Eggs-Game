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
	[Header("Riddle")]
	public GameObject riddleBtnObj;
	private Button riddleBtn;
	private Image riddleImg;
	private float riddleBtnAlpha;
	public float riddleBtnFadeTime;
	private bool riddleShow;
	private GameObject riddleCurntActive;
	public List<GameObject> riddleHints;
	[Header("Hint")]
	public GameObject hintBtnObj;
	public Button hintBtn;
	private Image hintImg;
	public HintManager hintManScript;
	[Header("Zones")]
	public GameObject closeMenuOnClick;
	public GameObject blockClickingOnEggs;
	public GameObject dontCloseMenu;
	[Header("Bird Movement")]
	public float duration;
	public bool moveUp, moveDown, isUp, isDown;
	public Transform hiddenHelpBirdPos, shownHelpBirdPos;
	public Vector3 curHelpBirdPos;
	private float totalDist;
	private float distLeft;
	private float distPercent;
	private float lerpValue;
	public AnimationCurve animCur;
	[Header("Script References")]
	public SceneTapEnabler scenTapEnabScript;
	public ClickOnEggs clickOnEggsScript;
	public LevelTapMannager lvlTapManScript;


	void Start ()
	{
		totalDist = Vector2.Distance(hiddenHelpBirdPos.position, shownHelpBirdPos.position);
		riddleBtn = riddleBtnObj.GetComponent<Button>();
		riddleImg = riddleBtnObj.GetComponent<Image>();
		riddleBtn.onClick.AddListener(ShowRiddleText);
		hintBtn = hintBtnObj.GetComponent<Button>();
		hintImg = hintBtnObj.GetComponent<Image>();
		hintBtn.onClick.AddListener(StartHint);
	}


	void Update ()
	{
		distLeft = Vector2.Distance(this.transform.position, shownHelpBirdPos.position);
		distPercent = (totalDist - distLeft) / totalDist;

		#region Up
		if (moveUp)
		{
			lerpValue += Time.deltaTime / duration;
			this.transform.position = Vector3.Lerp(curHelpBirdPos, shownHelpBirdPos.position, animCur.Evaluate(lerpValue));

			if (shadowAlpha < 1) { shadowAlpha = distPercent; }
			shadow.color = new Color(1,1,1, shadowAlpha);
		}

		// Finished moving up
		if (moveUp && lerpValue >= 1)
		{
			lerpValue = 1;
			isUp = true;
			moveUp = false;
			this.transform.position = shownHelpBirdPos.position;
		}
		// Is all the way up
		if (isUp)
		{
			blockClickingOnEggs.SetActive(true);
			// Fade in text bubble
			if (txtBubAlpha < 1) { txtBubAlpha += Time.deltaTime * txtBubFadeTime; }
			txtBubble.color = new Color(1,1,1, txtBubAlpha);
			// Bird parchment has fully appeared
			if (txtBubAlpha >= 1)
			{	// Make the riddle button appear
				if (!riddleBtnObj.activeSelf) 
				{
					riddleBtnObj.SetActive(true);
					riddleBtn.enabled = true;

					if (clickOnEggsScript.goldenEggFound > 0)
					{
						riddleBtn.interactable = false;
					}
					else 
					{
						riddleBtn.interactable = true;
						riddleImg.raycastTarget = true;
					}
				}
				// Make the hint button appear
				hintBtnObj.SetActive(true);
				if (clickOnEggsScript.eggsFound >= clickOnEggsScript.eggs.Count - 1) // -1 for the golden egg
				{
					hintBtn.interactable = false;
				}
				else
				{
					hintBtn.interactable = true;
					hintImg.raycastTarget = true;
				}
			}
		}
		#endregion

		#region Down
		if (moveDown)
		{
			closeMenuOnClick.SetActive(false);
			dontCloseMenu.SetActive(false);

			lerpValue += Time.deltaTime / duration;
			this.transform.position = Vector3.Lerp(curHelpBirdPos, hiddenHelpBirdPos.position, animCur.Evaluate(lerpValue));

			if (riddleCurntActive) { riddleCurntActive.SetActive(false); }

			if (shadowAlpha > 0) { shadowAlpha = distPercent; }
			shadow.color = new Color(1,1,1, shadowAlpha);

			// Fade out text bubble
			if (txtBubAlpha > 0) { txtBubAlpha -= Time.deltaTime * txtBubFadeTime; }
			txtBubble.color = new Color(1,1,1, txtBubAlpha);
			if (riddleBtnObj.activeSelf) { riddleBtnObj.SetActive(false); }
			if (hintBtnObj.activeSelf) { hintBtnObj.SetActive(false); }
		}

		// Finished moving down
		if (moveDown && lerpValue >= 1)
		{
			lerpValue = 0;
			blockClickingOnEggs.SetActive(false);
			moveDown = false;
			isDown = true;
			this.transform.position = hiddenHelpBirdPos.position;
		}
		#endregion

		// Riddle button alpha
		if (riddleBtnAlpha > 0f)
		{
			riddleBtnAlpha -= Time.deltaTime * riddleBtnFadeTime;
			riddleImg.color = new Color(1,1,1, riddleBtnAlpha);
			hintImg.color = new Color(1,1,1, riddleBtnAlpha);
		}
		else if (riddleShow && riddleBtnAlpha <= 0)
		{
			riddleShow = false;
			int random = Random.Range(0, riddleHints.Count);
			riddleHints[random].SetActive(true);
			riddleCurntActive = riddleHints[random];
			dontCloseMenu.SetActive(false);
			riddleBtn.interactable = false;
			riddleImg.raycastTarget = false;
			hintBtn.interactable = false;
			hintImg.raycastTarget = false;
		}
	}

	#region Methods
	public void MoveBirdUpDown() 
	{
		if(scenTapEnabScript.canTapHelpBird)
		{
			curHelpBirdPos = this.transform.position;
			
			if (moveDown || isDown)
			{
				moveDown = false;
				isDown = false;
				moveUp = true;
				scenTapEnabScript.canTapEggRidPanPuz = false;
				lvlTapManScript.ZoomOutCameraReset();
				closeMenuOnClick.SetActive(true);
				dontCloseMenu.SetActive(true);
				return; 
			}
			if (moveUp || isUp)
			{
				lerpValue = 0 + (1 - lerpValue);
				moveUp = false;
				isUp = false;
				moveDown = true; 
				scenTapEnabScript.canTapEggRidPanPuz = true;
			}
		}
	}

	public void ShowRiddleText()
	{
		if (!riddleShow)
		{
			riddleBtnAlpha = 1.05f;
			riddleBtn.enabled = false;	
			riddleShow = true;
		}
	}

	public void StartHint()
	{
		hintManScript.startHint = true;
		MoveBirdUpDown();
	}
	#endregion
}
