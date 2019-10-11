﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryTimeMotions : MonoBehaviour {
	public GameObject normalTime, bewilderedTime, divingTime, glidingTime;
	public GameObject currentTime;
	public Transform bewilderedMidTrans;
	public Vector3 accidentTimeScale;
	private Vector3 timePos;
	private Vector3 iniNormPos, iniNormScale;
	[Header("Move In")]
	public bool timeMovesIn;
	public Transform endTrans;
	private float lerpValue;
	public AnimationCurve moveInYAnimCurve;
	public AnimationCurve moveInXAnimCurve;
	public AnimationCurve scaleInAnimCurve;
	private Vector3 startPos;
	public float endScale;
	private float newScale, startScale;
	public float moveInDuration;
	[Header("Hover")]
	public bool timeHovers;
	public float hoverDuration;
	private float hoverLerpValue;
	private bool hoverUp, hoverDown;
	public Transform topYTrans, botYTrans, smallTopYTrans, smallBotYTrans;
	private float topY, botY;
	private float newXMagnitude;
	public float newXMagMin, newXMagMax;
	public AnimationCurve hoverYAnimCurve;
	public AnimationCurve hoverXAnimCurve;
	public FadeInOutSprite normTimeFadeScript;
	[Header("Spin")]
	public bool timeSpins;
	public float fastSpinDuration, slowSpinDuration, speedDownDuration, rotateAnglePerDur, iniRotateAnglePerDur;
	public AnimationCurve spinAnimCurve;
	private float spinLerpValue, rotateAroundValue, startSpinValue, endSpinValue, halfSpinDuration;
	private bool switchSpinValues, spinCountCheck;
	private int spinCount;
	public bool changeSpinTime;
	[Header("Dive In")]
	public bool timeDives;
	public float diveInDuration, diveHoverDuration, diveOutDuration;
	public AnimationCurve diveInCurve, diveOutCurve;
	public Transform diveStartTrans, diveMidTrans, diveEndTrans;
	private bool diveDelayDone;
	public bool diveIn, diveOut;
	private float diveHoverLerpValue;
	public float hoverCircleDur, hoverRandomRadius;
	private Vector3 circleStartPos, circleEndPos, newPos;
	public bool diveHover, timeDivesThrough;
	public float diveThroughDuration, diveThroughDelay, grabOneEgg;
	public StoryOneEgg storyOneEggScript;
	[Header ("Glide")]
	public bool timeGlides;
	public Transform glideStartTrans, glideEndTrans;
	public float glideDur;
	public AnimationCurve glideAnimCurve;
	public FadeInOutSprite glidingTimeFadeScript;
	public Animator glideAnim;
	[Header ("Audio reference")]
	public AudioIntro audioIntroScript;
	public bool audioSpin = true;

	void Start () {
		currentTime = normalTime;
		startPos = normalTime.transform.position;
		startScale = normalTime.transform.localScale.x;
		startSpinValue = 0f;
		endSpinValue = 180f;
		iniNormPos = normalTime.transform.position;
		iniNormScale = normalTime.transform.localScale;

		if (!audioIntroScript) {audioIntroScript = GameObject.Find("Audio").GetComponent<AudioIntro>();}
	}
	
	void Update () {
		if (timeMovesIn) {
			TimeMovesIn();
		}
		if (timeHovers) {
			TimeHovers();
		}
		if (timeSpins) {
			if (changeSpinTime) {
				ChangeSpinTime();
			}
			TimeSpins();
		}
		if (timeDives) {
			TimeDives();
		}
		if (diveHover) {
			DiveHover();
		}
		if (timeDivesThrough) {
			TimeDivesThrough();
		}
		if (timeGlides) {
			TimeGlides();
		}
	}

	public void SetTimePos(Vector3 timePos, bool stopAllMovements) {
		currentTime.transform.position = timePos;
		if (stopAllMovements) {
			timeMovesIn = timeHovers = timeSpins = timeDives = diveHover = timeDivesThrough = false;
			lerpValue = 0f;
		}
	}

	public void SetTimeScale(bool accidentScale) {
		if (accidentScale) {
			currentTime.transform.localScale = accidentTimeScale;
		}
		else {
			currentTime.transform.localScale = new Vector3(1f, 1f, 1f);
		}
	}

	void TimeMovesIn() {
		if (normTimeFadeScript.hidden) {
			normTimeFadeScript.FadeIn();
		}
		lerpValue += Time.deltaTime / moveInDuration;
		float moveInNewX = Mathf.Lerp(startPos.x, endTrans.position.x, moveInXAnimCurve.Evaluate(lerpValue));
		float moveInNewY = Mathf.Lerp(startPos.y, endTrans.position.y, moveInYAnimCurve.Evaluate(lerpValue));
		currentTime.transform.position = new Vector3(moveInNewX, moveInNewY, currentTime.transform.position.z);
		//currentTime.transform.position = Vector3.Lerp(startPos, endTrans.position, moveInAnimCurve.Evaluate(lerpValue));
		newScale = Mathf.Lerp(startScale, endScale, scaleInAnimCurve.Evaluate(lerpValue));
		currentTime.transform.localScale = new Vector3(newScale, newScale, newScale);

		if (lerpValue >= 1f && !hoverUp) {
			timeHovers = true;
			timePos = endTrans.transform.position;
			botY = currentTime.transform.localPosition.y;
			topY = topYTrans.localPosition.y;
			newXMagnitude = Random.Range(newXMagMin, newXMagMax);
		}
		if (lerpValue >= 1) {
			lerpValue = 0;
			timeMovesIn = false;
		}
	}

	void TimeHovers() {
		if (!hoverUp && !hoverDown) {
			hoverUp = true;
		}
		if (hoverUp) {
			hoverLerpValue += Time.deltaTime / hoverDuration;
			float newY = Mathf.Lerp(botY, topY, hoverYAnimCurve.Evaluate(hoverLerpValue));
			float newX = hoverXAnimCurve.Evaluate(hoverLerpValue) * newXMagnitude;

			currentTime.transform.position = new Vector3(timePos.x + newX, newY, currentTime.transform.position.z);
			if(hoverLerpValue >= 1) {
				hoverUp = false;
				hoverDown = true;
				hoverLerpValue = 0;
				botY = botYTrans.localPosition.y;
				newXMagnitude = Random.Range(newXMagMin, newXMagMax);
			}
		}
		else if (hoverDown) {
			hoverLerpValue += Time.deltaTime / hoverDuration;
			float newY = Mathf.Lerp(topY, botY, hoverYAnimCurve.Evaluate(hoverLerpValue));
			float newX = hoverXAnimCurve.Evaluate(hoverLerpValue) * -newXMagnitude;
			
			currentTime.transform.position = new Vector3(timePos.x + newX, newY, currentTime.transform.position.z);
			if(hoverLerpValue >= 1) {
				hoverUp = true;
				hoverDown = false;
				hoverLerpValue = 0;
				newXMagnitude = Random.Range(newXMagMin, newXMagMax);
			}
		}
	}

	public void SmallTimeHover() {
		botY = smallBotYTrans.position.y;
		topY = smallTopYTrans.position.y;
		newXMagMin /= 2;
		newXMagMax /= 2;
	}

	public void SetupTimeSpin(float spinDuration) {
		halfSpinDuration = spinDuration;
		timeSpins = true;
	}

	void TimeSpins() {
		if (spinLerpValue < 1) {
			spinLerpValue += Time.deltaTime / speedDownDuration;
			rotateAroundValue = Mathf.Lerp(iniRotateAnglePerDur, rotateAnglePerDur, spinAnimCurve.Evaluate(spinLerpValue)) * -1;
		}
		currentTime.transform.RotateAround(currentTime.transform.position, Vector3.up, rotateAroundValue * (Time.deltaTime / halfSpinDuration));

		// Checking to change the currentTime bird after a certain amount of spins.
		if (currentTime.transform.eulerAngles.y <= 90 && spinCountCheck) {
			spinCount++;
			spinCountCheck = false;


			//TEST SYNC SPIN
			if(audioSpin){
				audioIntroScript.introTimeSpinLoopSFX();
			}
			
		}
		if (currentTime.transform.eulerAngles.y >= 90 && !spinCountCheck) {
			spinCountCheck = true;
		} 
	}

	void ChangeSpinTime() {
		if (spinCount >= 1) {
			bewilderedTime.transform.position = currentTime.transform.position;
			bewilderedTime.transform.eulerAngles = new Vector3(0f, 90f, 0f);
			ChangeCurrentTime(bewilderedTime);
			SetTimeScale(true);
			changeSpinTime = false;
		}
	}

	void TimeDives() {
		// if (!diveIn && !diveHover && !diveOut) {
		// 	diveIn = true;
		// }
		if (diveIn) {
			lerpValue += Time.deltaTime / diveInDuration;
			currentTime.transform.position = Vector3.Lerp(diveStartTrans.position, diveMidTrans.position, diveInCurve.Evaluate(lerpValue));
			if (lerpValue >= 1f) {
				lerpValue = 0f;
				timePos = currentTime.transform.position;
				newPos = Random.insideUnitCircle * hoverRandomRadius;
				circleStartPos = currentTime.transform.position;
				circleEndPos = timePos + newPos;
				diveIn = false;
				diveHover = true;
			}
		}
		if (diveHover) {
			//if (lerpValue <= diveHoverDuration) {
				//lerpValue += Time.deltaTime;
			//}
			if (/* lerpValue >= diveHoverDuration &&  */diveOut) {
				timePos = currentTime.transform.position;
				diveHover = false;
				//diveOut = true;
				//lerpValue = 0f;
			}
		}
		if (diveOut) {
			lerpValue += Time.deltaTime / diveOutDuration;
			currentTime.transform.position = Vector3.Lerp(timePos, diveEndTrans.position, diveOutCurve.Evaluate(lerpValue));
			if (lerpValue >= 1) {
				diveOut = false;
				timeDives = false;
				lerpValue = 0f;
			}
		}
	}

	void DiveHover() {
		diveHoverLerpValue += Time.deltaTime / hoverCircleDur;

		//circleEndPos = currentTime.transform.position + newPos;
		currentTime.transform.position = Vector3.Lerp(circleStartPos, circleEndPos, diveHoverLerpValue);
		if (diveHoverLerpValue >= 1) {
			diveHoverLerpValue = 0f;
			newPos = Random.insideUnitCircle * hoverRandomRadius;
			circleStartPos = currentTime.transform.position;
			circleEndPos = timePos + newPos;
		}
	}

	public void SetupDiveHover() {
		timePos = currentTime.transform.position;
		newPos = Random.insideUnitCircle * hoverRandomRadius;
		circleStartPos = currentTime.transform.position;
		circleEndPos = timePos + newPos;
	}

	void TimeDivesThrough() {
		if (!diveDelayDone) {
			lerpValue += Time.deltaTime;
			if (lerpValue >= diveThroughDelay) {
				diveDelayDone = true;
				lerpValue = 0f;
			}
		}
		if (diveDelayDone) {
			lerpValue += Time.deltaTime / diveThroughDuration;
			currentTime.transform.position = Vector3.Lerp(diveStartTrans.position, diveEndTrans.position, lerpValue);
			if (lerpValue >= grabOneEgg && storyOneEggScript.theOneEgg.activeSelf == true) {
				storyOneEggScript.theOneEgg.SetActive(false);

				// // AUDIO - EGG COLLECTED!
				audioIntroScript.SilverEggTrailSFX();
			}
			if (lerpValue >= 1f) {
				lerpValue = 0f;
				timeDivesThrough = false;
				diveDelayDone = false;
			}
		}
	}

	void TimeGlides() {
		lerpValue += Time.deltaTime / glideDur;
		currentTime.transform.position = Vector3.Lerp(glideStartTrans.position, glideEndTrans.position, glideAnimCurve.Evaluate(lerpValue));
		if (lerpValue >= 1f) {
			lerpValue = 0f;
			timeGlides = false;
			glideAnim.SetTrigger("StartHovering");
		}
	}

	public void ChangeCurrentTime(GameObject whichTime) {
		currentTime = whichTime;
		normalTime.SetActive(false);
		bewilderedTime.SetActive(false);
		divingTime.SetActive(false);
		glidingTime.SetActive(false);
		if (currentTime != null) {
			currentTime.SetActive(true);
		}
	}

	public void FadeOutGlidingTime() {
		glidingTimeFadeScript.FadeOut();
	}

	public void ResetNormalTime() {
		normalTime.transform.position = iniNormPos;
		normalTime.transform.localScale = iniNormScale;
		normTimeFadeScript.FadeOut();
	}
}