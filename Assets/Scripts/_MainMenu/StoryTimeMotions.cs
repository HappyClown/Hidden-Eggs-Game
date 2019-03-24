using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryTimeMotions : MonoBehaviour {
	public GameObject normalTime, bewilderedTime, divingTime;
	public GameObject currentTime;
	public Transform bewilderedMidTrans;
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
	public Transform topYTrans, botYTrans;
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
	private bool switchSpinValues;

	void Start () {
		currentTime = normalTime;
		startPos = normalTime.transform.position;
		startScale = normalTime.transform.localScale.x;
		startSpinValue = 0f;
		endSpinValue = 180f;
	}
	
	void Update () {
		// if (timeMovesIn) {
		// 	if (normTimeFadeScript.hidden) {
		// 		normTimeFadeScript.FadeIn();
		// 	}
		// 	lerpValue += Time.deltaTime / moveInDuration;
		// 	float moveInNewX = Mathf.Lerp(startPos.x, endTrans.position.x, moveInXAnimCurve.Evaluate(lerpValue));
		// 	float moveInNewY = Mathf.Lerp(startPos.y, endTrans.position.y, moveInYAnimCurve.Evaluate(lerpValue));
		// 	normalTime.transform.position = new Vector3(moveInNewX, moveInNewY, normalTime.transform.position.z);
		// 	//normalTime.transform.position = Vector3.Lerp(startPos, endTrans.position, moveInAnimCurve.Evaluate(lerpValue));
		// 	newScale = Mathf.Lerp(startScale, endScale, scaleInAnimCurve.Evaluate(lerpValue));
		// 	normalTime.transform.localScale = new Vector3(newScale, newScale, newScale);
		// 	if (lerpValue >= 1f && !hoverUp) {
		// 		hover = true;
		// 		botY = normalTime.transform.localPosition.y;
		// 		topY = topYTrans.localPosition.y;
		// 		newXMagnitude = Random.Range(newXMagMin, newXMagMax);
		// 	}
		// 	if (lerpValue >= 1) {
		// 		lerpValue = 0;
		// 		timeMovesIn = false;
		// 	}
		// }
		// if (hoverUp) {
		// 	hoverLerpValue += Time.deltaTime / hoverDuration;
		// 	float newY = Mathf.Lerp(botY, topY, hoverYAnimCurve.Evaluate(hoverLerpValue));
		// 	float newX = hoverXAnimCurve.Evaluate(hoverLerpValue) * newXMagnitude;

		// 	normalTime.transform.position = new Vector3(endTrans.transform.position.x + newX, newY, normalTime.transform.position.z);
		// 	if(hoverLerpValue >= 1) {
		// 		hoverUp = false;
		// 		hoverDown = true;
		// 		hoverLerpValue = 0;
		// 		botY = botYTrans.localPosition.y;
		// 		newXMagnitude = Random.Range(newXMagMin, newXMagMax);
		// 	}
		// }
		// else if (hoverDown) {
		// 	hoverLerpValue += Time.deltaTime / hoverDuration;
		// 	float newY = Mathf.Lerp(topY, botY, hoverYAnimCurve.Evaluate(hoverLerpValue));
		// 	float newX = hoverXAnimCurve.Evaluate(hoverLerpValue) * -newXMagnitude;
			
		// 	normalTime.transform.position = new Vector3(endTrans.transform.position.x + newX, newY, normalTime.transform.position.z);
		// 	if(hoverLerpValue >= 1) {
		// 		hoverUp = true;
		// 		hoverDown = false;
		// 		hoverLerpValue = 0;
		// 		newXMagnitude = Random.Range(newXMagMin, newXMagMax);
		// 	}
		// }
		if (timeMovesIn) {
			TimeMovesIn();
		}
		if (timeHovers) {
			Hover();
		}
		if (timeSpins) {
			TimeSpins();
		}
	}

	public void SetTimePos(Vector3 timePos) {
		currentTime.transform.position = timePos;
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
			botY = currentTime.transform.localPosition.y;
			topY = topYTrans.localPosition.y;
			newXMagnitude = Random.Range(newXMagMin, newXMagMax);
		}
		if (lerpValue >= 1) {
			lerpValue = 0;
			timeMovesIn = false;
		}
	}

	void Hover() {
		if (!hoverUp && !hoverDown) {
			hoverUp = true;
		}
		if (hoverUp) {
			hoverLerpValue += Time.deltaTime / hoverDuration;
			float newY = Mathf.Lerp(botY, topY, hoverYAnimCurve.Evaluate(hoverLerpValue));
			float newX = hoverXAnimCurve.Evaluate(hoverLerpValue) * newXMagnitude;

			currentTime.transform.position = new Vector3(endTrans.transform.position.x + newX, newY, currentTime.transform.position.z);
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
			
			currentTime.transform.position = new Vector3(endTrans.transform.position.x + newX, newY, currentTime.transform.position.z);
			if(hoverLerpValue >= 1) {
				hoverUp = true;
				hoverDown = false;
				hoverLerpValue = 0;
				newXMagnitude = Random.Range(newXMagMin, newXMagMax);
			}
		}
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

		// spinLerpValue += Time.deltaTime / halfSpinDuration;
		// float spinCurveValue = Mathf.Lerp(startSpinValue, endSpinValue, spinAnimCurve.Evaluate(spinLerpValue));
		// currentTime.transform.eulerAngles = new Vector3(currentTime.transform.localRotation.x, spinCurveValue, currentTime.transform.localRotation.z);
		// if (spinLerpValue >= 1) {
		// 	spinLerpValue = 0f;
		// 	if (!switchSpinValues) {
		// 		startSpinValue = 0f;
		// 		endSpinValue = 180f;
		// 		switchSpinValues = true;
		// 	}
		// 	else {
		// 		startSpinValue = 180f;
		// 		endSpinValue = 360f;
		// 		switchSpinValues = false;
		// 	}
		// }
	}

	public void ChangeCurrentTime(GameObject whichTime) {
		currentTime = whichTime;
		normalTime.SetActive(false);
		bewilderedTime.SetActive(false);
		currentTime.SetActive(true);
	}
}
