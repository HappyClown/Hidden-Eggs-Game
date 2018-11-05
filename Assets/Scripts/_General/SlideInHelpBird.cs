using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideInHelpBird : MonoBehaviour 
{
	public Transform hiddenHelpBirdPos;
	public Transform shownHelpBirdPos;

	public GameObject shadow;
	public float shadowAlpha;

	public GameObject txtBubble;
	private float txtBubAlpha;
	public float txtBubFadeTime;

	public GameObject riddleBtnObj;
	private Button riddleBtn;
	private Image riddleImg;
	private float riddleBtnAlpha;
	public float riddleBtnFadeTime;
	private bool riddleShow;
	private GameObject riddleCurntActive;

	public GameObject hintBtnObj;
	public Button hintBtn;
	private Image hintImg;
	public HintManager hintManScript;

	public GameObject closeMenuOnClick;
	public GameObject blockClickingOnEggs;
	public GameObject dontCloseMenu;

	public List<GameObject> riddleHints;

	public bool moveUp = false;

	public float speed;

	private float totalDist;
	private float distLeft;
	private float distPercent;

	public SceneTapEnabler scenTapEnabScript;
	public ClickOnEggs clickOnEggsScript;
	


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

		if (moveUp == true)
		{
			this.transform.position = Vector3.MoveTowards(this.transform.position, shownHelpBirdPos.position, speed * Time.deltaTime);

			if (shadowAlpha < 1) { shadowAlpha = distPercent; }
			shadow.GetComponent<Image>().color = new Color(1,1,1, shadowAlpha);
		}
		
		if (moveUp == false)
		{
			closeMenuOnClick.SetActive(false);
			dontCloseMenu.SetActive(false);
			this.transform.position = Vector3.MoveTowards(this.transform.position, hiddenHelpBirdPos.position, speed * Time.deltaTime);

			if (riddleCurntActive) { riddleCurntActive.SetActive(false); }

			if (shadowAlpha > 0) { shadowAlpha = distPercent; }
			shadow.GetComponent<Image>().color = new Color(1,1,1, shadowAlpha);
		}

		if (moveUp == false && Vector2.Distance(this.transform.position, hiddenHelpBirdPos.position) <= 0.1f)
		{
			blockClickingOnEggs.SetActive(false);
		}


		if (moveUp == true && Vector2.Distance(this.transform.position, shownHelpBirdPos.position) <= 0.1f)
		{
			blockClickingOnEggs.SetActive(true);

			if (txtBubAlpha < 1) { txtBubAlpha += Time.deltaTime * txtBubFadeTime; }
			txtBubble.GetComponent<Image>().color = new Color(1,1,1, txtBubAlpha);
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
		}
		else
		{
			if (txtBubAlpha > 0) { txtBubAlpha -= Time.deltaTime * txtBubFadeTime; }
			txtBubble.GetComponent<Image>().color = new Color(1,1,1, txtBubAlpha);
			if (riddleBtnObj.activeSelf) { riddleBtnObj.SetActive(false); }
			if (hintBtnObj.activeSelf) { hintBtnObj.SetActive(false); }
		}

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


	public void MoveBirdUp() 
	{
		if(scenTapEnabScript.canTapHelpBird)
		{
			if (moveUp == false)
			{ 
				moveUp = true; 
				scenTapEnabScript.canTapEggRidPanPuz = false;
				closeMenuOnClick.SetActive(true);
				dontCloseMenu.SetActive(true);
				return; 
			}
			else if (moveUp == true)
			{
				moveUp = false; 
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
		MoveBirdUp();
	}
}
