using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSilverEggSpawner : MonoBehaviour 
{
	public float silverEggSpawnDelay;
	private int silEggSpawned;
	public int puzzSilEggCount;
	public int sceneSilEggCount;
	public ClickOnEggs clickOnEggsScript;
	public List<GameObject> silEggs;

	public List<int> puzzSilEggCountList;
	public List<int> sceneSilEggCountList;

	void Awake()
	{
		SetCorrectLevelLists();

		if (puzzSilEggCountList.Count > 0) { puzzSilEggCount = puzzSilEggCountList.Count; }
		if (sceneSilEggCountList.Count > 0) { sceneSilEggCount = sceneSilEggCountList.Count; }
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
				silEggs[puzzSilEggCountList[i]].SetActive(true);
				silEggs[puzzSilEggCountList[i]].GetComponent<SceneSilverEgg>().SendToPanel(puzzSilEggCountList[i], silverEggSpawnDelay * silEggSpawned);
			}

			clickOnEggsScript.checkLvlCompleteF = silverEggSpawnDelay * silEggSpawned;
		}
	}


	public void SetCorrectLevelLists()
	{
		if (SceneManager.GetActiveScene().name == GlobalVariables.globVarScript.marketName)
		{
			if (GlobalVariables.globVarScript.marketPuzzSilEggsCount.Count > 0) { puzzSilEggCountList = GlobalVariables.globVarScript.marketPuzzSilEggsCount; }
			if (GlobalVariables.globVarScript.marketSceneSilEggsCount.Count > 0) { sceneSilEggCountList = GlobalVariables.globVarScript.marketSceneSilEggsCount; }
		}

		if (SceneManager.GetActiveScene().name == GlobalVariables.globVarScript.parkName)
		{
			if (GlobalVariables.globVarScript.parkPuzzSilEggsCount.Count > 0) { puzzSilEggCountList = GlobalVariables.globVarScript.parkPuzzSilEggsCount; }
			if (GlobalVariables.globVarScript.parkSceneSilEggsCount.Count > 0) { sceneSilEggCountList = GlobalVariables.globVarScript.parkSceneSilEggsCount; }
		}
	}
}
