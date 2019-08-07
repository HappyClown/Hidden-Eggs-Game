﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompHelpBird : MonoBehaviour {
	[Header ("Settings")]
	public float moveDur;
	public AnimationCurve moveCurve;
	public float bubAdjustDur;
	public float bubSizeA, bubSizeB, bubSizeC;
	[Header ("References")]
	public GameObject helperBird;
	public Transform hiddenTrans, shownTrans;
	public SpriteRenderer textBubSpriteRend;
	public FadeInOutSprite textBubFadeScript;
	public FadeInOutSprite textBubPointerFadeScript;
	public FadeInOutTMP congratsTextFadeScript;
	public FadeInOutCanvasGroup counterCGFadeScript;
	public FadeInOutCanvasGroup backBtnCGFadeScript;
	[Header ("Info")]
	public bool moveUp;
	public bool waitForBubIn, waitForConTxtOut, waitForCountOut;
	private float lerp;
	private bool moveDown, shown, hidden;
	private bool adjustingBubSize;
	private float newBubSize, prevBubSize, curBubSize, bubLerp;

	
	void Update () {
		if (moveUp) {
			lerp += Time.deltaTime / moveDur;
			helperBird.transform.position = Vector3.Lerp(hiddenTrans.position, shownTrans.position, moveCurve.Evaluate(lerp));
			if (lerp >= 1f) {
				helperBird.transform.position = shownTrans.position;
				lerp = 0f;
				moveUp = false;
				waitForBubIn = true;
				textBubFadeScript.FadeIn();
				textBubPointerFadeScript.FadeIn();
				AdjustBubSize(bubSizeA);
			}
		}
		if (waitForBubIn && textBubFadeScript.shown) {
			congratsTextFadeScript.FadeIn();
			waitForBubIn = false;
		}
		if (waitForConTxtOut) {
			if (congratsTextFadeScript.shown) {
				congratsTextFadeScript.FadeOut();
			}
			if (congratsTextFadeScript.hidden && counterCGFadeScript.hidden) {
				AdjustBubSize(bubSizeB);
			}
			if (congratsTextFadeScript.hidden && counterCGFadeScript.hidden && curBubSize == bubSizeB) {
				counterCGFadeScript.FadeIn();
				waitForConTxtOut = false;
			}
		}
		if (waitForCountOut) {
			if (counterCGFadeScript.shown) {
				counterCGFadeScript.FadeOut();
			}
			if (counterCGFadeScript.hidden && backBtnCGFadeScript.hidden) {
				AdjustBubSize(bubSizeC);
			}
			if (counterCGFadeScript.hidden && backBtnCGFadeScript.hidden && curBubSize == bubSizeC) {
				backBtnCGFadeScript.FadeIn();
				waitForConTxtOut = false;
			}
		}

		if (adjustingBubSize) {
			bubLerp += Time.deltaTime / bubAdjustDur;
			curBubSize = Mathf.Lerp(prevBubSize, newBubSize, bubLerp);
			textBubSpriteRend.size = new Vector2(curBubSize, textBubSpriteRend.size.y);
			if (bubLerp >= 1f) {
				bubLerp = 0f;
				curBubSize = newBubSize;
				adjustingBubSize = false;
			}
		}
	}

	void AdjustBubSize(float targetBubSize) {
		if (curBubSize == 0) {
			curBubSize = textBubSpriteRend.size.x;
		}
		prevBubSize = curBubSize;
		newBubSize = targetBubSize;
		adjustingBubSize = true;
	}
}
