using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelperBirdHint : MonoBehaviour {
	[Header ("Hint")]
	private bool showHint;
	public GameObject hintBtnObj;
	public Button hintBtn;
	private Image hintImg;
	[Header ("Script References")]
	public HintManager hintManScript;
	public SlideInHelpBird slideInScript;
	public ClickOnEggs clickOnEggsScript;
	public FadeInOutImage hintFadeInOutScript, riddFadeInOutScript;

		public AudioHelperBird audioHelperBirdScript;

	void Start () {
		hintBtn = hintBtnObj.GetComponent<Button>();
		hintImg = hintBtnObj.GetComponent<Image>();
		hintBtn.onClick.AddListener(StartHint);		

		
		
		if(!audioHelperBirdScript){audioHelperBirdScript= GameObject.Find("Audio").GetComponent<AudioHelperBird>();}
	}

	void Update () {
		if (slideInScript.isUp && slideInScript.introDone && !showHint) {
			hintFadeInOutScript.FadeIn();
			if (clickOnEggsScript.eggsFound < clickOnEggsScript.totalRegEggs) {
				hintBtn.interactable = true;
			}
			else {
				hintBtn.interactable = false;
			}
			showHint = true;

		}

		if (!slideInScript.isUp && showHint) {
			hintBtn.interactable = false;
			hintFadeInOutScript.FadeOut();
			showHint = false;
		}
	}

	public void StartHint() {
		if (hintManScript.hintAvailable) {
			hintManScript.startHint = true;

			//sound long
			audioHelperBirdScript.hintSndOnLong();
		}
		slideInScript.MoveBirdUpDown();
	}
}
