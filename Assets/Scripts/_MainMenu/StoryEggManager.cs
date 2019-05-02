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
	public bool spawnFallingEggs;
	public float timeBetweenFallEggs;
	public List<Transform> fallingEggStartTrans;
	public List<Transform> fallingEggEndTrans;

	void Start () {
		// The first egg spawns immediately.
		timeBetweenEggs = 0f;
	}
	
	void Update () {
		if (spawnBagEggs) {
			eggSpawnTimer += Time.deltaTime;
			if (eggSpawnTimer > timeBetweenEggs) {
				eggSpawnTimer = 0f;
				storyEggScripts[currentEggNum].SpawnEggInBag();
				currentEggNum++;
				timeBetweenEggs = Random.Range(minTimeBetweenEggs, maxTimeBetweenEggs);
				if (currentEggNum > storyEggScripts.Count - 1) {
					//spawnEggs = false;
					currentEggNum = 0;
				}
			}
		}

		if (spawnFallingEggs) {
			eggSpawnTimer += Time.deltaTime;
			if (eggSpawnTimer > timeBetweenFallEggs) {
				eggSpawnTimer = 0f;
				storyEggScripts[currentEggNum].SpawnEggsAtTop(fallingEggStartTrans[currentEggNum].position, fallingEggEndTrans[currentEggNum].position);
				currentEggNum++;
				if (currentEggNum > fallingEggStartTrans.Count - 1) {
					currentEggNum = 0;
					spawnFallingEggs = false;
				}
			}
		}
	}

	public void SpawnFallingEggs() {
		currentEggNum = 0;
		spawnFallingEggs = true;
		eggSpawnTimer = 0f;
	}

	public void ResetEggs() {
		foreach (StoryEggMotions storyEgg in storyEggScripts)
		{
			storyEgg.Reset();
		}
	}
}
