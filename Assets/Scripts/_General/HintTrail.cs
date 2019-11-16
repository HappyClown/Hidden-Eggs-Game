using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintTrail : MonoBehaviour {
	public GameObject hintBall;
	public GameObject hintGroupParent;
	public TrailRenderer trailRend;
	public float trailFadeTime;
	private bool clearNextUpdate;

	void Update() {
		if (Input.GetKeyDown("space")) {
			ClearTrail();
		}
		if (clearNextUpdate) {
			trailRend.enabled = true;
			trailRend.Clear();
			clearNextUpdate = false;
		}
	}
	
	public void UnparentFromBall() {
		// trailRend.time = 0f;
		// trailRend.Clear();
		this.transform.parent = hintGroupParent.transform;
	}

	public void ParentTHintBall() {
		// trailRend.time = trailFadeTime;
		this.transform.position = hintBall.transform.position;
		this.transform.parent = hintBall.transform;
		trailRend.enabled = false;
		//trailRend.Clear();
	}

	public void ClearTrail() {
		//trailRend.Clear();
		clearNextUpdate = true;
	}
}