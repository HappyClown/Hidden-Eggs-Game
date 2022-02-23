using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSilverEggSpawner : MonoBehaviour {
	[Header("Settings")]
	public float silverEggSpawnDelay;
	[Header ("References")]
	public List<GameObject> silEggs;
	public ClickOnEggs clickOnEggs;
	public SceneEggMovement sceneEggMovement;
	[Header ("Info")]
	public bool spawnSilverEggs;
	public int puzzSilEggCount;
	public int sceneSilEggCount;
	public List<int> puzzSilEggCountList;
	public List<int> sceneSilEggCountList;

	// If new Silver Eggs have been collected in the puzzle, send them to the Egg Panel.
	public void NewSilverEggsCheck() {
		SetCorrectLevelLists();
		SetListCounts();
		// Check if there are new silver eggs to send to the panel.
		if (puzzSilEggCount > sceneSilEggCount) { 
			QueueSequenceManager.AddSequenceToQueue(StartSilverEggSpawnSequence);
		}
	}
	// Start the coroutine sequence to spawn the silver eggs.
	public void StartSilverEggSpawnSequence() {
		StartCoroutine(SpawnSilverEggs());
	}

	IEnumerator SpawnSilverEggs() {
		float timer = 0f;
		int silEggsToSpawn = puzzSilEggCount - sceneSilEggCount;
		int silEggSpawned = 0;
		while (silEggSpawned < silEggsToSpawn) {
			// Waiting time before spawning each silver egg.
			while (timer < silverEggSpawnDelay) {
				timer += Time.deltaTime;
				yield return null;
			}
			timer = 0f;
			// Spawn the next silver egg.
			silEggs[puzzSilEggCountList[sceneSilEggCount]].SetActive(true);
			sceneEggMovement.StartCoroutine(sceneEggMovement.MoveSceneEggToCorner(silEggs[puzzSilEggCountList[sceneSilEggCount]], clickOnEggs.silverEggSpots[sceneSilEggCount], 0+sceneSilEggCount, null, false, true, false));
			GlobalVariables.globVarScript.sceneSilEggsCount.Add(sceneSilEggCount); 
			GlobalVariables.globVarScript.SaveEggState();
			sceneSilEggCount++;
			silEggSpawned++;
		}
		// Save silver eggs that were put in the panel.
		clickOnEggs.AddEggsFound();
		yield return null;
	}
	// Assign the Silver Egg lists to variables.
	public void SetCorrectLevelLists() {
		if (GlobalVariables.globVarScript.puzzSilEggsCount.Count > 0) { 
			puzzSilEggCountList = GlobalVariables.globVarScript.puzzSilEggsCount; 
		}
		if (GlobalVariables.globVarScript.sceneSilEggsCount.Count > 0) { 
			sceneSilEggCountList = GlobalVariables.globVarScript.sceneSilEggsCount; 
		}
	}
	void SetListCounts() {
		if (puzzSilEggCountList.Count > 0) { puzzSilEggCount = puzzSilEggCountList.Count; }
		if (sceneSilEggCountList.Count > 0) { sceneSilEggCount = sceneSilEggCountList.Count; }
	}
}
