using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleteEggSpawner : MonoBehaviour {
	[Header ("Settings")]
	// public float regEggDuration;
	public float  /* silEggDuration, golEggDuration, */ allEggSpawnDuration;
	public float allEggToBagDuration;
	// public float spawnRegEggs, spawnSilEggs, spawnGolEggs, spawnAllEggs;
	public AnimationCurve spawnCurve, toBagCurve;
	[Header ("References")]
	//public List<LevelCompleteEggMoveSpin> regEggs;
	public List<LevelCompleteEggMoveSpin> /* silEggs, golEggs, */ allEggs;


	[Header ("Info")]
	// public List<float> regEggDelay;
	public List<float> /* silEggDelay, golEggDelay, */ allEggSpawnDelay;
	public List<float> allEggToBagDelay;
	// private bool startEggSpawning, startedRegEggs, startedSilEggs, startedGolEggs;
	private float /* eggTypeTimer, regSpawnInterval, silSpawnInterval, golSpawnInterval, */ allSpawnInterval;

	void Update () {
		// if (startEggSpawning) {
		// 	eggTypeTimer += Time.deltaTime;
		// 	if (eggTypeTimer > spawnRegEggs && !startedRegEggs) {
		// 		startedRegEggs = true;
		// 		StartRegEggSpawn();
		// 	}
		// 	if (eggTypeTimer > spawnSilEggs && !startedSilEggs) {
		// 		startedSilEggs = true;
		// 		StartSilEggSpawn();
		// 	}
		// 	if (eggTypeTimer > spawnGolEggs && !startedGolEggs) {
		// 		startedGolEggs = true;
		// 		StartGolEggSpawn();
		// 		startEggSpawning = false;
		// 	}
		// }
	}

	// public void StartEggSpawning() {
	// 	startEggSpawning = true;
	// }

	public void StartAllEggSpawn() {
		for(int i = 0; i < allEggs.Count; i++)
		{
			allEggs[i].StartEggMovement(allEggSpawnDelay[i] * allEggSpawnDuration, allEggToBagDelay[i] * allEggToBagDuration);
		}
	}
	// public void StartRegEggSpawn() {
	// 	for(int i = 0; i < regEggs.Count; i++)
	// 	{
	// 		regEggs[i].StartEggMovement(regEggDelay[i] * regEggDuration);
	// 		//eggDelay += eggDelay;
	// 	}
	// }
	// public void StartSilEggSpawn() {
	// 	for(int i = 0; i < silEggs.Count; i++)
	// 	{
	// 		silEggs[i].StartEggMovement(silEggDelay[i] * silEggDuration);
	// 		//eggDelay += eggDelay;
	// 	}
	// }
	// public void StartGolEggSpawn() {
	// 	for(int i = 0; i < golEggs.Count; i++)
	// 	{
	// 		golEggs[i].StartEggMovement(golEggDelay[i] * golEggDuration);
	// 		//eggDelay += eggDelay;
	// 	}
	// }

	public void CalculateIntervals() {
		allSpawnInterval = 1f / allEggs.Count;
		float spawnTime = allSpawnInterval;

		allEggSpawnDelay.Clear();
		for(int i = 0; i < allEggs.Count; i++)
		{
			allEggSpawnDelay.Add(spawnCurve.Evaluate(spawnTime));
			spawnTime += allSpawnInterval;
		}

		spawnTime = allSpawnInterval;

		allEggToBagDelay.Clear();
		for(int i = 0; i < allEggs.Count; i++)
		{
			allEggToBagDelay.Add(toBagCurve.Evaluate(spawnTime));
			spawnTime += allSpawnInterval;
		}
		// regEggDelay.Clear();
		// regSpawnInterval = 1f / regEggs.Count;
		// spawnTime = regSpawnInterval;
		// for(int i = 0; i < regEggs.Count; i++) 
		// {
		// 	regEggDelay.Add(spawnCurve.Evaluate(spawnTime));
		// 	spawnTime += regSpawnInterval;
		// }
		// silEggDelay.Clear();
		// silSpawnInterval = 1f / silEggs.Count;
		// spawnTime = silSpawnInterval;
		// for(int i = 0; i < silEggs.Count; i++) 
		// {
		// 	silEggDelay.Add(spawnCurve.Evaluate(spawnTime));
		// 	spawnTime += silSpawnInterval;
		// }
		// golEggDelay.Clear();
		// golSpawnInterval = 1f / golEggs.Count;
		// spawnTime = golSpawnInterval;
		// for(int i = 0; i < golEggs.Count; i++) 
		// {
		// 	golEggDelay.Add(spawnCurve.Evaluate(spawnTime));
		// 	spawnTime += golSpawnInterval;
		// }
	}
}
