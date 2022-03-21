using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryIcons : MonoBehaviour {
	public float duration;
	public float minScale, maxScale;
	public Transform nextIconTrans;
	private bool scaling, scaleDown, scaleUp;
	private float timer;
	private float startScale, targetScale, newScale;

	[Header ("Button version")]
	public FadeInOutCanvasGroup btnCGFadeScript;

	// This Update method is allowed to live because the Next button is almost always on and is always scaling when it is on.
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

	public void ShowNextButton() {
		btnCGFadeScript.FadeIn();
		nextIconTrans.localScale = new Vector3(minScale, minScale, minScale);
		scaling = true;
		scaleDown = false;
		scaleUp = true;
		startScale = minScale;
		targetScale = maxScale;
	}

	public void HideNextButton() {
		btnCGFadeScript.FadeOut();
	}
}
