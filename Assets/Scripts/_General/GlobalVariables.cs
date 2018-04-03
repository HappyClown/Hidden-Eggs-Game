using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GlobalVariables : MonoBehaviour 
{
	public static GlobalVariables globVarScript;

	public string previousScene;
	public string currentScene;

	public List<bool> eggsFoundBools;

	public int silverEggsCount;
	
	public bool eggToSave;

	public GameObject eggHolder;

	public ClickOnEggs clickOnEggsScript;

	public bool rainbowRiddleSolved;



	void OnEnable () 
	{
		if (globVarScript == null)
		{
			globVarScript = this;
			DontDestroyOnLoad(this.gameObject);
		} 
		else if (globVarScript != this.gameObject)
		{
			Destroy(this.gameObject);
			return;
		}


		// if (!eggs[1])
		// {
		// 	eggHolder = GameObject.FindGameObjectWithTag("EggPanel");
		// 	foreach(Transform childEgg in eggHolder.transform)
		// 	{
		// 		eggs.Add(childEgg.gameObject);
		// 	}
		// }
		//In builds, lock cursor in the screen window. May wanna change if windowed mode is a possibility.
		Cursor.lockState = CursorLockMode.Confined;

		eggsFoundBools = SaveLoadManager.LoadEggs();

		silverEggsCount = SaveLoadManager.LoadSilverEggs();

		rainbowRiddleSolved = SaveLoadManager.LoadRainbowRiddle();



		foreach(GameObject egg in clickOnEggsScript.eggs)
		{
			Debug.Log("should be filling eggsfoundbool array");
			eggsFoundBools.Add(egg.GetComponent<EggGoToCorner>().eggFound);
		}
		
		
		List<bool> loadedEggs = SaveLoadManager.LoadEggs();

		if (loadedEggs.Count > 2)
		{
			eggsFoundBools = loadedEggs;
		}
	}
	


	public void SaveEggState () 
	{
		SaveLoadManager.SaveEggs(this);
		Debug.Log("Save Variables");
	}



	void Update ()
	{
		if (!clickOnEggsScript)
		{
			clickOnEggsScript = GameObject.Find("Game Engine").GetComponent<ClickOnEggs>();
		}

		if (!eggHolder)
		{
			eggHolder = GameObject.FindGameObjectWithTag("EggHolder");
		}
	}
}



		// if (currentScene != SceneManager.GetActiveScene().name)
		// {
		// 	previousScene = currentScene;
		// }

		// currentScene = SceneManager.GetActiveScene().name;