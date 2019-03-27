using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryEggMotions : MonoBehaviour {
	[Header("Out of Bag")]
	public Transform eggSpawnTrans;
	public GameObject bewilderedTime;
	public float eggFlyDuration;
	public AnimationCurve yAnimCurve;
	public float yMoveDist, minYMove, maxYMove;
	public AnimationCurve xAnimCurve;
	public float xMoveDist;

	public float minScale, maxScale;
	public float minEggAngle, maxEggAngle;

	private bool spawnInBag;
	private float lerpValue, startY, endY, startX, endX, newX, newY, newEggAngle;
	[Header("Falling")]
	public AnimationCurve toMidAnimCurve;
	public AnimationCurve toBotAnimCurve;
	public float fallDuration, fallEggScale;
	public List<Transform> fallEggSpawnTrans;
	private bool fallFromTop, fallDown;
	[Header("Hover")]
	public AnimationCurve hoverAnimCurve;
	public float hoverDuration, hoverUpHeight;
	private bool hover;
	private Vector3 iniHoverPos;
	public int hoverAmnt;
	private int hoverAmntNum;
	[Header ("Rotate")]
	public float minRotDur;
	public float maxRotDur;
	private float fullRotDuration;
	private bool rotate;

	void Update () {
		if (spawnInBag) {
			OutOfBag();
		}
		if (rotate) {
			Rotate();
		}
		if (fallFromTop) {
			FallFromTop();
		}
		if (hover) {
			Hover();
		}
		if (fallDown) {
			FallDown();
		}
	}

	// For the "TimeConfused" storyboard.
	public void SpawnEggInBag() {
		spawnInBag = true;
		if (bewilderedTime.transform.eulerAngles.y > 90 && bewilderedTime.transform.eulerAngles.y < 270/*  || bewilderedTime.transform.eulerAngles.y < -90 && bewilderedTime.transform.eulerAngles.y >- 180 */) {
			xMoveDist = Mathf.Abs(xMoveDist) * -1;
		}
		else {
			xMoveDist = Mathf.Abs(xMoveDist);
		}
		newEggAngle = Random.Range(minEggAngle, maxEggAngle);
		yMoveDist = Random.Range(minYMove, maxYMove);
		this.transform.localEulerAngles = new Vector3(this.transform.localEulerAngles.x, this.transform.localEulerAngles.y, newEggAngle);
		startX = eggSpawnTrans.position.x;
		endX = eggSpawnTrans.position.x + xMoveDist;
		startY = eggSpawnTrans.position.y;
		endY = eggSpawnTrans.position.y + yMoveDist;
	}

	void OutOfBag() {
		if (lerpValue < 1) {
			lerpValue += Time.deltaTime / eggFlyDuration;
			newX = Mathf.LerpUnclamped(startX, endX, xAnimCurve.Evaluate(lerpValue));
			newY = Mathf.LerpUnclamped(startY, endY, yAnimCurve.Evaluate(lerpValue));

			this.transform.position = new Vector3(newX, newY, this.transform.position.z);
		}
		else {
			spawnInBag = false;
			lerpValue = 0f;
		}
	}
	// For the "EggsFalling" storyboard.
	public void SpawnEggsAtTop(Vector3 startPos, Vector3 endPos) {
		this.transform.localScale = new Vector3(fallEggScale, fallEggScale, fallEggScale);
		this.transform.position = startPos;
		fallFromTop = true;
		startY = this.transform.position.y;
		endY = endPos.y;
		rotate = true;
		fullRotDuration = Random.Range(minRotDur, maxRotDur);
		int newRot = Random.Range(0, 360);
		this.transform.localEulerAngles = new Vector3(this.transform.localEulerAngles.x, this.transform.localEulerAngles.y, newRot);
	}

	void Rotate() {
		this.transform.RotateAround(this.transform.position, Vector3.back, 360 * (Time.deltaTime / fullRotDuration));
	}

	void FallFromTop() {
		if (lerpValue < 1) {
			lerpValue += Time.deltaTime / fallDuration;
			newY = Mathf.Lerp(startY, endY, toMidAnimCurve.Evaluate(lerpValue));
			this.transform.position = new Vector3(this.transform.position.x, newY, this.transform.position.z);
		}
		else {
			fallFromTop = false;
			lerpValue = 0f;
			hover = true;
			iniHoverPos = this.transform.position;
		}
	}

	void Hover() {
		if (lerpValue < 1) {
			lerpValue += Time.deltaTime / hoverDuration;
			newY = hoverAnimCurve.Evaluate(lerpValue) * hoverUpHeight;
			this.transform.position = new Vector3(this.transform.position.x, iniHoverPos.y + newY, this.transform.position.z);
			if (lerpValue > 0.5f && hoverAmntNum >= hoverAmnt) { // The egg starts going down from its top hover position. Wether this happens at 0.5 of the lerp or otherwise depends on the AnimationCurves peak.
				hover = false;
				fallDown = true;
				lerpValue = 0f;
				hoverAmntNum = 0;
				startY = this.transform.position.y;
				endY = startY - 20f; // Height of the screen to make sur all the eggs go off screen.
			}
		}
		else {
			lerpValue = 0f;
			hoverAmntNum++;
		}
	}

	void FallDown() {
		if (lerpValue < 1) {
			lerpValue += Time.deltaTime / fallDuration;
			newY = Mathf.Lerp(startY, endY, toBotAnimCurve.Evaluate(lerpValue));
			this.transform.position = new Vector3(this.transform.position.x, newY, this.transform.position.z);
		}
		else {
			fallDown = false;
			lerpValue = 0f;
		}
	}
}
