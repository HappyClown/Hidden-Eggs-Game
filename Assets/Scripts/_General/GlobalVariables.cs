using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GlobalVariables : MonoBehaviour 
{
	public static GlobalVariables globVarScript;

	public string previousScene;
	public string currentScene;

	[Header("Scene Names")]
	public string menuName;
	public string marketName, parkName, beachName;
	public string marketPuzName, parkPuzName, beachPuzName;

	[Header("Eggs")]
	public List<bool> eggsFoundBools;
	public int silverEggsCount;
	public bool eggToSave;
	public bool RiddleSolved;

	[Header("Market Eggs")]
	public List<bool> marketEggsFoundBools;
	public int marketSilverEggsCount;
	//public bool marketEggToSave;
	public bool rainbowRiddleSolved;
	public int marketTotalEggsFound;

	[Header("Park Eggs")]
	public List<bool> parkEggsFoundBools;
	public int parkSilverEggsCount;
	//public bool parkEggToSave;
	public bool hopscotchRiddleSolved;
	public int parkTotalEggsFound;

	[Header("Beach Eggs")]
	public List<bool> beachEggsFoundBools;
	public int beachSilverEggsCount;
	//public bool beachEggToSave;
	public bool crabRiddleSolved;
	public int beachTotalEggsFound;

	[Header("Script References")]
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

		FindClickOnEggScript();
		FindEggHolderScript();
		LoadCorrectEggs();
	}



	void OnLevelWasLoaded()
	{
		FindClickOnEggScript();
		FindEggHolderScript();
		LoadCorrectEggs();

		Debug.Log("OnLevelWasLoaded has been called.");
	}
	


	public void SaveEggState () 
	{
		if (SceneManager.GetActiveScene().name == marketName || SceneManager.GetActiveScene().name == marketPuzName) { MarketSaveLoadManager.SaveMarketEggs(this); }

		if (SceneManager.GetActiveScene().name == parkName || SceneManager.GetActiveScene().name == parkPuzName) { ParkSaveLoadManager.SaveParkEggs(this); }
		
		if (SceneManager.GetActiveScene().name == beachName || SceneManager.GetActiveScene().name == beachPuzName) { BeachSaveLoadManager.SaveBeachEggs(this); }

		Debug.Log("Save Variables");
	}



	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			DeleteEggData();
		}
	}



	public void FindEggHolderScript()
	{
		eggHolder = null;

		if (GameObject.FindGameObjectWithTag("EggHolder")) { eggHolder = GameObject.FindGameObjectWithTag("EggHolder"); }
	}



	public void FindClickOnEggScript()
	{
		clickOnEggsScript = null;
		
		if (GameObject.Find("Game Engine")) { clickOnEggsScript = GameObject.Find("Game Engine").GetComponent<ClickOnEggs>(); }
	}



	public void LoadCorrectEggs()
	{
		// CHECK SCENE AND ASSIGN CORRECT EGGS FOUND
		if (SceneManager.GetActiveScene().name == marketName) 
		{ 
			marketEggsFoundBools = MarketSaveLoadManager.LoadMarketEggs();

			marketSilverEggsCount = MarketSaveLoadManager.LoadMarketSilverEggs();
				
			rainbowRiddleSolved = MarketSaveLoadManager.LoadRainbowRiddle(); 

			marketTotalEggsFound = MarketSaveLoadManager.LoadMarketTotalEggs();


			List<bool> loadedEggs = MarketSaveLoadManager.LoadMarketEggs();

			if (loadedEggs.Count > 2)
			{
				marketEggsFoundBools = loadedEggs;
			}


			if(marketEggsFoundBools.Count < 1)
			{
				foreach(GameObject egg in clickOnEggsScript.eggs)
				{
					Debug.Log("should be filling eggsfoundbool array");
					marketEggsFoundBools.Add(egg.GetComponent<EggGoToCorner>().eggFound);
				}
			}
		}	




		if (SceneManager.GetActiveScene().name == parkName) 
		{ 
			parkEggsFoundBools = ParkSaveLoadManager.LoadParkEggs();

			parkSilverEggsCount = ParkSaveLoadManager.LoadParkSilverEggs();
				
			hopscotchRiddleSolved = ParkSaveLoadManager.LoadHopscotchRiddle(); 


			List<bool> loadedEggs = ParkSaveLoadManager.LoadParkEggs();

			if (loadedEggs.Count > 2)
			{
				parkEggsFoundBools = loadedEggs;
			}	

		
			if(parkEggsFoundBools.Count < 1)
			{
				foreach(GameObject egg in clickOnEggsScript.eggs)
				{
					Debug.Log("should be filling eggsfoundbool array");
					parkEggsFoundBools.Add(egg.GetComponent<EggGoToCorner>().eggFound);
				}
			}
		}



		if (SceneManager.GetActiveScene().name == beachName) 
		{ 
			beachEggsFoundBools = BeachSaveLoadManager.LoadBeachEggs();

			beachSilverEggsCount = BeachSaveLoadManager.LoadBeachSilverEggs();
				
			crabRiddleSolved = BeachSaveLoadManager.LoadCrabRiddle(); 


			List<bool> loadedEggs = BeachSaveLoadManager.LoadBeachEggs();

			if (loadedEggs.Count > 2)
			{
				beachEggsFoundBools = loadedEggs;
			}	

		
			if(beachEggsFoundBools.Count < 1)
			{
				foreach(GameObject egg in clickOnEggsScript.eggs)
				{
					Debug.Log("should be filling eggsfoundbool array");
					beachEggsFoundBools.Add(egg.GetComponent<EggGoToCorner>().eggFound);
				}
			}
		}
	}


	public void DeleteEggData()
	{
		MarketSaveLoadManager.DeleteMarketSaveFile();
		ParkSaveLoadManager.DeleteParkSaveFile();
		BeachSaveLoadManager.DeleteBeachSaveFile();
	}
}