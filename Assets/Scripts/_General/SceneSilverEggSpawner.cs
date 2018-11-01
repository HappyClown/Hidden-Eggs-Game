using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSilverEggSpawner : MonoBehaviour 
{
	public float silverEggSpawnDelay;
	private int silEggSpawned;
	public int puzzSilEggCount;
	public int sceneSilEggCount;
	public ClickOnEggs clickOnEggsScript;
	public List<GameObject> silEggs;

	void Awake()
	{
		if (GlobalVariables.globVarScript.marketPuzzSilEggsCount.Count > 0) { puzzSilEggCount = GlobalVariables.globVarScript.marketPuzzSilEggsCount.Count; }
		if (GlobalVariables.globVarScript.marketSceneSilEggsCount.Count > 0) { sceneSilEggCount = GlobalVariables.globVarScript.marketSceneSilEggsCount.Count; }
	}
	
	public void SpawnNewSilverEggs()
	{
		if (puzzSilEggCount > sceneSilEggCount) // Meaning I have new silver eggs to send to the panel.
		{ 	
			// for (int i = sceneSilverEggs; i < maxSilverEggs; i++)
			// {
			// 	silEggSpawned++;
			// 	silverEggs[i].SceneActivationSequence(silverEggSpawnDelay * silEggSpawned); //
			// 	sceneSilverEggs++;
			// }
			Debug.Log("puzz sil eggs > scene sil eggs" + (puzzSilEggCount - sceneSilEggCount));

			for(int i = sceneSilEggCount; i < puzzSilEggCount; i++)
			{
				silEggSpawned++;
				silEggs[GlobalVariables.globVarScript.marketPuzzSilEggsCount[i]].SetActive(true);
				silEggs[GlobalVariables.globVarScript.marketPuzzSilEggsCount[i]].GetComponent<SceneSilverEgg>().SendToPanel(GlobalVariables.globVarScript.marketPuzzSilEggsCount[i], silverEggSpawnDelay * silEggSpawned);
			}

			clickOnEggsScript.checkLvlCompleteF = silverEggSpawnDelay * silEggSpawned;
		}
	}
}
