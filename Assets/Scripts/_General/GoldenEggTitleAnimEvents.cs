using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenEggTitleAnimEvents : MonoBehaviour {
	public List<RotationBurst> rotBurstScripts;
	public List<FadeInOutSprite> starFadeScripts;

	void StartStartRotations() {
		foreach(RotationBurst rotBurst in rotBurstScripts)
		{
			rotBurst.StartRotation();
		}
	}

	void StartStarFades() {
		foreach(FadeInOutSprite starFadeScript in starFadeScripts)
		{
			starFadeScript.FadeIn();
		}
	}
}
