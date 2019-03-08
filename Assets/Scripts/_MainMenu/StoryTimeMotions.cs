using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryTimeMotions : MonoBehaviour {
	public GameObject normalTime;
	[Header("")]
	public bool timeMovesIn;
	private float lerpValue;
	public Transform endTrans;
	private Vector3 startPos;
	public float endScale;
	private float newScale, startScale;
	public float moveInDuration;
	public AnimationCurve moveInAnimCurve;
	public AnimationCurve scaleInAnimCurve;
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
			normalTime.transform.position = Vector3.Lerp(startPos, endTrans.position, moveInAnimCurve.Evaluate(lerpValue));

			newScale = Mathf.Lerp(startScale, endScale, scaleInAnimCurve.Evaluate(lerpValue));
			normalTime.transform.localScale = new Vector3(newScale, newScale, newScale);
		}
	}
}
