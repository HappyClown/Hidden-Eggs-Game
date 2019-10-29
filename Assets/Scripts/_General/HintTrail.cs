using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintTrail : MonoBehaviour {
	public GameObject hintBall;
	public GameObject hintGroupParent;
	public TrailRenderer trailRend;
	public float trailFadeTime;
	
	public void UndoTrailParent() {
		// trailRend.time = 0f;
		this.transform.parent = hintGroupParent.transform;
	}

	public void SetTrailParent() {
		trailRend.time = trailFadeTime;
		this.transform.position = hintBall.transform.position;
		this.transform.parent = hintBall.transform;
	}
}