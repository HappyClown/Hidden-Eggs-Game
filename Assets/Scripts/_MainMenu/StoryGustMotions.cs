using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryGustMotions : MonoBehaviour {
	[Header("References")]
	public GameObject gust;
	public Transform startTrans, midTrans, endTrans;
	[Header("Move In")]
	public bool moveMid;
	public AnimationCurve moveInXCurve, moveInYCurve;
	public float moveInDur;
	public float moveInUpDownDurMin, moveInUpDownDurMax;
	public float moveInYMultMin, moveInYMultMax;
	public bool midHover, hoverWithCircle;
	public float hoverRandomRadius, hoverDur;
	private float startY, moveInYMult, moveInUpDownDur;
	private bool moveInHoverUp;
	private Vector3 newPos, circleStartPos, circleEndPos;
	[Header("Move Out")]
	public bool moveEnd;
 	public float moveOutDur;
	private float lerpValue, lerpValueTwo;

	void Start () {
		startY = gust.transform.position.y;
		moveInUpDownDur = Random.Range(moveInUpDownDurMin, moveInUpDownDurMax);
		moveInYMult = Random.Range(moveInYMultMin, moveInYMultMax);
	}
	
	void Update () {
		if (Input.GetKeyDown("space")) { // DELETE MEEEE!!! PlleaaAAaasseEeee..eeEe.... :(
			moveMid = true;
		}
		if (moveMid) {
			lerpValue += Time.deltaTime / moveInDur;
			float newX = Mathf.Lerp(startTrans.position.x, midTrans.position.x, moveInXCurve.Evaluate(lerpValue));

			lerpValueTwo += Time.deltaTime / moveInUpDownDur;
			float newY = moveInYCurve.Evaluate(lerpValueTwo) * moveInYMult;
			if (lerpValueTwo >= 1) {
				lerpValueTwo = 0f;
				moveInUpDownDur = Random.Range(moveInUpDownDurMin, moveInUpDownDurMax);
				moveInYMult = Random.Range(moveInYMultMin, moveInYMultMax);
				moveInYMult = moveInYMult * -1;
			}

			gust.transform.position = new Vector3(newX, startY + newY, gust.transform.position.z);
			if (lerpValue >= 1f) {
				moveMid = false;
				midHover = true;
				lerpValue = 0f;
				newPos = Random.insideUnitCircle * hoverRandomRadius;
				circleStartPos = gust.transform.position;
				circleEndPos = midTrans.position + newPos;
			}
		}
		if (midHover) {
			if (!hoverWithCircle) {
				lerpValueTwo += Time.deltaTime / moveInUpDownDur;
				float newY = moveInYCurve.Evaluate(lerpValueTwo) * moveInYMult;
				gust.transform.position = new Vector3(gust.transform.position.x, startY + newY, gust.transform.position.z);
				if (lerpValueTwo >= 1) {
					lerpValueTwo = 0f;
					moveInUpDownDur = Random.Range(moveInUpDownDurMin, moveInUpDownDurMax);
					moveInYMult = Random.Range(moveInYMultMin, moveInYMultMax);
					moveInYMult = moveInYMult * -1;
				}
			}
			else {
				lerpValue += Time.deltaTime / hoverDur;
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
}
