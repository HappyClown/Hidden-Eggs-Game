using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryEggManager : MonoBehaviour {
	[Header ("Eggs out of Bag")]
	public bool spawnBagEggs;
	public List<StoryEggMotions> storyEggScripts;
	private int currentEggNum;
	public float minTimeBetweenEggs, maxTimeBetweenEggs;
	private float eggSpawnTimer, timeBetweenEggs;
	//public int maxAmountEggs;
	//public Transform eggSpawnTrans;
	[Header ("Eggs from Sky")]
	public bool spawnSkyEggs;

	void Start () {
		// The first egg spawns immediately.
		timeBetweenEggs = 0f;
	}
	
	void Update () {
		if (spawnBagEggs) {
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

		if (spawnSkyEggs) {

		}
	}
}
