using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GlobalVariables : MonoBehaviour 
{
	public static GlobalVariables globVarScript;
	public string previousScene;
	public string currentScene;
	public bool toHub;

	[Header("Scene Names")]
	public string menuName;
	public string marketName, parkName, beachName;
	public string marketPuzName, parkPuzName, beachPuzName;

	[Header("Level Data")]
	public List<bool> eggsFoundBools;
	public int silverEggsCount;
	public List<int> puzzSilEggsCount;
	public List<int> sceneSilEggsCount;
	public bool riddleSolved;
	public int puzzMaxLvl;
	public int totalEggsFound;
	public bool levelComplete;

	[Header("Hub Data")]
	public int hubTotalEggsFound;

	// [Header("Market Eggs")]
	// public List<bool> eggsFoundBools;
	// public int silverEggsCount;
	// public List<int> puzzSilEggsCount;
	// public List<int> sceneSilEggsCount;
	// //public bool marketEggToSave;
	// public bool riddleSolved;
	// public int puzzMaxLvl;
	// public int totalEggsFound;
	// public bool levelComplete;

	// [Header("Park Eggs")]
	// public List<bool> eggsFoundBools;
	// public int silverEggsCount;
	// public List<int> puzzSilEggsCount;
	// public List<int> sceneSilEggsCount;
	// //public bool parkEggToSave;
	// public bool riddleSolved;
	// public int puzzMaxLvl;
	// public int totalEggsFound;
	// public bool levelComplete;

	// [Header("Beach Eggs")]
	// public List<bool> eggsFoundBools;
	// public int silverEggsCount;
	// //public bool beachEggToSave;
	// public bool riddleSolved;
	// public int totalEggsFound;

	[Header("Script References")]
	public ClickOnEggs clickOnEggsScript;
	public GameObject eggHolder;
	public DissolveSeasons dissSeasonsScript;


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
		LoadHubDissolve();
		//LoadCorrectPuzz();
	}


	void OnLevelWasLoaded()
	{
		Debug.Log("Level loaded check.");
		FindClickOnEggScript();
		FindEggHolderScript();
		LoadCorrectEggs(); // Do I really wanna do this here every scene loaded ??? ***
		LoadHubDissolve();
		//Debug.Log("OnLevelWasLoaded has been called.");
	}



	public void SaveVillageState()
	{
		VillageSaveLoadManager.SaveVillage(this);
	}


	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			DeleteAllData();
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
		// Change to find a tag or something. OR Have the clickOnEggScript give itself to GlobalVariables on awake.
		if (GameObject.Find("Game Engine")) { clickOnEggsScript = GameObject.Find("Game Engine").GetComponent<ClickOnEggs>(); }
	}

	// public void FindUnlockedSeasonsScript()
	// {
	// 	unlockedSeasonsScript = null;
		
	// 	if (GameObject.Find("Game Engine")) { clickOnEggsScript = GameObject.Find("Game Engine").GetComponent<ClickOnEggs>(); }
	// }

	public void LoadHubDissolve()
	{
		if (SceneManager.GetActiveScene().name == menuName)
		{
			//Debug.Log("Loading dissolved seasons.");
			//SaveVillageState();
			dissSeasonsBools = VillageSaveLoadManager.LoadDissolvedSeasons();
			if (dissSeasonsBools.Count < 1) // Should only run once unless save file dededeleted.
			{
				//Debug.Log("Should only see this once per save.");
				//dissSeasonsBools = dissSeasonsScript.dissSeasonsTemp;
				for (int i = 0; i < dissSeasonsScript.dissSeasonsTemp.Count; i++)
				{
					dissSeasonsBools.Add(dissSeasonsScript.dissSeasonsTemp[i]);
				}
				SaveVillageState();
			}
		}
	}

	public void SaveEggState () 
	{
		if (SceneManager.GetActiveScene().name == marketName || SceneManager.GetActiveScene().name == marketPuzName) { MarketSaveLoadManager.SaveMarketEggs(this); }

		if (SceneManager.GetActiveScene().name == parkName || SceneManager.GetActiveScene().name == parkPuzName) { ParkSaveLoadManager.SaveParkEggs(this); }
		
		if (SceneManager.GetActiveScene().name == beachName || SceneManager.GetActiveScene().name == beachPuzName) { BeachSaveLoadManager.SaveBeachEggs(this); }

		Debug.Log("Save Variables");
	}


	public void LoadCorrectEggs()
	{
		hubTotalEggsFound = 0;
		// CHECK SCENE AND ASSIGN CORRECT EGGS FOUND
		if (SceneManager.GetActiveScene().name == marketName || SceneManager.GetActiveScene().name == marketPuzName || SceneManager.GetActiveScene().name == menuName) 
		{
			eggsFoundBools = MarketSaveLoadManager.LoadMarketEggs();

			silverEggsCount = MarketSaveLoadManager.LoadMarketSilverEggs();

			riddleSolved = MarketSaveLoadManager.LoadRainbowRiddle(); 

			totalEggsFound = MarketSaveLoadManager.LoadMarketTotalEggs();

			puzzMaxLvl = MarketSaveLoadManager.LoadMarketPuzzMaxLvl();
			//Debug.Log("Loaded " + SceneManager.GetActiveScene().name + "'s max level.");

			puzzSilEggsCount = MarketSaveLoadManager.LoadMarketPuzzSilEggsCount();
			Debug.Log(puzzSilEggsCount);

			sceneSilEggsCount = MarketSaveLoadManager.LoadMarketSceneSilEggsCount();
			Debug.Log(sceneSilEggsCount);

			levelComplete = MarketSaveLoadManager.LoadMarketLevelComplete();


			List<bool> loadedEggs = MarketSaveLoadManager.LoadMarketEggs();

			if (loadedEggs.Count > 2)
			{
				eggsFoundBools = loadedEggs;
			}


			if(clickOnEggsScript != null && eggsFoundBools.Count < 1)
			{
				foreach(GameObject egg in clickOnEggsScript.eggs)
				{
					Debug.Log("should be filling eggsfoundbool array");
					eggsFoundBools.Add(egg.GetComponent<EggGoToCorner>().eggFound);
				}
			}

			hubTotalEggsFound += totalEggsFound;
		}	

		if (SceneManager.GetActiveScene().name == parkName || SceneManager.GetActiveScene().name == parkPuzName || SceneManager.GetActiveScene().name == menuName) 
		{ 
			eggsFoundBools = ParkSaveLoadManager.LoadParkEggs();

			silverEggsCount = ParkSaveLoadManager.LoadParkSilverEggs();
				
			riddleSolved = ParkSaveLoadManager.LoadHopscotchRiddle(); 

			totalEggsFound = ParkSaveLoadManager.LoadParkTotalEggs();

			puzzMaxLvl = ParkSaveLoadManager.LoadParkPuzzMaxLvl();
			//Debug.Log("Loaded " + SceneManager.GetActiveScene().name + "'s max level.");

			puzzSilEggsCount = ParkSaveLoadManager.LoadParkPuzzSilEggsCount();
			Debug.Log(puzzSilEggsCount);

			sceneSilEggsCount = ParkSaveLoadManager.LoadParkSceneSilEggsCount();
			Debug.Log(sceneSilEggsCount);

			levelComplete = ParkSaveLoadManager.LoadParkLevelComplete();


			List<bool> loadedEggs = ParkSaveLoadManager.LoadParkEggs();

			if (loadedEggs.Count > 2)
			{
				eggsFoundBools = loadedEggs;
			}	

		
			if(clickOnEggsScript != null && eggsFoundBools.Count < 1)
			{
				foreach(GameObject egg in clickOnEggsScript.eggs)
				{
					Debug.Log("should be filling eggsfoundbool array");
					eggsFoundBools.Add(egg.GetComponent<EggGoToCorner>().eggFound);
				}
			}

			hubTotalEggsFound += totalEggsFound;
		}

		if (SceneManager.GetActiveScene().name == beachName) 
		{ 
			eggsFoundBools = BeachSaveLoadManager.LoadBeachEggs();

			silverEggsCount = BeachSaveLoadManager.LoadBeachSilverEggs();
				
			riddleSolved = BeachSaveLoadManager.LoadCrabRiddle(); 

			List<bool> loadedEggs = BeachSaveLoadManager.LoadBeachEggs();

			if (loadedEggs.Count > 2)
			{
				eggsFoundBools = loadedEggs;
			}	

		
			if(eggsFoundBools.Count < 1)
			{
				foreach(GameObject egg in clickOnEggsScript.eggs)
				{
					Debug.Log("should be filling eggsfoundbool array");
					eggsFoundBools.Add(egg.GetComponent<EggGoToCorner>().eggFound);
				}
			}

			hubTotalEggsFound += totalEggsFound;
		}
	}

	public void DeleteAllData()
	{
		MarketSaveLoadManager.DeleteMarketSaveFile();
		ParkSaveLoadManager.DeleteParkSaveFile();
		BeachSaveLoadManager.DeleteBeachSaveFile();
		VillageSaveLoadManager.DeleteVillageSaveFile();
	}
}