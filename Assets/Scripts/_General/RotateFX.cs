using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateFX : MonoBehaviour {
	public float rotDur;
	public AnimationCurve rotCurve;
	public ParticleSystem rotFX;
	private bool rotating;
	private float timer;
	private float curRotDur;
	
	void Update () {
		if (rotating) {
			this.transform.RotateAround(this.transform.position, Vector3.forward, 360 * Time.deltaTime / curRotDur);
			timer += Time.deltaTime;
			if (timer > curRotDur) {
				rotating = false;
				rotFX.Stop();
				timer = 0f;
			}
		}
	}

	public void RotatePlayFX(float rotationDuration = 0f) {
		rotating = true;
		timer = 0f;
		// Debug.Log(rotationDuration);
		if (rotationDuration == 0) {
			curRotDur = rotDur;
		}
		else {
			curRotDur = rotationDuration;
		}
		this.transform.eulerAngles = Vector3.zero;
		// AUDIO - ROTATION STARTS!
		rotFX.Play();
	}
}
