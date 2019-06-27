using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSilverEggSpawner : MonoBehaviour {
	[Header("Settings")]
	public bool spawnSilverEggs;
	public float silverEggSpawnDelay;
	[Header ("References")]
	public List<GameObject> silEggs;
	public ClickOnEggs clickOnEggsScript;
	[Header ("Info")]
	public int puzzSilEggCount;
	public int sceneSilEggCount;
	public List<int> puzzSilEggCountList;
	public List<int> sceneSilEggCountList;
	private int silEggSpawned;

	void Awake () {
		SetCorrectLevelLists();
		SetListCounts();
	}
	void Update () {
		// Wait until no other sequences are playing to start the Silver Eggs sequence.
		if (spawnSilverEggs && !ClickOnEggs.inASequence) {
			// In a sequence.
			ClickOnEggs.inASequence = true;
			for(int i = sceneSilEggCount; i < puzzSilEggCount; i++) {
				silEggSpawned++;
				silEggs[puzzSilEggCountList[i]].SetActive(true);
				silEggs[puzzSilEggCountList[i]].GetComponent<SceneSilverEgg>().SendToPanel(puzzSilEggCountList[i], silverEggSpawnDelay * silEggSpawned);
				if (i == puzzSilEggCount - 1) {
					silEggs[puzzSilEggCountList[i]].GetComponent<SceneSilverEgg>().lastSpawned = true;
				}
			}
			clickOnEggsScript.checkLvlCompleteF = silverEggSpawnDelay * silEggSpawned;
			spawnSilverEggs = false;
		}
	}
	// If new Silver Eggs have been collected in the puzzle, send them to the Egg Panel.
	public void SpawnNewSilverEggs() {
		SetCorrectLevelLists();
		SetListCounts();
		if (puzzSilEggCount > sceneSilEggCount) { // Meaning I have new silver eggs to send to the panel.
			spawnSilverEggs = true;
		}
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
