using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryTimeMotions : MonoBehaviour {
	public GameObject normalTime;
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
	public float hoverDuration;
	private float hoverLerpValue;
	public bool hoverUp, hoverDown;
	public Transform topYTrans, botYTrans;
	private float topY, botY;
	private float newXMagnitude;
	public float newXMagMin, newXMagMax;
	public AnimationCurve hoverYAnimCurve;
	public AnimationCurve hoverXAnimCurve;
	public FadeInOutSprite normTimeFadeScript;

	void Start () {
		startPos = normalTime.transform.position;
		startScale = normalTime.transform.localScale.x;
	}
	
	void Update () {
		if (timeMovesIn) {
			if (normTimeFadeScript.hidden) {
				normTimeFadeScript.FadeIn();
			}
			lerpValue += Time.deltaTime / moveInDuration;
			float moveInNewX = Mathf.Lerp(startPos.x, endTrans.position.x, moveInXAnimCurve.Evaluate(lerpValue));
			float moveInNewY = Mathf.Lerp(startPos.y, endTrans.position.y, moveInYAnimCurve.Evaluate(lerpValue));
			normalTime.transform.position = new Vector3(moveInNewX, moveInNewY, normalTime.transform.position.z);
			//normalTime.transform.position = Vector3.Lerp(startPos, endTrans.position, moveInAnimCurve.Evaluate(lerpValue));
			newScale = Mathf.Lerp(startScale, endScale, scaleInAnimCurve.Evaluate(lerpValue));
			normalTime.transform.localScale = new Vector3(newScale, newScale, newScale);
			if (lerpValue >= 1f && !hoverUp) {
				hoverUp = true;
				botY = normalTime.transform.localPosition.y;
				topY = topYTrans.localPosition.y;
				newXMagnitude = Random.Range(newXMagMin, newXMagMax);
			}
			if (lerpValue >= 1) {
				lerpValue = 0;
				timeMovesIn = false;
			}
		}

		if (hoverUp) {
			hoverLerpValue += Time.deltaTime / hoverDuration;
			float newY = Mathf.Lerp(botY, topY, hoverYAnimCurve.Evaluate(hoverLerpValue));
			float newX = hoverXAnimCurve.Evaluate(hoverLerpValue) * newXMagnitude;

			normalTime.transform.position = new Vector3(endTrans.transform.position.x + newX, newY, normalTime.transform.position.z);
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
			
			normalTime.transform.position = new Vector3(endTrans.transform.position.x + newX, newY, normalTime.transform.position.z);
			if(hoverLerpValue >= 1) {
				hoverUp = true;
				hoverDown = false;
				hoverLerpValue = 0;
				newXMagnitude = Random.Range(newXMagMin, newXMagMax);
			}
		}
	}

	public void SetPosMid() {
		normalTime.transform.position = endTrans.position;
	}
}
