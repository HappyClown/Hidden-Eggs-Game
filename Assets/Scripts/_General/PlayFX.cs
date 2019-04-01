using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFX : MonoBehaviour {
	public ParticleSystem partFX;

	void PlayParticleFX () {
		partFX.Play();
	}
}
