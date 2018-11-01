using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ClickOnEggs : MonoBehaviour 
{
	RaycastHit2D hit;
	Vector2 mousePos2D;
	Vector3 mousePos;

	public LevelTapMannager levelTapManScript;
	public SceneTapEnabler scenTapEnabScript;
	public SceneFade sceneFadeScript;

	public bool iniSeq;
	public float iniSeqTimer;
	public float iniDelay; // waiting for titlecard to fade away
	public float checkNewSilEggsF;
	public float checkLvlCompleteF, allowTapF;
	public bool iniSilEggCheckB, iniLvlCompCheckB;

	[Header("Egg Info")]
	public int eggsLeft;
	[HideInInspector]
	public int eggsFound;
	private int totalEggs;
	private GameObject[] eggsCount;

	public TextMeshProUGUI eggCounterText;
	public TextMeshProUGUI silverEggCounterText;
	public TextMeshProUGUI goldenEggCounterText;

	[Header("Picked Up Eggs")]
	public Vector3 newCornerPos;
	public float cornerExtraPos;
	public Transform cornerPos;

	[Header("Puzzle")]
	public GameObject puzzleClickArea;
	public string puzzleSceneName;
	public Animation scaleAnim;
	public float puzzleUnlock;
	public ParticleSystem puzzleParticles;

	[Header("Egg Panel")]
	public GameObject eggPanel;
	public List<GameObject> eggSpots;
	public List<GameObject> silverEggSpots;
	public SceneSilverEggSpawner sceneSilEggSpaScript;
	public GameObject goldenEggSpot;
	public int goldenEggFound;
	[HideInInspector]
	public int eggMoving;
	public GameObject eggPanelHidden;
	public GameObject eggPanelShown;
	public float panelMoveSpeed;
	public float basePanelOpenTime;
	public List<GameObject> silverEggsInPanel;
	public GameObject dropDrowArrow;
	public List<GameObject> eggs;
	private float timer;
	private bool lockDropDownPanel;
	[HideInInspector]
	public bool openEggPanel;

	[Header("Level Complete")]
	public int totalEggsFound;
	public int eggsNeeded;
	public int silverEggsFound;
	public bool levelComplete;
	public LevelComplete levelCompleteScript;
	
	//public AudioSceneGeneral audioSceneGenScript;


	void Start () 
	{
		if (sceneFadeScript == null) { sceneFadeScript = GlobalVariables.globVarScript.GetComponent<SceneFade>(); }
		eggsCount = GameObject.FindGameObjectsWithTag("Egg");
		eggsLeft = eggsCount.Length;
		totalEggs = eggsLeft;
		silverEggCounterText.text = "Silver:" + (GlobalVariables.globVarScript.marketSilverEggsCount);
		goldenEggCounterText.text = "Golden:" + (GlobalVariables.globVarScript.rainbowRiddleSolved);
		newCornerPos = cornerPos.position;
		if (iniDelay < sceneFadeScript.fadeTime) { iniDelay = sceneFadeScript.fadeTime; }
		AdjustLevelComplete(); // Check if level has already been completed. (bool)
		MakeSilverEggsAppear(); // Make the silver eggs already found appear in the egg panel.
		AdjustSilverEggCount(); // Silver egg text.
		AdjustGoldenEggCount(); // Golden egg text.
		AdjustTotalEggsFound();	// Total eggs found = to what it was last time the scene was openned. // Not sure if this is needed.
		iniSeq = true;
		//CheckIfLevelComplete(); // if "thisSceneName" level complete screen was not played, check to see if its complete. (probably for when the players last eggs are from the puzzle)
	}


	void Update()
	{
		// initial delay  -> check silver eggs -> check lvl complete -> allow play
		if (iniSeq)
		{
			if (iniDelay > 0) iniDelay -= Time.deltaTime;
			else
			{
				iniSeqTimer += Time.deltaTime;
				if (iniSeqTimer > checkNewSilEggsF && !iniSilEggCheckB) { sceneSilEggSpaScript.SpawnNewSilverEggs(); iniSilEggCheckB = true; allowTapF += checkLvlCompleteF; }
				if (iniSeqTimer > checkLvlCompleteF && !iniLvlCompCheckB) { if (totalEggsFound == eggsNeeded && !levelComplete) { PlayLvlCompleteSeq(); } ; iniLvlCompCheckB = true; }
				if (iniSeqTimer > allowTapF) 
				{ 
					if (!levelCompleteScript.inLvlCompSeqSetup) 
					{
						scenTapEnabScript.canTapEggRidPanPuz = true;
						scenTapEnabScript.canTapHelpBird = true;
						scenTapEnabScript.canTapPauseBtn = true; 
					}
					iniSeq = false; 
				}
			}
		}
		// -- ON CLICK/TAP -- //
		if (Input.GetMouseButtonDown(0))
			{
				mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				mousePos2D = new Vector2 (mousePos.x, mousePos.y);
				hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f);
				//Debug.DrawRay(mousePos2D, Vector3.forward, Color.red, 60f);
				if (hit)//levelTapManScripthit
				{
					if (scenTapEnabScript.canTapEggRidPanPuz) // On regular eggs, puzzle, eggPanel
					{
						// - Egg Tapped - //
						if (hit.collider.CompareTag("Egg"))
						{
							Debug.Log(hit.collider.name);
							EggGoToCorner eggScript = hit.collider.gameObject.GetComponent<EggGoToCorner>();
							eggScript.EggFound();
							hit.collider.enabled = false;

							eggsFound += 1;
							eggMoving += 1;
							openEggPanel = true;
							UpdateEggsString();
							//Play egg  click sound
							//audioSceneGenScript.

							AddEggsFound();
							if (levelComplete) { levelCompleteScript.inLvlCompSeqSetup = true; }
							eggScript.SaveEggToCorrectFile();
						}

						// - Go To Puzzle Scene - //
						if (hit.collider.CompareTag("Puzzle"))
						{
							SceneFade.SwitchScene(puzzleSceneName);
							PlayerPrefs.SetString ("LastLoadedScene", SceneManager.GetActiveScene().name);
						}

						// - Opening Egg Panel Manually - //
						if (hit.collider.CompareTag("EggPanel"))
						{
							if (lockDropDownPanel)
							{
								openEggPanel = false;
								lockDropDownPanel = false;
								return;
							}

							if (eggMoving <= 0)
							{
								openEggPanel = true;
								lockDropDownPanel = true;
							}

							if (eggMoving > 0)
							{
								lockDropDownPanel = true;
							}
						}
					}
					if (scenTapEnabScript.canTapGoldEgg)
					{
						// - Golden Egg Tapped - //
						if ((hit.collider.CompareTag("GoldenEgg")))
						{
							EggGoToCorner eggScript = hit.collider.gameObject.GetComponent<EggGoToCorner>();
							eggScript.EggFound();
							hit.collider.enabled = false;

							AdjustGoldenEggCount();

							AddEggsFound();
							eggScript.SaveEggToCorrectFile();
						}
					}
				}
			}

		// - Play the level complete sequence - //
		if (totalEggsFound == eggsNeeded && !levelComplete  && !iniSeq)
		{
			if (eggMoving <= 0)
			{
				openEggPanel = false;
				lockDropDownPanel = false;
				PlayLvlCompleteSeq();
			}
		}

		// -- Egg Panel Movement -- //
		if (eggMoving <= 0 && !lockDropDownPanel)
		{
			// - Hide Egg Panel - //
			if (timer <= basePanelOpenTime && openEggPanel)
			{
				timer += Time.deltaTime;	
			} else { openEggPanel = false; timer = 0f;}

			eggPanel.transform.position = Vector3.MoveTowards(eggPanel.transform.position, eggPanelHidden.transform.position, Time.deltaTime * panelMoveSpeed);
			dropDrowArrow.transform.eulerAngles = new Vector3(dropDrowArrow.transform.eulerAngles.x, dropDrowArrow.transform.eulerAngles.y , 180);
		}

		if (eggMoving > 0 || openEggPanel)
		{
			// - Show Egg Panel - //
			eggPanel.transform.position = Vector3.MoveTowards(eggPanel.transform.position, eggPanelShown.transform.position, Time.deltaTime * panelMoveSpeed);
			dropDrowArrow.transform.eulerAngles = new Vector3(dropDrowArrow.transform.eulerAngles.x, dropDrowArrow.transform.eulerAngles.y , 0);
		}


		// - Activate Puzzle - //
		if (puzzleClickArea.activeSelf == false && eggsFound >= puzzleUnlock)
		{
			puzzleClickArea.SetActive(true);
			var emission = puzzleParticles.emission;
			emission.enabled = true;
			scaleAnim.Play();
		}
	}


	#region Methods
	public void UpdateEggsString()
	{
		eggCounterText.text = "Eggs Found: " + (eggsFound) + "/" + (totalEggs);
	}


	public void PlayLvlCompleteSeq()
	{
		scenTapEnabScript.canTapEggRidPanPuz = false;
		scenTapEnabScript.canTapHelpBird = false;
		scenTapEnabScript.canTapPauseBtn = true;
		scenTapEnabScript.canTapLvlComp = true;

		levelCompleteScript.inLvlCompSeqSetup = true;
	}


	public void AddEggsFound()
	{
		totalEggsFound = eggsFound + silverEggsFound + goldenEggFound;

		// if (totalEggsFound == eggsNeeded && !levelComplete)
		// {
		// 	levelComplete = true;
		// 	SaveLevelComplete();
		// }
	}


	// --- Dependant On Scene Name --- //
	public void MakeSilverEggsAppear() // Could be merged with AdjustSilverEggCount since they will always be called together IF we implement the egg panel in the puzzle scene
	{
		if (SceneManager.GetActiveScene().name == GlobalVariables.globVarScript.marketName)
		{
			if (GlobalVariables.globVarScript.marketSceneSilEggsCount.Count > 0)
			{
				foreach(int silEggInPanel in GlobalVariables.globVarScript.marketSceneSilEggsCount)
				{
					silverEggsInPanel[silEggInPanel].SetActive(true);
				}
			}	
		}

		if (SceneManager.GetActiveScene().name == GlobalVariables.globVarScript.parkName)
		{
			for (int i = 0; i < GlobalVariables.globVarScript.parkSilverEggsCount; i++)
			{
				silverEggsInPanel[i].SetActive(true);
			}
		}

		if (SceneManager.GetActiveScene().name == GlobalVariables.globVarScript.beachName)
		{
			for (int i = 0; i < GlobalVariables.globVarScript.beachSilverEggsCount; i++)
			{
				silverEggsInPanel[i].SetActive(true);
			}
		}
	}


	public void AdjustSilverEggCount()
	{
		if (SceneManager.GetActiveScene().name == GlobalVariables.globVarScript.marketName)
		{
			silverEggsFound = GlobalVariables.globVarScript.marketSceneSilEggsCount.Count;
			silverEggCounterText.text = "Silver: " + silverEggsFound + "/6";
		}

		if (SceneManager.GetActiveScene().name == GlobalVariables.globVarScript.parkName)
		{
			silverEggsFound = GlobalVariables.globVarScript.parkSilverEggsCount;
			silverEggCounterText.text = "Silver: " + silverEggsFound + "/6";
		}

		if (SceneManager.GetActiveScene().name == GlobalVariables.globVarScript.beachName)
		{
			silverEggsFound = GlobalVariables.globVarScript.beachSilverEggsCount;
			silverEggCounterText.text = "Silver: " + silverEggsFound + "/6";
		}
	}


	public void AdjustGoldenEggCount()
	{
		if (SceneManager.GetActiveScene().name == GlobalVariables.globVarScript.marketName)
		{
			if (GlobalVariables.globVarScript.rainbowRiddleSolved) { goldenEggFound = 1; } else { goldenEggFound = 0; }
			goldenEggCounterText.text = "Golden: " + (goldenEggFound) + "/1";
		}

		if (SceneManager.GetActiveScene().name == GlobalVariables.globVarScript.parkName)
		{
			if (GlobalVariables.globVarScript.hopscotchRiddleSolved) { goldenEggFound = 1; } else { goldenEggFound = 0; }
			goldenEggCounterText.text = "Golden: " + (goldenEggFound) + "/1";
		}

		if (SceneManager.GetActiveScene().name == GlobalVariables.globVarScript.beachName)
		{
			if (GlobalVariables.globVarScript.crabRiddleSolved) { goldenEggFound = 1; } else { goldenEggFound = 0; }
			goldenEggCounterText.text = "Golden: " + (goldenEggFound) + "/1";
		}
	}


	public void AdjustTotalEggsFound()
	{
		if (SceneManager.GetActiveScene().name == GlobalVariables.globVarScript.marketName)
		{
			totalEggsFound = GlobalVariables.globVarScript.marketTotalEggsFound;
		}

		if (SceneManager.GetActiveScene().name == GlobalVariables.globVarScript.parkName)
		{

		}

		if (SceneManager.GetActiveScene().name == GlobalVariables.globVarScript.beachName)
		{

		}
	}


	public void SaveLevelComplete()
	{
		if (SceneManager.GetActiveScene().name == GlobalVariables.globVarScript.marketName)
		{
			GlobalVariables.globVarScript.marketLevelComplete = levelComplete;
			GlobalVariables.globVarScript.SaveEggState();
		}

		if (SceneManager.GetActiveScene().name == GlobalVariables.globVarScript.parkName)
		{

		}

		if (SceneManager.GetActiveScene().name == GlobalVariables.globVarScript.beachName)
		{

		}
	}


	public void AdjustLevelComplete()
	{
		if (SceneManager.GetActiveScene().name == GlobalVariables.globVarScript.marketName)
		{
			levelComplete = GlobalVariables.globVarScript.marketLevelComplete;
		}

		if (SceneManager.GetActiveScene().name == GlobalVariables.globVarScript.parkName)
		{
			
		}

		if (SceneManager.GetActiveScene().name == GlobalVariables.globVarScript.beachName)
		{

		}
	}
	#endregion
}