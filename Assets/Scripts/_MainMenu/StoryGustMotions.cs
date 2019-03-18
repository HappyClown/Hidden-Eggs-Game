using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryGustMotions : MonoBehaviour {
	[Header("References")]
	public GameObject gust;
	public Transform startTrans, midTrans, endTrans;
	[Header("X Move")]
	public AnimationCurve moveInXCurve; 
	public AnimationCurve moveOutXCurve;
	public float moveInDur, moveOutDur;
	[Header("Y Hover")]
	public AnimationCurve hoverYCurve;
	public float hoverUpDownDurMin, hoverUpDownDurMax;
	public float hoverYMultMin, hoverYMultMax;
	public bool hoverWithCircle;
	public float hoverRandomRadius, hoverCircleDur;
	private float lerpValueTwo;
	public bool yHover;
	// Generic Variables
	private bool hoverHoverUp;
	private float startY, hoverYMult, hoverUpDownDur, newX, newY;
	private Vector3 newPos, circleStartPos, circleEndPos;
	private float lerpValue;
	private bool xMove, backToStartPos;
	private float startX, endX, duration;
	private AnimationCurve animCurve;

	void Start () {
		startY = gust.transform.position.y;
		hoverUpDownDur = Random.Range(hoverUpDownDurMin, hoverUpDownDurMax);
		hoverYMult = Random.Range(hoverYMultMin, hoverYMultMax);
	}
	
	void Update () {
		// if (moveMid) {
		// 	lerpValue += Time.deltaTime / moveInDur;
		// 	newX = Mathf.Lerp(startTrans.position.x, midTrans.position.x, moveInXCurve.Evaluate(lerpValue));

		// 	gust.transform.position = new Vector3(newX, startY + newY, gust.transform.position.z);
		// 	if (lerpValue >= 1f) {
		// 		moveMid = false;
		// 		yHover = true;
		// 		lerpValue = 0f;
		// 		newPos = Random.insideUnitCircle * hoverRandomRadius;
		// 		circleStartPos = gust.transform.position;
		// 		circleEndPos = midTrans.position + newPos;
		// 	}
		// }

		// if (moveMidEnd) {
		// 	lerpValue += Time.deltaTime / moveInDur;
		// 	newX = Mathf.Lerp(midTrans.position.x, endTrans.position.x, moveInXCurve.Evaluate(lerpValue));

		// 	gust.transform.position = new Vector3(newX, startY + newY, gust.transform.position.z);
		// 	if (lerpValue >= 1f) {
		// 		moveMidEnd = false;
		// 		lerpValue = 0f;
		// 		gust.transform.position = startTrans.position;
		// 	}
		// }


		if (xMove) {
			lerpXMove();
		}
		if (yHover) {
			YHover();
		}
	}

	public void SetupXMove(float lerpStartX, float lerpEndX, float lerpDuration, AnimationCurve lerpAnimCurve, bool goBackToStartPos) {
		startX = lerpStartX;
		endX = lerpEndX;
		duration = lerpDuration;
		animCurve = lerpAnimCurve;
		backToStartPos = goBackToStartPos;
		xMove = true;
	}

	void lerpXMove() {
		lerpValue += Time.deltaTime / duration;
		newX = Mathf.Lerp(startX, endX, animCurve.Evaluate(lerpValue));
		gust.transform.position = new Vector3(newX, gust.transform.position.y, gust.transform.position.z);

		if (lerpValue >= 1f) {
			xMove = false;
			lerpValue = 0f;
			if (backToStartPos) {
				gust.transform.position = startTrans.position;
			}
		}
	}

	void YHover() {
		if (!hoverWithCircle) {
				lerpValueTwo += Time.deltaTime / hoverUpDownDur;
				newY = hoverYCurve.Evaluate(lerpValueTwo) * hoverYMult;
				gust.transform.position = new Vector3(gust.transform.position.x, startY + newY, gust.transform.position.z);
				if (lerpValueTwo >= 1) {
					lerpValueTwo = 0f;
					hoverUpDownDur = Random.Range(hoverUpDownDurMin, hoverUpDownDurMax);
					hoverYMult = Random.Range(hoverYMultMin, hoverYMultMax);
					hoverYMult = hoverYMult * -1;
				}
			}
			else {
				lerpValue += Time.deltaTime / hoverCircleDur;
				gust.transform.position = Vector3.Lerp(circleStartPos, circleEndPos, lerpValue);
				if (lerpValue >= 1) {
					lerpValue = 0f;
					newPos = Random.insideUnitCircle * hoverRandomRadius;
					circleStartPos = gust.transform.position;
					circleEndPos = midTrans.position + newPos;
				}
			}
	}
}
