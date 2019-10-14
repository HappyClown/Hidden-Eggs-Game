using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryIcons : MonoBehaviour {
	public float duration;
	public float minScale, maxScale;
	public FadeInOutSprite nextIconFadeScript;
	public Transform nextIconTrans;
	private bool scaling, scaleDown, scaleUp;
	private float timer;
	private float startScale, targetScale, newScale;

	[Header ("Button version")]
	public FadeInOutCanvasGroup btnCGFadeScript;


	void Update () {
		if (scaling) {
			timer += Time.deltaTime / duration;
			newScale = Mathf.Lerp(startScale, targetScale, timer);
			nextIconTrans.localScale = new Vector3(newScale, newScale, newScale);
			if (timer >= 1f) {
				timer = 0f;
				if (scaleUp) {
					nextIconTrans.localScale = new Vector3(maxScale, maxScale, maxScale);
					startScale = maxScale;
					targetScale = minScale;
					scaleUp = false;
					scaleDown = true;
					return;
				}
				if (scaleDown) {
					nextIconTrans.localScale = new Vector3(minScale, minScale, minScale);
					startScale = minScale;
					targetScale = maxScale;
					scaleDown = false;
					scaleUp = true;
				}
			}
		}
	}
	
	public void ShowNextIcon() {
		nextIconFadeScript.FadeIn();
		nextIconTrans.localScale = new Vector3(minScale, minScale, minScale);
		scaling = true;
		scaleDown = false;
		scaleUp = true;
		startScale = minScale;
		targetScale = maxScale;
	}

	public void ShowNextButton() {
		btnCGFadeScript.FadeIn();
		nextIconTrans.localScale = new Vector3(minScale, minScale, minScale);
		scaling = true;
		scaleDown = false;
		scaleUp = true;
		startScale = minScale;
		targetScale = maxScale;
	}

	public void HideNextIcon() {
		nextIconFadeScript.FadeOut();
	}

	public void HideNextButton() {
		btnCGFadeScript.FadeOut();
	}
}
