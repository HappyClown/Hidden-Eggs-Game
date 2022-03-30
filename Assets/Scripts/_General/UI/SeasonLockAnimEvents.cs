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

	// void Update () {
	// 	if (scaleDownReqSparks) {
	// 		// Only works like this if the scale goes from 1 to 0. If not, additional calculations need to be made. 
	// 		newReqSparkScale -= Time.deltaTime / reqSparkDownDuration;
	// 		Vector3 newScaleVec = new Vector3(newReqSparkScale, newReqSparkScale, newReqSparkScale);
	// 		oneReqSparkTrans.localScale = newScaleVec;
	// 		multiReqSparkTrans.localScale = newScaleVec;
	// 		if (newReqSparkScale <= 0f) {
	// 			scaleDownReqSparks = false;
	// 		}
	// 	}
	// }
	IEnumerator ScaleDownSparks() {
		while (newReqSparkScale > 0f) {
			newReqSparkScale -= Time.deltaTime / reqSparkDownDuration;
			Vector3 newScaleVec = new Vector3(newReqSparkScale, newReqSparkScale, newReqSparkScale);
			oneReqSparkTrans.localScale = newScaleVec;
			multiReqSparkTrans.localScale = newScaleVec;
			yield return null;
		}
	}
	
	void PlayFlashFX() {
		flashFX.gameObject.SetActive(true);
		flashFX.Play();
	}

	void PlaySparksFX() {
		sparksFX.gameObject.SetActive(true);
		sparksFX.Play();
	}

	void PlaySparkleDustFX() {
		sparkleDustFX.gameObject.SetActive(true);
		sparkleDustFX.Play();
	}

	void StopSparkleDustFX() {
		sparkleDustFX.Stop();
	}

	void PlayShaftsFX() {
		shaftsFX.gameObject.SetActive(true);
		shaftsFX.Play();
	}

	void ScaleDownReqSparks() {
		//scaleDownReqSparks = true;
		StartCoroutine(ScaleDownSparks());
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
