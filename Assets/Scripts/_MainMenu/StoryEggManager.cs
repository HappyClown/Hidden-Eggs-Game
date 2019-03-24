using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryEggManager : MonoBehaviour {
	public bool spawnEggs;
	public List<StoryEggMotions> storyEggScripts;
	private int currentEggNum;
	//public int maxAmountEggs;
	//public Transform eggSpawnTrans;
	public float minTimeBetweenEggs, maxTimeBetweenEggs;
	private float eggSpawnTimer, timeBetweenEggs;

	void Start () {
		// The first egg spawns immediately.
		timeBetweenEggs = 0f;
	}
	
	void Update () {
		if (spawnEggs) {
			eggSpawnTimer += Time.deltaTime;
			if (eggSpawnTimer > timeBetweenEggs) {
				eggSpawnTimer = 0f;
				storyEggScripts[currentEggNum].SpawnEgg();
				currentEggNum++;
				timeBetweenEggs = Random.Range(minTimeBetweenEggs, maxTimeBetweenEggs);
				if (currentEggNum > storyEggScripts.Count - 1) {
					//spawnEggs = false;
					currentEggNum = 0;
				}
			}
		}
	}
}
