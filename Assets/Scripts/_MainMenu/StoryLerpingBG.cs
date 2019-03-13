using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryLerpingBG : MonoBehaviour {
	private float lerpValue;
	public Vector3 lerpStartPos;
	public Transform startTrans;
	public Transform endTrans;
	public bool lerp;
	public float moveSpeed;
	public bool amIFirst;
	private float fullMoveSpeedValue, moveSpeedValue;
	public float fullMoveSpeedDuration;
	public AnimationCurve moveSpeedCurve;

	void Start () {
		lerpStartPos = this.transform.position;
		if (amIFirst) {
			moveSpeed = moveSpeed * 2;
		}
	}
	
	void Update () {
		if (lerp) {
			if (fullMoveSpeedValue < 1) {
				fullMoveSpeedValue += Time.deltaTime / fullMoveSpeedDuration;
				moveSpeedValue = Mathf.Lerp(0, moveSpeed, moveSpeedCurve.Evaluate(fullMoveSpeedValue));
			}
			lerpValue += Time.deltaTime * moveSpeedValue;
			this.transform.position = Vector3.Lerp(lerpStartPos, endTrans.position, lerpValue);
			if (lerpValue >= 1) {
				if (amIFirst) {
					moveSpeedValue = moveSpeedValue / 2;
					amIFirst = false;
				}
				this.transform.position = startTrans.position;
				lerpStartPos = startTrans.position;
				lerpValue = 0;
			}
		}
	}
}
