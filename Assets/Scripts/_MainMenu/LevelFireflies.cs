using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFireflies : MonoBehaviour {

	public List<ParticleSystem> levelPartSys;
	//public List<ParticleSystem> allLevelPartSys;
	public EdgeFireflies edgeFirefliesScript;

	public void PlayLevelFireflies() {
		StopLevelFireflies();
		foreach(ParticleSystem partSys in levelPartSys)
		{
			// var emission = partSys.emission;
			// emission.enabled = true;
			partSys.Play();
		}
	}

	public void StopLevelFireflies() {
		foreach (ParticleSystem partSys in edgeFirefliesScript.partSystems)
		{
			// var emission = partSys.emission;
			// emission.enabled = false;
			partSys.Stop();
		}
	}
}
