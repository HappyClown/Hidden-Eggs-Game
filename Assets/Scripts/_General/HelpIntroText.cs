using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HelpIntroText : MonoBehaviour {
	public TextMeshProUGUI introTMP;
	public GameObject nextBtnObj;
	private int sentenceCount;
	public int maxSentence;
	public SlideInHelpBird slideInHelpScript;

	void Start () {
		nextBtnObj.GetComponent<Button>().onClick.AddListener(NextIntroText);
	}
	
	void Update () {
		
	}

	public void ShowIntroText() {
		introTMP.gameObject.SetActive(true);
		if (!slideInHelpScript.introDone) {
			nextBtnObj.SetActive(true);
		}
		else {
			nextBtnObj.SetActive(false);
		}
	}

	public void NextIntroText() {
		sentenceCount += 1;
		if (sentenceCount >= maxSentence) {
			slideInHelpScript.introDone = true;
			slideInHelpScript.RiddleButton();
			slideInHelpScript.HintButton();
			nextBtnObj.SetActive(false);
			introTMP.gameObject.SetActive(false);
		}
	}
}