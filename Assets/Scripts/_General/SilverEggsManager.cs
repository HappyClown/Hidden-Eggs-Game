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
		silverEggsPickedUp = GlobalVariables.globVarScript.parkSilverEggsCount;
	}



	// Silver Eggs counter Saving Funcion
	public void SaveSilverEggsToCorrectFile()
	{
		if (silverEggsPickedUp > GlobalVariables.globVarScript.parkSilverEggsCount) 
		{
			GlobalVariables.globVarScript.parkTotalEggsFound += 1;
			GlobalVariables.globVarScript.parkSilverEggsCount = silverEggsPickedUp; 
			GlobalVariables.globVarScript.SaveEggState();
		}
	}
	//New Silver Eggs Found Saving Function
	public void SaveNewSilEggsFound(int newSilEggFound)
	{
		//bool alreadySaved = false;
		foreach (int silEggNumber in GlobalVariables.globVarScript.parkPuzzSilEggsCount)
		{
			if (silEggNumber == newSilEggFound)
			{
				return;
			}
		}
		GlobalVariables.globVarScript.parkPuzzSilEggsCount.Add(newSilEggFound);
		GlobalVariables.globVarScript.SaveEggState();
	}
}
