using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilverEggsManager : MonoBehaviour {

	public int silverEggsPickedUp;
	public Sprite hollowSilEgg;
	public List<GameObject> lvlSilverEggs, activeSilverEggs, allSilEggs;
	public List<SilverEggs> allSilverEggsScripts;
	public bool silverEggsActive;
	public int amntSilEggsTapped;

	void Start(){
		silverEggsPickedUp = GlobalVariables.globVarScript.silverEggsCount;
	}



	// Silver Eggs counter Saving Funcion
	public void SaveSilverEggsToCorrectFile()
	{
		if (silverEggsPickedUp > GlobalVariables.globVarScript.silverEggsCount) 
		{
			GlobalVariables.globVarScript.totalEggsFound += 1;
			GlobalVariables.globVarScript.silverEggsCount = silverEggsPickedUp; 
			GlobalVariables.globVarScript.SaveEggState();
		}
	}
	//New Silver Eggs Found Saving Function
	public void SaveNewSilEggsFound(int newSilEggFound)
	{
		//bool alreadySaved = false;
		foreach (int silEggNumber in GlobalVariables.globVarScript.puzzSilEggsCount)
		{
			if (silEggNumber == newSilEggFound)
			{
				return;
			}
		}
		GlobalVariables.globVarScript.puzzSilEggsCount.Add(newSilEggFound);
		GlobalVariables.globVarScript.SaveEggState();
	}
}
