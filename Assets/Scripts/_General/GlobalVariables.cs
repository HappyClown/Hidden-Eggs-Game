﻿using System.Collections;
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

	[Header("Eggs")]
	public List<bool> eggsFoundBools;
	public int silverEggsCount;
	public bool eggToSave;
	public bool RiddleSolved;

	[Header("Village")]
	public List<bool> dissSeasonsBools;

	[Header("Market Eggs")]
	public List<bool> marketEggsFoundBools;
	public int marketSilverEggsCount;
	public List<int> marketPuzzSilEggsCount;
	public List<int> marketSceneSilEggsCount;
	//public bool marketEggToSave;
	public bool rainbowRiddleSolved;
	public int marketPuzzMaxLvl;
	public int marketTotalEggsFound;
	public bool marketLevelComplete;
	public bool marketIntroDone;

	[Header("Park Eggs")]
	public List<bool> parkEggsFoundBools;
	public int parkSilverEggsCount;
	public List<int> parkPuzzSilEggsCount;
	public List<int> parkSceneSilEggsCount;
	//public bool parkEggToSave;
	public bool hopscotchRiddleSolved;
	public int parkPuzzMaxLvl;
	public int parkTotalEggsFound;
	public bool parkLevelComplete;

	[Header("Beach Eggs")]
	public List<bool> beachEggsFoundBools;
	public int beachSilverEggsCount;
	//public bool beachEggToSave;
	public bool crabRiddleSolved;
	public int beachTotalEggsFound;

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
	

	public void SaveEggState () 
	{
		if (SceneManager.GetActiveScene().name == marketName || SceneManager.GetActiveScene().name == marketPuzName) { MarketSaveLoadManager.SaveMarketEggs(this); }

		if (SceneManager.GetActiveScene().name == parkName || SceneManager.GetActiveScene().name == parkPuzName) { ParkSaveLoadManager.SaveParkEggs(this); }
		
		if (SceneManager.GetActiveScene().name == beachName || SceneManager.GetActiveScene().name == beachPuzName) { BeachSaveLoadManager.SaveBeachEggs(this); }

		//Debug.Log("Save Variables");
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

	public void LoadCorrectEggs()
	{
		// CHECK SCENE AND ASSIGN CORRECT EGGS FOUND
		if (SceneManager.GetActiveScene().name == marketName || SceneManager.GetActiveScene().name == marketPuzName || SceneManager.GetActiveScene().name == menuName) 
		{ 
			marketEggsFoundBools = MarketSaveLoadManager.LoadMarketEggs();
			marketSilverEggsCount = MarketSaveLoadManager.LoadMarketSilverEggs();
			rainbowRiddleSolved = MarketSaveLoadManager.LoadRainbowRiddle(); 
			marketTotalEggsFound = MarketSaveLoadManager.LoadMarketTotalEggs();
			marketPuzzMaxLvl = MarketSaveLoadManager.LoadMarketPuzzMaxLvl();
			marketPuzzSilEggsCount = MarketSaveLoadManager.LoadMarketPuzzSilEggsCount();
			marketSceneSilEggsCount = MarketSaveLoadManager.LoadMarketSceneSilEggsCount();
			marketLevelComplete = MarketSaveLoadManager.LoadMarketLevelComplete();
			marketIntroDone = MarketSaveLoadManager.LoadMarketBirdIntro();

			if(clickOnEggsScript != null && marketEggsFoundBools.Count < 1)
			{
				foreach(GameObject egg in clickOnEggsScript.eggs)
				{
					marketEggsFoundBools.Add(false/* egg.GetComponent<EggGoToCorner>().eggFound */);
				}
			}
		}	

		if (SceneManager.GetActiveScene().name == parkName || SceneManager.GetActiveScene().name == parkPuzName || SceneManager.GetActiveScene().name == menuName) 
		{ 
			parkEggsFoundBools = ParkSaveLoadManager.LoadParkEggs();
			parkSilverEggsCount = ParkSaveLoadManager.LoadParkSilverEggs();
			hopscotchRiddleSolved = ParkSaveLoadManager.LoadHopscotchRiddle();
			parkTotalEggsFound = ParkSaveLoadManager.LoadParkTotalEggs();
			parkPuzzMaxLvl = ParkSaveLoadManager.LoadParkPuzzMaxLvl();
			parkPuzzSilEggsCount = ParkSaveLoadManager.LoadParkPuzzSilEggsCount();
			parkSceneSilEggsCount = ParkSaveLoadManager.LoadParkSceneSilEggsCount();
			parkLevelComplete = ParkSaveLoadManager.LoadParkLevelComplete();
		
			if(clickOnEggsScript != null && parkEggsFoundBools.Count < 1)
			{
				foreach(GameObject egg in clickOnEggsScript.eggs)
				{
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
					beachEggsFoundBools.Add(egg.GetComponent<EggGoToCorner>().eggFound);
				}
			}
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