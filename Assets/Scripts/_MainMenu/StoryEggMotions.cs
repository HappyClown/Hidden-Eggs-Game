using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryEggMotions : MonoBehaviour {
	public Transform eggSpawnTrans;
	public GameObject bewilderedTime;
	public float eggFlyDuration;
	public AnimationCurve yAnimCurve;
	public float yMult, yMoveDist;
	public AnimationCurve xAnimCurve;
	public float xMult, xMoveDist;

	public float minScale, maxScale;
	public float minEggAngle, maxEggAngle;

	private bool spawn;
	private float lerpValue, startY, endY, startX, endX, newX, newY, newEggAngle;


	void Start () {
		
	}
	
	void Update () {
		if (spawn) {
			EggMotion();
		}
	}

	public void SpawnEgg() {
		spawn = true;
		if (bewilderedTime.transform.eulerAngles.y > 90 && bewilderedTime.transform.eulerAngles.y < 270/*  || bewilderedTime.transform.eulerAngles.y < -90 && bewilderedTime.transform.eulerAngles.y >- 180 */) {
			xMoveDist = Mathf.Abs(xMoveDist) * -1;
		}
		else {
			xMoveDist = Mathf.Abs(xMoveDist);
		}
		newEggAngle = Random.Range(minEggAngle, maxEggAngle);
		this.transform.localEulerAngles = new Vector3(this.transform.localEulerAngles.x, this.transform.localEulerAngles.y, newEggAngle);
		startX = eggSpawnTrans.position.x;
		endX = eggSpawnTrans.position.x + xMoveDist;
		startY = eggSpawnTrans.position.y;
		endY = eggSpawnTrans.position.y + yMoveDist;
	}

	void EggMotion() {
		if (lerpValue < 1) {
			lerpValue += Time.deltaTime / eggFlyDuration;
			newX = Mathf.LerpUnclamped(startX, endX, xAnimCurve.Evaluate(lerpValue)) * xMult;
			newY = Mathf.LerpUnclamped(startY, endY, yAnimCurve.Evaluate(lerpValue)) * yMult;

			this.transform.position = new Vector3(newX, newY, this.transform.position.z);
		}
		else {
			spawn = false;
			lerpValue = 0f; // Not needed
		}

	}
}
