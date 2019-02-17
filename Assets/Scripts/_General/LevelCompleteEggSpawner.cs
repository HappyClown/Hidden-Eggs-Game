using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleteEggSpawner : MonoBehaviour {
	public List<LevelCompleteEggMove> regEggs, silEggs, golEggs;
	public List<float> regEggDelay, silEggDelay, golEggDelay;
	private bool startEggSpawning, startedRegEggs, startedSilEggs, startedGolEggs;
	//public float delay;
	public float regEggDuration, silEggDuration, golEggDuration;
	public float spawnRegEggs, spawnSilEggs, spawnGolEggs;
	public AnimationCurve animCurve;
	private float eggTypeTimer, regSpawnInterval, silSpawnInterval, golSpawnInterval;
	// private float eggDelay;

	void Start () {
		//eggDelay = delay;
	}

	void Update () {
		if (startEggSpawning) {
			eggTypeTimer += Time.deltaTime;
			if (eggTypeTimer > spawnRegEggs && !startedRegEggs) {
				startedRegEggs = true;
				StartRegEggSpawn();
			}
			if (eggTypeTimer > spawnSilEggs && !startedSilEggs) {
				startedSilEggs = true;
				StartSilEggSpawn();
			}
			if (eggTypeTimer > spawnGolEggs && !startedGolEggs) {
				startedGolEggs = true;
				StartGolEggSpawn();
				startEggSpawning = false;
			}
		}
	}

	public void StartEggSpawning() {
		startEggSpawning = true;
	}

	public void StartRegEggSpawn() {
		for(int i = 0; i < regEggs.Count; i++)
		{
			regEggs[i].StartEggMovement(regEggDelay[i] * regEggDuration);
			//eggDelay += eggDelay;
		}
	}
	public void StartSilEggSpawn() {
		for(int i = 0; i < silEggs.Count; i++)
		{
			silEggs[i].StartEggMovement(silEggDelay[i] * silEggDuration);
			//eggDelay += eggDelay;
		}
	}
	public void StartGolEggSpawn() {
		for(int i = 0; i < golEggs.Count; i++)
		{
			golEggs[i].StartEggMovement(golEggDelay[i] * golEggDuration);
			//eggDelay += eggDelay;
		}
	}

	public void CalculateIntervals() {
		regEggDelay.Clear();
		regSpawnInterval = 1f / regEggs.Count;
		float spawnTime = regSpawnInterval;
		for(int i = 0; i < regEggs.Count; i++) 
		{
			regEggDelay.Add(animCurve.Evaluate(spawnTime));
			spawnTime += regSpawnInterval;
		}
		silEggDelay.Clear();
		silSpawnInterval = 1f / silEggs.Count;
		spawnTime = silSpawnInterval;
		for(int i = 0; i < silEggs.Count; i++) 
		{
			silEggDelay.Add(animCurve.Evaluate(spawnTime));
			spawnTime += silSpawnInterval;
		}
		golEggDelay.Clear();
		golSpawnInterval = 1f / golEggs.Count;
		spawnTime = golSpawnInterval;
		for(int i = 0; i < golEggs.Count; i++) 
		{
			golEggDelay.Add(animCurve.Evaluate(spawnTime));
			spawnTime += golSpawnInterval;
		}
	}
}
