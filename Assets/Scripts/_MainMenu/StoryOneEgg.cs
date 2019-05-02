using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryOneEgg : MonoBehaviour {
	[Header ("Rotate")]
	public bool rotate;
	public float halfRotDur;
	[Header ("Fly To Middle")]
	public float flyDur;
	public AnimationCurve flyAnimCurveX, flyAnimCurveY;
	public Transform flyEndTrans;
	private bool flyOutOfTime;
	private float lerpValue, iniX, iniY, maxX, maxY, newX, newY;
	public Vector3 endScale;
	private Vector3 startScale;
	[Header ("General")]
	public GameObject theOneEgg;
	[Header ("References")]
	public StoryTimeMotions storyTimeMoScript;
	
	void Update () {
		if (rotate) {
			Rotate();
		}
		if (flyOutOfTime) {
			FlyOutOfTime();
		}
	}

	void Rotate() {
		theOneEgg.transform.RotateAround(theOneEgg.transform.position, Vector3.up, 180 * (Time.deltaTime / halfRotDur));
	}

	void FlyOutOfTime() {
		lerpValue += Time.deltaTime / flyDur;
		newX = Mathf.Lerp(iniX, maxX, flyAnimCurveX.Evaluate(lerpValue));
		newY = Mathf.Lerp(iniY, maxY, flyAnimCurveY.Evaluate(lerpValue));
		theOneEgg.transform.position = new Vector3(newX, newY, theOneEgg.transform.position.z);
		theOneEgg.transform.localScale = Vector3.Lerp(startScale, endScale, flyAnimCurveY.Evaluate(lerpValue));
		if (lerpValue >= 1f) {
			lerpValue = 0f;
			flyOutOfTime = false;
		}
	}

	public void SetupFlyOutOfTime() {
		theOneEgg.SetActive(true);
		theOneEgg.transform.eulerAngles = Vector3.zero;
		iniX = storyTimeMoScript.currentTime.transform.position.x;
		iniY = storyTimeMoScript.currentTime.transform.position.y;
		maxX = flyEndTrans.position.x;
		maxY = flyEndTrans.position.y;
		startScale = theOneEgg.transform.localScale;
		flyOutOfTime = true;
	}
}
