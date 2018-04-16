using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GlobalVariables : MonoBehaviour 
{
	public static GlobalVariables globVarScript;

	public string previousScene;
	public string currentScene;

	[Header("Eggs")]
	public List<bool> eggsFoundBools;
	public int silverEggsCount;
	public bool eggToSave;
	public bool RiddleSolved;

	[Header("Market Eggs")]
	public List<bool> marketEggsFoundBools;
	public int marketSilverEggsCount;
	public bool marketEggToSave;
	public bool rainbowRiddleSolved;

	[Header("Park Eggs")]
	public List<bool> parkEggsFoundBools;
	public int parkSilverEggsCount;
	public bool parkEggToSave;
	public bool hopscotchRiddleSolved;

	public ClickOnEggs clickOnEggsScript;

	public GameObject eggHolder;



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
		
		Cursor.lockState = CursorLockMode.Confined;

		// CHECK SCENE AND ASSIGN CORRECT EGGS FOUND
		if (SceneManager.GetActiveScene().name == "Market") 
		{ 
			eggsFoundBools = MarketSaveLoadManager.LoadMarketEggs();

			silverEggsCount = MarketSaveLoadManager.LoadMarketSilverEggs();
			
			rainbowRiddleSolved = MarketSaveLoadManager.LoadRainbowRiddle(); 


			// List<bool> loadedEggs = MarketSaveLoadManager.LoadMarketEggs();

			// if (loadedEggs.Count > 2)
			// {
			// 	eggsFoundBools = loadedEggs;
			// }
			
		}	


		if (SceneManager.GetActiveScene().name == "Park") 
		{ 
			eggsFoundBools = ParkSaveLoadManager.LoadParkEggs();

			silverEggsCount = ParkSaveLoadManager.LoadParkSilverEggs();
			
			hopscotchRiddleSolved = ParkSaveLoadManager.LoadHopscotchRiddle(); 


			List<bool> loadedEggs = ParkSaveLoadManager.LoadParkEggs();

			if (loadedEggs.Count > 2)
			{
				eggsFoundBools = loadedEggs;
			}
			
		}	


		if(eggsFoundBools.Count < 1)
		{
			foreach(GameObject egg in clickOnEggsScript.eggs)
			{
				Debug.Log("should be filling eggsfoundbool array");
				eggsFoundBools.Add(egg.GetComponent<EggGoToCorner>().eggFound);
			}
		}
		
		
		// eggsFoundBools = marketEggsFoundBools;

		// silverEggsCount = marketSilverEggsCount;
	}
	


	public void SaveEggState () 
	{
		if (SceneManager.GetActiveScene().name == "Market") { MarketSaveLoadManager.SaveMarketEggs(this); }

		if (SceneManager.GetActiveScene().name == "Park") { ParkSaveLoadManager.SaveParkEggs(this); }
		
		//if (SceneManager.GetActiveScene().name == "Beach") { ParkSaveLoadManager.SaveBeachEggs(this); }

		Debug.Log("Save Variables");
	}



	void Update ()
	{
		if (!clickOnEggsScript)
		{
			Debug.Log("no click on egg");
			if (GameObject.Find("Game Engine"))
			{
				clickOnEggsScript = GameObject.Find("Game Engine").GetComponent<ClickOnEggs>();
			}
			else
			{
				clickOnEggsScript = null;
			}
		}

		if (!eggHolder)
		{
			eggHolder = GameObject.FindGameObjectWithTag("EggHolder");
		}

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			MarketSaveLoadManager.DeleteMarketSaveFile();
			ParkSaveLoadManager.DeleteParkSaveFile();
		}
	}
}



		// if (currentScene != SceneManager.GetActiveScene().name)
		// {
		// 	previousScene = currentScene;
		// }

		// currentScene = SceneManager.GetActiveScene().name;