using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorySingleCloudManager : MonoBehaviour {
	[Header ("ParticleSystems")]
	public ParticleSystem xPartSys;
	public ParticleSystem yPartSys;
	private ParticleSystem activePartSys;
	//public float ogMinSpeed, ogMaxSpeed, gustMinSpeed, gustMaxSpeed;
	[Tooltip("The speed multiplier references the particle's maximum startspeed. The minimum speed is determined by the min curve value (out of 1) in the inspector.")]
	public float ogSpeedMult, gustSpeedMult, vertSpeedMult;
	private float ogEmRate;
	public bool slowDownClouds;
	[Header ("Scripts")]
	public StoryScrollingBG storyScrollBGScript;

	void Update () {
		if (slowDownClouds) {
			SlowDownClouds();
		}
	}
	
	public void PlayClouds(ParticleSystem partSys, float speedMult = 0, bool prewarmed = false) {
		activePartSys = partSys;
		// Store the Particle System's components to allow modifications.
		var ps = partSys.main;
		var em = partSys.emission;
		var vol = partSys.velocityOverLifetime;
		var col = partSys.colorOverLifetime;
		// Stop all other particle systems.
		if (partSys != xPartSys) {
			xPartSys.Clear();
			xPartSys.Stop();
		}
		if (partSys != yPartSys) {
			yPartSys.Clear();
			yPartSys.Stop();
		}
		// Prewarm the particles to the desired temperature.
		ps.prewarm = prewarmed;
		// Clear the old particles. (This could be rendered obsolete by using the vol's SpeedModifier, but I just found 
		// out about it and I dont want to rewrite everything that uses the mainModule's startSpeedMultiplier. ^^)
		vol.speedModifier = 1f;
		activePartSys.Clear();
		activePartSys.Stop();
		//Adjust the particle min and max speed.
		if (speedMult == 0) {
			speedMult = ogSpeedMult;
		}
		ps.startSpeedMultiplier = speedMult;
		em.rateOverTimeMultiplier = speedMult / ogSpeedMult;
		activePartSys.Play();
		//Play the appropriate particle system.
		if (!partSys.isPlaying) {
			vol.speedModifier = 1f;
			vol.enabled = false;
			col.enabled = false;
			em.enabled = true;
			partSys.Play(true);
		}
	}
	public void SlowDownCloudsSetup(ParticleSystem partSys) {
		activePartSys = partSys;
		slowDownClouds = true;
		// Enable Velocity Over Lifetime to slow down active particles with the SpeedModifier attribute.
		// Enable Color Over Lifetime to have the clouds on screen fade out at the end of their life. // Consider extending their life?
		var vol = partSys.velocityOverLifetime;
		var col = partSys.colorOverLifetime;
		vol.enabled = true;
		col.enabled = true;
		//speed modifier lerp down from 1 - 0 (based on clouds slowdown speed?)
	}
	void SlowDownClouds() {
		var vol = activePartSys.velocityOverLifetime;
		float speedModValue = storyScrollBGScript.ScrollValue / storyScrollBGScript.ScrollSpeed;
		vol.speedModifier = speedModValue;
		if (speedModValue <= 0f) {
			slowDownClouds = false;
		}
	}
	public void StopActivePartSys() {
		// Stop the active particle systems.
		activePartSys.Clear();
		activePartSys.Stop();
	}
	#region Non particle system single clouds.
	// public List<StorySingleCloud> singleClouds;
	// public float minSpawnTime, maxSpawnTime;
	// private float timer, spawnTime;
	// public BoxCollider2D xSpawnArea, ySpawnArea;
	// public bool rightLeft, downUp;
	// public float xResetLimit, yResetLimit;
	// public float minScale, maxScale, minAlpha, MaxAlpha, minSpeed, maxSpeed;

	// private int cloudToMove;

	// void Start () {
		
	// }

	// void Update () {
		
	// }

	// void SpawnTimer() {
	// 	timer += Time.deltaTime;
	// 	if (timer > spawnTime) {
	// 		// Spawn
	// 		timer = 0f;
	// 		spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
	// 	}
	// }

	// void ChooseCloud() {
	// 	// Pick which cloud will start moving.
	// 	cloudToMove = Random.Range(0, singleClouds.Count);
	// 	if (!singleClouds[cloudToMove].moving) {
	// 		singleClouds[cloudToMove].moving = true;
	// 	}
	// 	else {
	// 		int whileEscape = 0;
	// 		while(!singleClouds[cloudToMove].moving) {
	// 			cloudToMove++;
	// 			if (cloudToMove >= singleClouds.Count) {
	// 				cloudToMove = 0;
	// 			}
	// 			// In case all the clouds are moving (if their speed is very low for example), break the while loop.
	// 			whileEscape ++;
	// 			if (whileEscape >= singleClouds.Count) {
	// 				break;
	// 			}
	// 		}
	// 		//singleClouds[cloudToMove].moving = true;
	// 		SpawnCloud();
	// 	}
	// }

	// void SpawnCloud() {
		
	// }
	#endregion
}