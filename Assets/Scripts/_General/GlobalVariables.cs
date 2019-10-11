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
	public List<int> eggsFoundOrder;
	public int silverEggsCount;
	public List<int> puzzSilEggsCount;
	public List<int> sceneSilEggsCount;
	public bool riddleSolved;
	public int puzzMaxLvl;
	public int totalEggsFound;
	public bool levelComplete;
	public bool birdIntroDone;
	public bool puzzIntroDone;

	[Header("Hub Data")]
	public List<bool> dissSeasonsBools;
	public int hubTotalEggsFound;

	[Header("General Data")]
	public int levelsCompleted;
	public float lastEggTotVal = 0f;
	public bool fallLocked = false;

	[Header("Script References")]
	public ClickOnEggs clickOnEggsScript;
	public GameObject eggHolder;
	public DissolveSeasons dissSeasonsScript;
	public SceneFade sceneFadeScript;
	//camilo bools for level titles
	//[HideInInspector]
	public bool marketSE, marketGE, marketNE, parkSE, parkGE, parkNE, beachSE, beachGE, beachNE;

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

		Application.targetFrameRate = 60;
	}

	void OnLevelWasLoaded()
	{
		// Debug.Log("Level loaded check.");
		FindClickOnEggScript();
		FindEggHolderScript();
		LoadCorrectEggs();
		LoadHubDissolve();
	}

	public void SaveVillageState()
	{
		VillageSaveLoadManager.SaveVillage(this);
	}

	public void SaveGeneralData() {
		GeneralSaveLoadManager.SaveGeneralData(this);
	}

	public void Update()
	{
		if (Input.GetKeyDown("r"))
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

	public void LoadHubDissolve()
	{
		if (SceneManager.GetActiveScene().name == menuName)
		{
			dissSeasonsBools = VillageSaveLoadManager.LoadDissolvedSeasons();
			if (dissSeasonsBools.Count < 1) // Should only run once unless save file dededeleted.
			{
				for (int i = 0; i < dissSeasonsScript.dissSeasonsTemp.Count; i++)
				{
					dissSeasonsBools.Add(dissSeasonsScript.dissSeasonsTemp[i]);
				}
				SaveVillageState();
			}
		}
	}

	public void SaveEggState () {
		if (SceneManager.GetActiveScene().name == marketName || SceneManager.GetActiveScene().name == marketPuzName) { MarketSaveLoadManager.SaveMarketEggs(this); }
		if (SceneManager.GetActiveScene().name == parkName || SceneManager.GetActiveScene().name == parkPuzName) { ParkSaveLoadManager.SaveParkEggs(this); }
		if (SceneManager.GetActiveScene().name == beachName || SceneManager.GetActiveScene().name == beachPuzName) { BeachSaveLoadManager.SaveBeachEggs(this); }

		//Debug.Log("Save Variables");
	}

	public void LoadCorrectEggs() {
		hubTotalEggsFound = 0;
		levelsCompleted = GeneralSaveLoadManager.LoadLevelsCompleted();
		lastEggTotVal = GeneralSaveLoadManager.LoadLastEggTotVal();
		fallLocked = GeneralSaveLoadManager.LoadFallLocked();
		// CHECK SCENE AND ASSIGN CORRECT EGGS FOUND
		// Market
		if (SceneManager.GetActiveScene().name == marketName || SceneManager.GetActiveScene().name == marketPuzName || SceneManager.GetActiveScene().name == menuName) {
			riddleSolved = false;
			eggsFoundBools = MarketSaveLoadManager.LoadMarketEggs();
			eggsFoundOrder = MarketSaveLoadManager.LoadMarketEggsOrder();
			silverEggsCount = MarketSaveLoadManager.LoadMarketSilverEggs();
			riddleSolved = MarketSaveLoadManager.LoadRainbowRiddle(); 
			totalEggsFound = MarketSaveLoadManager.LoadMarketTotalEggs();
			puzzMaxLvl = MarketSaveLoadManager.LoadMarketPuzzMaxLvl();
			puzzSilEggsCount = MarketSaveLoadManager.LoadMarketPuzzSilEggsCount();
			sceneSilEggsCount = MarketSaveLoadManager.LoadMarketSceneSilEggsCount();
			levelComplete = MarketSaveLoadManager.LoadMarketLevelComplete();
			birdIntroDone = MarketSaveLoadManager.LoadMarketBirdIntro();
			puzzIntroDone = MarketSaveLoadManager.LoadMarketPuzzIntro();
			List<bool> loadedEggs = MarketSaveLoadManager.LoadMarketEggs();
			if (loadedEggs.Count > 2) {
				eggsFoundBools = loadedEggs;
			}
			if(clickOnEggsScript != null && eggsFoundBools.Count < 1) {
				foreach(GameObject egg in clickOnEggsScript.eggs) 
				{
					eggsFoundBools.Add(egg.GetComponent<EggGoToCorner>().eggFound);
					eggsFoundOrder.Add(0);
				}
			}

			///camilo script to count eggs for the titles
			int eggsFoundAmmount = 0;
			if(silverEggsCount == 6){
				marketSE = true;
			}
			if(riddleSolved){
				marketGE = true;
			}
			for (int i = 0; i < eggsFoundBools.Count - 1; i++)
			{
				if(eggsFoundBools[i])
				eggsFoundAmmount ++;
			}
			if(eggsFoundAmmount == (eggsFoundBools.Count-1) && eggsFoundAmmount > 0)			{
				marketNE = true;
			}
			//end camiloScript

			hubTotalEggsFound += totalEggsFound;
		}	
		// Park
		if (SceneManager.GetActiveScene().name == parkName || SceneManager.GetActiveScene().name == parkPuzName || SceneManager.GetActiveScene().name == menuName) 
		{ 
			eggsFoundBools = ParkSaveLoadManager.LoadParkEggs();
			eggsFoundOrder = ParkSaveLoadManager.LoadParkEggsOrder();
			silverEggsCount = ParkSaveLoadManager.LoadParkSilverEggs();
			riddleSolved = ParkSaveLoadManager.LoadHopscotchRiddle(); 
			totalEggsFound = ParkSaveLoadManager.LoadParkTotalEggs();
			puzzMaxLvl = ParkSaveLoadManager.LoadParkPuzzMaxLvl();
			puzzSilEggsCount = ParkSaveLoadManager.LoadParkPuzzSilEggsCount();
			sceneSilEggsCount = ParkSaveLoadManager.LoadParkSceneSilEggsCount();
			levelComplete = ParkSaveLoadManager.LoadParkLevelComplete();
			birdIntroDone = ParkSaveLoadManager.LoadParkBirdIntro();
			puzzIntroDone = ParkSaveLoadManager.LoadParkPuzzIntro();

			List<bool> loadedEggs = ParkSaveLoadManager.LoadParkEggs();
			if (loadedEggs.Count > 2) {
				eggsFoundBools = loadedEggs;
			}	
		
			if(clickOnEggsScript != null && eggsFoundBools.Count < 1) {
				foreach(GameObject egg in clickOnEggsScript.eggs)
				{
					//Debug.Log("should be filling eggsFoundBool & eggsFoundOrder lists");
					eggsFoundBools.Add(egg.GetComponent<EggGoToCorner>().eggFound);
					eggsFoundOrder.Add(0);
				}
			}

			///camilo script to count eggs for the titles
			int eggsFoundAmmount = 0;
			if(silverEggsCount == 6){
				parkSE = true;
			}
			if(riddleSolved){
				parkGE = true;
			}
			for (int i = 0; i < eggsFoundBools.Count - 1; i++)
			{
				if(eggsFoundBools[i])
				eggsFoundAmmount ++;
			}
			if(eggsFoundAmmount == (eggsFoundBools.Count-1) && eggsFoundAmmount > 0)			{
				parkNE = true;
			}
			//end camiloScript

			hubTotalEggsFound += totalEggsFound;
		}
		// Beach
		if (SceneManager.GetActiveScene().name == beachName || SceneManager.GetActiveScene().name == beachPuzName || SceneManager.GetActiveScene().name == menuName) 
		{ 
			eggsFoundBools = BeachSaveLoadManager.LoadBeachEggs();
			eggsFoundOrder = BeachSaveLoadManager.LoadBeachEggsOrder();
			silverEggsCount = BeachSaveLoadManager.LoadBeachSilverEggs();
			riddleSolved = BeachSaveLoadManager.LoadCrabRiddle(); 
			totalEggsFound = BeachSaveLoadManager.LoadBeachTotalEggs();
			puzzMaxLvl = BeachSaveLoadManager.LoadBeachPuzzMaxLvl();
			puzzSilEggsCount = BeachSaveLoadManager.LoadBeachPuzzSilEggsCount();
			sceneSilEggsCount = BeachSaveLoadManager.LoadBeachSceneSilEggsCount();
			levelComplete = BeachSaveLoadManager.LoadBeachLevelComplete();
			birdIntroDone = BeachSaveLoadManager.LoadBeachBirdIntro();
			puzzIntroDone = BeachSaveLoadManager.LoadBeachPuzzIntro();

			List<bool> loadedEggs = BeachSaveLoadManager.LoadBeachEggs();
			if (loadedEggs.Count > 2) {
				eggsFoundBools = loadedEggs;
			}	

			if(clickOnEggsScript != null && eggsFoundBools.Count < 1) {
				foreach(GameObject egg in clickOnEggsScript.eggs)
				{
					// Debug.Log("should be filling eggsfoundbool array");
					eggsFoundBools.Add(egg.GetComponent<EggGoToCorner>().eggFound);
					eggsFoundOrder.Add(0);
				}
			}
			
			///camilo script to count eggs for the titles
			int eggsFoundAmmount = 0;
			if(silverEggsCount == 6){
				beachSE = true;
			}
			if(riddleSolved){
				beachGE = true;
			}
			for (int i = 0; i < eggsFoundBools.Count - 1; i++)
			{
				if(eggsFoundBools[i])
				eggsFoundAmmount ++;
			}
			if(eggsFoundAmmount == (eggsFoundBools.Count-1) && eggsFoundAmmount > 0)			{
				beachNE = true;
			}
			//end camiloScript

			hubTotalEggsFound += totalEggsFound;
		}
	}

	public void ResetParchmentEggs() {
		marketGE = marketNE = marketSE = parkGE = parkNE = parkSE = beachGE = beachNE = beachSE = false;
	}

	public void DeleteAllData()
	{
		MarketSaveLoadManager.DeleteMarketSaveFile();
		ParkSaveLoadManager.DeleteParkSaveFile();
		BeachSaveLoadManager.DeleteBeachSaveFile();
		VillageSaveLoadManager.DeleteVillageSaveFile();
		GeneralSaveLoadManager.DeleteGeneralSaveFile();
	}
}