﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoryText : MonoBehaviour {
	public List<TextMeshProUGUI> texts;
	public Image parchment;
	public List<float> parchmentWidths;
	public FadeInOutCanvasGroup fadeCanvasScript;
	//[Header("Change Text Fade")]
	private bool changeText;
	private int lastTextNum;

	void Update () {
		if (changeText) {
			if (fadeCanvasScript.shown) {
				fadeCanvasScript.FadeOut();
			}
			if (fadeCanvasScript.hidden) {
				SetupText(lastTextNum);
				changeText = false;
			}
		}
	}
	// Makes the correct text fade in.
	public void SetupText (int textNum) {
		lastTextNum = textNum;
		if (textNum > 0) {
			texts[textNum-1].gameObject.SetActive(false);
		}
		texts[textNum].gameObject.SetActive(true);
		parchment.rectTransform.sizeDelta = new Vector2(parchmentWidths[textNum], parchment.rectTransform.sizeDelta.y);
		fadeCanvasScript.FadeIn();
	}
	// Fades out the current text and makes the next one fade in.
	public void ChangeTextFade(int textNum) {
		lastTextNum = textNum;
		changeText = true;
	}
	// Turn off the text immediately.
	public void TurnTextOff() {
		fadeCanvasScript.canvasG.alpha = 0f;
	}

}