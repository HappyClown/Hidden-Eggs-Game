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
	public AnimationCurve fallingAnimCurve;
	public float fallDuration, fallEggScale, fallYAmnt;
	public List<Transform> fallEggSpawnTrans;
	private bool spawnFromTop;
	[Header("Hover")]
	public AnimationCurve hoverAnimCurve;
	public float hoverDuration, hoverUpHeight;
	private bool hover;

	void Update () {
		if (spawnInBag) {
			OutOfBag();
		}
		if (spawnFromTop) {
			FallFromTop();
		}
		if (hover) {
			Hover();
		}
	}

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

	public void SpawnEggsAtTop(Vector3 spawnPos) {
		this.transform.localScale = new Vector3(fallEggScale, fallEggScale, fallEggScale);
		this.transform.position = spawnPos;
		spawnFromTop = true;
		startY = this.transform.position.y;
		endY = this.transform.position.y - fallYAmnt;
	}

	void FallFromTop() {
		if (lerpValue < 1) {
			lerpValue += Time.deltaTime / fallDuration;
			newY = Mathf.Lerp(startY, endY, fallingAnimCurve.Evaluate(lerpValue));
			this.transform.position = new Vector3(this.transform.position.x, newY, this.transform.position.z);
		}
		else {
			spawnFromTop = false;
			lerpValue = 0f;
		}
	}
	void Hover() {

	}
}
