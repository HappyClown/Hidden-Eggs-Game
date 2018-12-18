using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HelpIntroText : MonoBehaviour {
	public List<TextMeshProUGUI> introTMPs;
	public GameObject nextBtnObj;
	private int sentenceCount;
	public int maxSentence;
	public List<Button> helpBirdBtns;
	public SlideInHelpBird slideInHelpScript;

	void Start () {
		nextBtnObj.GetComponent<Button>().onClick.AddListener(NextIntroText);
	}

	public void ShowIntroText() {
		introTMPs[0].gameObject.SetActive(true);
		if (!slideInHelpScript.introDone) {
			nextBtnObj.SetActive(true);
		}
		else {
			nextBtnObj.SetActive(false);
		}
	}

	public void NextIntroText() {
		introTMPs[sentenceCount].gameObject.SetActive(false);
		sentenceCount += 1;
		if (sentenceCount >= maxSentence) {
			slideInHelpScript.introDone = true;
			slideInHelpScript.RiddleButton();
			slideInHelpScript.HintButton();
			slideInHelpScript.closeMenuOnClick.SetActive(true);
			nextBtnObj.SetActive(false);
			TurnOnHelpBtns();
		} else {
			introTMPs[sentenceCount].gameObject.SetActive(true);
		}

	}

	public void TurnOnHelpBtns()
	{
		foreach (Button btn in helpBirdBtns)
		{
			btn.enabled = true;
		}
	}
}