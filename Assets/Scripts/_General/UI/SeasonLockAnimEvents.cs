using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonLockAnimEvents : MonoBehaviour {
	public ParticleSystem flashFX;
	public ParticleSystem sparksFX;
	public ParticleSystem sparkleDustFX;
	public ParticleSystem shaftsFX;
	
	public Transform oneReqSparkTrans;
	public Transform multiReqSparkTrans;
	public float reqSparkDownDuration;
	private float newReqSparkScale;
	private bool scaleDownReqSparks;

	public SeasonLock seasonLockScript;

	void Update () {
		if (scaleDownReqSparks) {
			// Only works like this if the scale goes from 1 to 0. If not, additional calculations need to be made. 
			newReqSparkScale -= Time.deltaTime / reqSparkDownDuration;
			Vector3 newScaleVec = new Vector3(newReqSparkScale, newReqSparkScale, newReqSparkScale);
			oneReqSparkTrans.localScale = newScaleVec;
			multiReqSparkTrans.localScale = newScaleVec;
			if (newReqSparkScale <= 0f) {
				scaleDownReqSparks = false;
			}
		}
	}
	
	void PlayFlashFX() {
		flashFX.Play();
	}

	void PlaySparksFX() {
		sparksFX.Play();
	}

	void PlaySparkleDustFX() {
		sparkleDustFX.Play();
	}

	void StopSparkleDustFX() {
		sparkleDustFX.Stop();
	}

	void PlayShaftsFX() {
		shaftsFX.Play();
	}

	void ScaleDownReqSparks() {
		scaleDownReqSparks = true;
		newReqSparkScale = 1f;
	}

	void ScaleDownLockGroup() {
		seasonLockScript.ScaleDownGroup();
	}

	void ShowCinematicButton() {
		seasonLockScript.CinematicButton();
	}

	void EnableLevelObjs() {
		seasonLockScript.enableSeasonObjsDelay = true;
	}
}
