using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoryText : MonoBehaviour {
	public List<TextMeshProUGUI> texts;
	public Image parchment;
	public List<float> parchmentWidths;
	public FadeInOutCanvasGroup fadeCanvasScript;
	public int lastTextNum;
	public StoryParchment storyParchScript;
	public AudioManagerHubMenu audioManHubMenuScript;

	void Start(){
		if (!audioManHubMenuScript) {audioManHubMenuScript = GameObject.Find("Audio").GetComponent<AudioManagerHubMenu>();}
	}

	// Makes the correct text fade in.
	public void SetupText (int textNum) {
		lastTextNum = textNum;
		if (textNum > 0) {
			texts[textNum-1].gameObject.SetActive(false);
		}
		else {
			// This is only useful if a user starts a new game twice without entering a different scene or closing the application.
			texts[texts.Count-1].gameObject.SetActive(false);
		}
		texts[textNum].gameObject.SetActive(true);
		parchment.rectTransform.sizeDelta = new Vector2(parchmentWidths[textNum], parchment.rectTransform.sizeDelta.y);
		fadeCanvasScript.FadeIn();
		storyParchScript.OpenParchment(parchmentWidths[textNum]);
		//AUDIO Paper sound
		audioManHubMenuScript.StatPaperSound_on();
	}
	// Fade out the current text and make the next one fade in.
	public void ChangeTextFade(int textNum) {
		lastTextNum = textNum;
		StartCoroutine(TextFadeSameScene());
	}
	IEnumerator TextFadeSameScene () {
		if (fadeCanvasScript.shown) {
			fadeCanvasScript.FadeOut();
		}
		float timer = 0f;
		while (timer < fadeCanvasScript.fadeDuration) {
			timer += Time.deltaTime;
			yield return null;
		}
		if (fadeCanvasScript.hidden) {
			SetupText(lastTextNum);
		}
	}

	// Turn off the text instantly.
	public void TurnTextOff() {
		fadeCanvasScript.canvasG.alpha = 0f;
	}
	// Just fade out the current text.
	public void FadeOutText() {
		fadeCanvasScript.FadeOut();
	}

}
