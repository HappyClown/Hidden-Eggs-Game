using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryGustMotions : MonoBehaviour {
	[Header("References")]
	public GameObject gust;
	public FadeInOutSprite gustFadeScript;
	public Transform startTrans, midTrans, endTrans, topEndTrans;
	public GameObject gustEyes;
	public Transform lookFrontPos, lookBackPos;
	[Header("X Move")]
	public AnimationCurve moveInXCurve; 
	public AnimationCurve moveOutXCurve;
	public AnimationCurve moveAcrossCurve;
	[Header("Y Move")]
	public AnimationCurve moveOutTopYCurve;
	[Header ("Durations")]
	public float moveInDur;
	public float moveOutDur, moveAcrossDur;
	[Header("Y Hover")]
	public AnimationCurve hoverYCurve;
	public float hoverUpDownDurMin, hoverUpDownDurMax;
	public float hoverYMultMin, hoverYMultMax;
	public bool hoverWithCircle;
	public float hoverRandomRadius, hoverCircleDur;
	private float lerpValueHover, hoverYMultAdjust = 1f;
	public bool yHover;
	[Header ("Scale")]
	public float moveOutScale;
	public bool scaleDown;
	private float newScale;
	// Generic Variables
	private bool hoverHoverUp;
	private float iniYPos, hoverIniPos, hoverYMult, hoverUpDownDur, newX, newY, hoverNewY;
	private Vector3 newPos, circleStartPos, circleEndPos;
	private float lerpValueX, lerpValueY, lerpValueScale;
	private bool xMove, yMove, backToStartPos;
	private float startX, endX, startY, endY, startScale, endScale, durationX, durationY, durationScale;
	private AnimationCurve animCurveX, animCurveY, animCurveScale;
	[Header("Collision with Time")]
	public float gustCollisionScale;
	private float iniScale;

	void Start () {
		iniScale = gust.transform.localScale.x;
		iniYPos = gust.transform.position.y;
		hoverUpDownDur = Random.Range(hoverUpDownDurMin, hoverUpDownDurMax);
		hoverYMult = Random.Range(hoverYMultMin, hoverYMultMax);
	}
	
	void Update () {
		if (xMove) {
			LerpXMove();
		}
		if (yMove) {
			LerpYMove();
		}
		if (yHover) {
			YHover();
		}
		if (scaleDown) {
			ScaleDown();
		}
	}

	public void SetupXMove(float lerpStart, float lerpEnd, float lerpDuration, AnimationCurve lerpAnimCurve/* , bool goBackToStartPos */) {
		startX = lerpStart;
		endX = lerpEnd;
		durationX = lerpDuration;
		animCurveX = lerpAnimCurve;
		//backToStartPos = goBackToStartPos;
		xMove = true;
	}
	public void SetupYMove(float lerpStart, float lerpEnd, float lerpDuration, AnimationCurve lerpAnimCurve/* , bool goBackToStartPos */) {
		startY = lerpStart;
		endY = lerpEnd;
		durationY = lerpDuration;
		animCurveY = lerpAnimCurve;
		//backToStartPos = goBackToStartPos;
		hoverIniPos = iniYPos;
		yMove = true;
	}

	void LerpXMove() {
		lerpValueX += Time.deltaTime / durationX;
		newX = Mathf.Lerp(startX, endX, animCurveX.Evaluate(lerpValueX));
		gust.transform.position = new Vector3(newX, gust.transform.position.y, gust.transform.position.z);

		if (lerpValueX >= 1f) {
			xMove = false;
			lerpValueX = 0f;
			// if (backToStartPos) {
			// 	gust.transform.position = startTrans.position;
			// }
		}
	}
	void LerpYMove() {
		lerpValueY += Time.deltaTime / durationY;
		newY = Mathf.Lerp(0, endY, animCurveY.Evaluate(lerpValueY));
		//gust.transform.position = new Vector3(gust.transform.position.x, hoverIniPos + newY, gust.transform.position.z);
		iniYPos = hoverIniPos + newY;
		//iniYPos = gust.transform.position.y;
		if (lerpValueY >= 1f) {
			yMove = false;
			lerpValueY = 0f;
			// if (backToStartPos) {
			// 	gust.transform.position = startTrans.position;
			// }
		}
	}

	void YHover() {
		if (!hoverWithCircle) {
				lerpValueHover += Time.deltaTime / hoverUpDownDur;
				hoverNewY = hoverYCurve.Evaluate(lerpValueHover) * hoverYMult;
				gust.transform.position = new Vector3(gust.transform.position.x, iniYPos + hoverNewY, gust.transform.position.z);
				if (lerpValueHover >= 1) {
					lerpValueHover = 0f;
					hoverUpDownDur = Random.Range(hoverUpDownDurMin, hoverUpDownDurMax);
					hoverYMult = Random.Range(hoverYMultMin, hoverYMultMax);
					hoverYMult *= -hoverYMultAdjust;
				}
		}
		else {
			lerpValueHover += Time.deltaTime / hoverCircleDur;
			gust.transform.position = Vector3.Lerp(circleStartPos, circleEndPos, lerpValueHover);
			if (lerpValueHover >= 1) {
				lerpValueHover = 0f;
				newPos = Random.insideUnitCircle * hoverRandomRadius;
				circleStartPos = gust.transform.position;
				circleEndPos = midTrans.position + newPos;
			}
		}
	}

	public void SetupScaleDown(float lerpStart, float lerpEnd, float lerpDuration, AnimationCurve lerpAnimCurve) {
		startScale = lerpStart;
		endScale = lerpEnd;
		durationScale = lerpDuration;
		animCurveScale = lerpAnimCurve;
		scaleDown = true;
	}
	void ScaleDown() {
		lerpValueScale += Time.deltaTime / durationScale;
		newScale = Mathf.Lerp(startScale, endScale, animCurveScale.Evaluate(lerpValueScale));
		gust.transform.localScale = new Vector3(newScale, newScale, newScale);
		if (lerpValueScale >= 1) {
			lerpValueScale = 0f;
			scaleDown = false;
		}
	}

	public void ChangeEyePos() {
		// Change gust's eye position based on where they are now. Could be a bool.
		if (Vector2.Distance(gustEyes.transform.position, lookFrontPos.position) < 0.1f) {
			gustEyes.transform.position = lookBackPos.position;
		}
		else {
			gustEyes.transform.position = lookFrontPos.position;
		}
	}
	public void ChangeScale() {
		//Change gust's scale based on its current scale. Could be a bool.
		if (Mathf.Abs(gust.transform.localScale.x - iniScale) <= 0.1f) {
			gust.transform.localScale = new Vector3(gustCollisionScale, gustCollisionScale, gustCollisionScale);
		}
		else {
			gust.transform.localScale = new Vector3(iniScale, iniScale, iniScale);
		}
	}
}
