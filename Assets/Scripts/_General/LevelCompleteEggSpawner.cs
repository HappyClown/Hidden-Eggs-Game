using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleteEggSpawner : MonoBehaviour {

	[Header ("Settings")]
	public float allEggSpawnDuration;
	public float allEggToBagDuration;
	public AnimationCurve spawnCurve, toBagCurve;

	[Header ("References")]
	public List<LevelCompleteEggMoveSpin> allEggs;
	public LevelCompleteEggVariables[] allEggVariables;
	public LevelCompleteEggMovement lvlCompEggMovement;
	public GameObject eggSprites;
	
	[Header ("Info")]
	public List<float> allEggSpawnDelay;
	public List<float> allEggToBagDelay;
	private float allSpawnInterval;

	// public void StartAllEggSpawn() {
	// 	eggSprites.SetActive(true);
	// 	for(int i = 0; i < allEggs.Count; i++)
	// 	{
	// 		allEggs[i].StartEggMovement(allEggSpawnDelay[i] * allEggSpawnDuration, allEggToBagDelay[i] * allEggToBagDuration);
	// 	}
	// }

	public IEnumerator StartAllEggs() {
		eggSprites.SetActive(true);
		int eggNumber = 0;
		float timer = 0f;
		while (eggNumber < allEggVariables.Length) {
			timer += Time.deltaTime;
			if (timer >= allEggSpawnDelay[eggNumber]*allEggSpawnDuration) {
				allEggVariables[eggNumber].gameObject.SetActive(true);
				if (eggNumber == 0) {
					lvlCompEggMovement.StartCoroutine(lvlCompEggMovement.SpinMoveEggs(allEggVariables[eggNumber], true));
				}
				else if (eggNumber == allEggVariables.Length-1) {
					lvlCompEggMovement.StartCoroutine(lvlCompEggMovement.SpinMoveEggs(allEggVariables[eggNumber], false, true));
				}
				else {
					lvlCompEggMovement.StartCoroutine(lvlCompEggMovement.SpinMoveEggs(allEggVariables[eggNumber]));
				}
				eggNumber++;
				print ("EGGNUMBERSTARTEDDOK");
			}
			yield return null;
		}
	}

	// Used in editor to calculate when the eggs should spawn according to an animation curve.
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
	}
}
