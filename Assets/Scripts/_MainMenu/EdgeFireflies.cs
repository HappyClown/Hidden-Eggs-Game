using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeFireflies : MonoBehaviour {
	public List<ParticleSystem> partSystems;
	public float amountByRadius;
	
	public void SetEmissionByRadius () {
		foreach(ParticleSystem partSys in partSystems)
		{
			//var main = partSys.main;
			var em = partSys.emission;

			em.rateOverTime = partSys.shape.radius * amountByRadius;
		}
	}

	public void StartFireflyFX () {
		foreach(ParticleSystem partSystem in partSystems)
		{
			partSystem.Play(true);
		}
	}

	public void StopFireflyFX () {
		foreach(ParticleSystem partSystem in partSystems)
		{
			partSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
		}
	}
}
