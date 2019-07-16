using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonLockAnimEvents : MonoBehaviour {
	public ParticleSystem flashFX;
	public ParticleSystem sparksFX;
	public ParticleSystem sparkleDustFX;
	public ParticleSystem shaftsFX;
	
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

	void ShowStoreButton() {
		
	}
}
