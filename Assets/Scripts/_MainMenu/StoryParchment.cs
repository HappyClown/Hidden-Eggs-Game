using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryParchment : MonoBehaviour {
	private float timer;
	public float duration;
	public RectTransform parchmentRect;
	private float curWidth, minWidth, maxWidth;
	private bool opening, open, closing, closed;
	public AnimationCurve widthCurve;
	
	void Update () {
		if (opening) {
			timer += Time.deltaTime / duration;
			curWidth = Mathf.Lerp(0, maxWidth, widthCurve.Evaluate(timer));
			parchmentRect.sizeDelta = new Vector2(curWidth, parchmentRect.sizeDelta.y);
			if (timer >= 1f) {
				opening = false;
				timer = 0f;
				open = true;
			}
		}
		if (closing) {
			timer += Time.deltaTime / duration;
			curWidth = Mathf.Lerp(maxWidth, 0, widthCurve.Evaluate(timer));
			parchmentRect.sizeDelta = new Vector2(curWidth, parchmentRect.sizeDelta.y);
			if (timer >= 1f) {
				closing = false;
				timer = 0f;
				closed = true;
			}
		}
	}

	public void OpenParchment(float thisTextWidth) {
		timer = 0f;
		open = false;
		closed = false;
		closing = false;
		opening = true;
		maxWidth = thisTextWidth;
	}

	public void ParchmentClosed() {
		open = false;
		closed = true;
		closing = false;
		opening = false;
		parchmentRect.sizeDelta = new Vector2(0, parchmentRect.sizeDelta.y);
		timer = 0f;
	}
}
