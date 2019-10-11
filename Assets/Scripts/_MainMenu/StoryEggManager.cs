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
	private bool spawnFallingEggsRandom, hoveringEggsFall;
	public bool randomFallingEggs;
	//private List<int> intsForRandom;
	private List<int> eggFallingOrder = new List<int>();

	void Start () {
		// The first egg spawns immediately.
		timeBetweenEggs = 0f;
	}
	
	void Update () {
		// Eggs pop out of Time's bag.
		if (spawnBagEggs) {
			eggSpawnTimer += Time.deltaTime;
			if (eggSpawnTimer > timeBetweenEggs) {
				eggSpawnTimer = 0f;
				storyEggScripts[currentEggNum].SpawnEggInBag();
				// AUDIO - EGG COMES OUT OF BAG!
				currentEggNum++;
				timeBetweenEggs = Random.Range(minTimeBetweenEggs, maxTimeBetweenEggs);
				if (currentEggNum > storyEggScripts.Count - 1) {
					//spawnEggs = false;
					currentEggNum = 0;
				}
			}
		}
		// Eggs fall from the top of the screen. For the EggsFalling story board(#008).
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
		if (spawnFallingEggsRandom) {
			eggSpawnTimer += Time.deltaTime;
			if (eggSpawnTimer > timeBetweenFallEggs) {
				eggSpawnTimer = 0f;
				//Debug.Log(currentEggNum + " Left should be i norder, right the random order, here we go! " + eggFallingOrder[currentEggNum]);
				int eggNum = eggFallingOrder[currentEggNum];
				storyEggScripts[eggNum].SpawnEggsAtTop(fallingEggStartTrans[eggNum].position, fallingEggEndTrans[eggNum].position);
				currentEggNum++;
			}
			if (currentEggNum > fallingEggStartTrans.Count - 1) {
				currentEggNum = 0;
				spawnFallingEggsRandom = false;
			}
		}
		// Make the eggs fall in order after hovering.
		if (hoveringEggsFall) {
			eggSpawnTimer += Time.deltaTime;
			if (eggSpawnTimer >= timeBetweenFallEggs) {
				eggSpawnTimer = 0f;
				storyEggScripts[currentEggNum].fadeToSceneEgg = true;
				currentEggNum++;
				if (currentEggNum > fallingEggStartTrans.Count - 1) {
					currentEggNum = 0;
					hoveringEggsFall = false;
				}
			}
		}
	}

	public void SpawnFallingEggs() {
		currentEggNum = 0;
		// To have the eggs fall into random positions.
		if (randomFallingEggs) {
			spawnFallingEggsRandom = true;
			//Debug.Log(Time.time);
			//Fill int list in order 0 -> storyEggScripts.Count.
			List<int> intsForRandom = new List<int>();
			for (int i = 0; i < storyEggScripts.Count; i++)
			{
				intsForRandom.Add(i);
			}
			// Randomly assign ints to a new list once.
			for (int i = 0; i < storyEggScripts.Count; i++)
			{
				currentEggNum = Random.Range(0, intsForRandom.Count);
				while (eggFallingOrder.Contains(currentEggNum)) 
				{
					if (currentEggNum >= intsForRandom.Count - 1) {
						currentEggNum = 0;
					}
					else {
						currentEggNum++;
					}
				}
				eggFallingOrder.Add(currentEggNum);
			}
			//Debug.Log(Time.time);
			currentEggNum = 0;
		}
		else {
			spawnFallingEggs = true;
		}
		eggSpawnTimer = 0f;
	}

	public void EggsFallOffScreen() {
		currentEggNum = 0;
		eggSpawnTimer = 0f;
		hoveringEggsFall = true;
		//fadeToSceneEgg = true;
	}

	public void ResetEggs() {
		spawnBagEggs = spawnFallingEggs = spawnFallingEggsRandom = false;
		currentEggNum = 0;
		eggSpawnTimer = 0f;
		foreach (StoryEggMotions storyEgg in storyEggScripts)
		{
			storyEgg.Reset();
		}
	}
}
