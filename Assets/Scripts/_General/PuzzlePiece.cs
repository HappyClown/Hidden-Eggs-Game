using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePiece : MonoBehaviour {

	public bool inRotate, inScale;
	public Vector3 rotateTo, rotatePos, rotateNeg;
	public AnimationCurve rotCurve, scaleCurve;
	public float duration;
	public int rotAmnt;
	private float lerpValue, scaleValue;
	private Vector3 iniRot;
	private int rotDirection = 1, rotCount;

	private void Update() {
		if (inRotate) {
			lerpValue += Time.deltaTime / duration;
			this.transform.eulerAngles = Vector3.Lerp(iniRot, rotateTo, rotCurve.Evaluate(lerpValue));
			if (lerpValue >= 1) {
				this.transform.eulerAngles = rotateTo;
				iniRot = this.transform.eulerAngles;
				//rotDirection *= -1;
				//rotateTo *= rotDirection;
				if (rotateTo == rotatePos) {
					rotateTo = rotateNeg;
				}
				else {
					rotateTo = rotatePos;
				}
				rotCount++;
				lerpValue = 0f;
				if (rotCount >= rotAmnt) {
					inRotate = false;
				}
			}
		}
	}


}
