using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClamPuzzle : MonoBehaviour 
{

	Ray2D ray;
	RaycastHit2D hit;
	Vector2 mousePos2D;
	Vector3 mousePos;
	public string sceneName;

	[Header("Level Lists")]
	public List<GameObject> lvl01Clams;
	public List<ClamSpot> lvl01ClamSpots;
	public List<GameObject> lvl01BlackClams;

	public List<GameObject> lvl02Clams;
	public List<ClamSpot> lvl02ClamSpots;
	public List<GameObject> lvl02BlackClams;

	public List<GameObject> lvl03Clams;
	public List<ClamSpot> lvl03ClamSpots;
	public List<GameObject> lvl03BlackClams;

	public List<ClamSpot> tempClamSpots;
	public List<GameObject> levelXXStuffs;
	public List<int> requiredBlackClamsLevels;

	[Header("Level One")]
	public GameObject level01Stuff;
	public int amntLvl01BlackClams;
	public GameObject lvl01SilverEggs;

	[Header("Level Two")]
	public GameObject level02Stuff;
	public int amntLvl02BlackClams;
	public GameObject lvl02SilverEggs;

	[Header("Level Three")]
	public GameObject level03Stuff;
	public int amntLvl03BlackClams;
	public GameObject lvl03SilverEggs;

	[Header("Current Status Variables")]
	public bool inBetweenLvls;
	public bool settingUpLevel;
	public bool shufflingClams;
	public int silverEggsPickedUp;	
	public int curntLvl;
	public int blackClamsClicked;
	public int requiredBlackClams;

	[Header("Shuffle Variables")]
	public float timer;
	public float shuffleCD;
	public float crntShuffleAmount;
	public float shuffleCount;




	void Start () 
	{
		
	}



	void FixedUpdate () 
	{
		mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos2D = new Vector2 (mousePos.x, mousePos.y);

		hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f);
	}


	
	void Update () 
	{
		if (!inBetweenLvls)
		{
			if (hit)
			{
				Debug.Log(hit.collider.gameObject);
				if (hit.collider.CompareTag("Puzzle") && Input.GetMouseButtonDown(0))
				{
					Debug.Log(hit.collider.gameObject);
					hit.collider.GetComponent<ClamFadeInOut>().FadeIn();
					hit.collider.GetComponent<CircleCollider2D>().enabled = false;
					blackClamsClicked += 1;

				}
				if (!hit.collider.CompareTag("Puzzle") && !settingUpLevel && Input.GetMouseButtonDown(0))
				{
					inBetweenLvls = true;	
					blackClamsClicked = 0;
					shufflingClams = true; 
					settingUpLevel = true;
				}
			}

			if (blackClamsClicked == requiredBlackClams)
			{
				//levelXXStuffs[curntLvl].SetActive(false);
				curntLvl += 1;
				inBetweenLvls = true;	
				blackClamsClicked = 0;
			}
		}
		else if (inBetweenLvls)
		{
			// --- CLICK ON SILVER EGGS --- //
			if (hit && hit.collider.CompareTag("Egg") && Input.GetMouseButtonDown(0))
			{
				silverEggsPickedUp += 1;
				hit.collider.gameObject.GetComponent<SilverEggs>().StartSilverEggAnim();
				//hit.collider.gameObject.SetActive(false);
				SaveSilverEggsToCorrectFile();
			}

		// --- LEVEL ONE SETUP --- //
		// RANDOMIZE CLAM LOCATIONS
		if (curntLvl == 0)
			{
				requiredBlackClams = requiredBlackClamsLevels[0];
			if (shufflingClams && settingUpLevel)
			{
				for (int i = 0; i < lvl01Clams.Count; i++)
				{
					int availibleSpot = 0;
					GameObject availibleSpotObj = null;
					foreach (ClamSpot clam in lvl01ClamSpots)
					{
						Debug.Log("Should check all clams");
						if (clam.occupied == false)
						{
							Debug.Log("Should populate temp list");
							tempClamSpots.Add(clam);
						}
					}
					availibleSpot = Random.Range(0,tempClamSpots.Count);
					availibleSpotObj = tempClamSpots[availibleSpot].gameObject;
					Debug.Log(availibleSpotObj);
					lvl01Clams[i].transform.position = availibleSpotObj.transform.position;
					availibleSpotObj.GetComponent<ClamSpot>().occupied = true;
					lvl01Clams[i].GetComponent<ClamFadeInOut>().FadeIn(); // FADE CLAMS BACK IN
					tempClamSpots.Clear();
				}
				shufflingClams = false;

				foreach (ClamSpot clam in lvl01ClamSpots)
				{
					clam.occupied = false;
				}
			}

			// TIMER IN BETWEEN SHUFFLES FADE IN's AND OUT
			if (!shufflingClams && crntShuffleAmount < shuffleCount && settingUpLevel)
			{
				timer += Time.deltaTime;

				if (timer >= shuffleCD - 1f)
				{
					foreach(GameObject clam in lvl01Clams)
					{
						clam.GetComponent<ClamFadeInOut>().FadeOut(); // FADE CLAMS OUT
					}
				}

				if (timer >= shuffleCD)
				{
					
					shufflingClams = true;
					timer = 0;
					crntShuffleAmount += 1;
				}
			}
			else if (!shufflingClams && crntShuffleAmount == shuffleCount && settingUpLevel) // LAST FADEOUT
			{
				timer += Time.deltaTime;

				if (timer >= shuffleCD - 1f)
				{
					foreach(GameObject clam in lvl01Clams)
					{
						clam.GetComponent<ClamFadeInOut>().FadeOut();
					}
					timer = 0f;
					settingUpLevel = false;
					inBetweenLvls = false;
					crntShuffleAmount = 0f;
					foreach(GameObject blackClam in lvl01BlackClams)
					{
						blackClam.GetComponent<CircleCollider2D>().enabled = true;
					}
				}
			}
		}

		// --- LEVEL TWO SETUP --- //
		// RANDOMIZE CLAM LOCATIONS
		if (curntLvl == 1)
		{
			requiredBlackClams = requiredBlackClamsLevels[1];
			lvl01SilverEggs.SetActive(true);

			if (silverEggsPickedUp == 1)
			{
				foreach(GameObject bClam in lvl01BlackClams)
				{
					bClam.GetComponent<ClamFadeInOut>().FadeOut();
				}
				level02Stuff.SetActive(true);
				settingUpLevel = true;

				if (shufflingClams && settingUpLevel)
				{
					for (int i = 0; i < lvl02Clams.Count; i++)
					{
						int availibleSpot = 0;
						GameObject availibleSpotObj = null;
						foreach (ClamSpot clam in lvl02ClamSpots)
						{
							Debug.Log("Should check all clams");
							if (clam.occupied == false)
							{
								Debug.Log("Should populate temp list");
								tempClamSpots.Add(clam);
							}
						}
						availibleSpot = Random.Range(0,tempClamSpots.Count);
						availibleSpotObj = tempClamSpots[availibleSpot].gameObject;
						Debug.Log(availibleSpotObj);
						lvl02Clams[i].transform.position = availibleSpotObj.transform.position;
						availibleSpotObj.GetComponent<ClamSpot>().occupied = true;
						lvl02Clams[i].GetComponent<ClamFadeInOut>().FadeIn(); // FADE CLAMS BACK IN
						tempClamSpots.Clear();
					}
					shufflingClams = false;

					foreach (ClamSpot clam in lvl02ClamSpots)
					{
						clam.occupied = false;
					}
				}

				// TIMER IN BETWEEN SHUFFLES FADE IN's AND OUT
				if (!shufflingClams && crntShuffleAmount < shuffleCount && settingUpLevel)
				{
					timer += Time.deltaTime;

					if (timer >= shuffleCD - 1f)
					{
						foreach(GameObject clam in lvl02Clams)
						{
							clam.GetComponent<ClamFadeInOut>().FadeOut(); // FADE CLAMS OUT
						}
					}

					if (timer >= shuffleCD)
					{
						
						shufflingClams = true;
						timer = 0;
						crntShuffleAmount += 1;
					}
				}
				else if (!shufflingClams && crntShuffleAmount == shuffleCount && settingUpLevel) // LAST FADEOUT
				{
					timer += Time.deltaTime;

					if (timer >= shuffleCD - 1f)
					{
						foreach(GameObject clam in lvl02Clams)
						{
							clam.GetComponent<ClamFadeInOut>().FadeOut();
						}
						timer = 0f;
						settingUpLevel = false;
						inBetweenLvls = false;
						crntShuffleAmount = 0f;
						foreach(GameObject blackClam in lvl02BlackClams)
						{
							blackClam.GetComponent<CircleCollider2D>().enabled = true;
						}
					}
				}
			}
		}


				// --- LEVEL THREE SETUP --- //
		// RANDOMIZE CLAM LOCATIONS
		if (curntLvl == 2)// CHANGE REQUIRED FOR NEW LVL
		{
			requiredBlackClams = requiredBlackClamsLevels[2];// CHANGE REQUIRED FOR NEW LVL
			lvl02SilverEggs.SetActive(true); // CHANGE REQUIRED FOR NEW LVL

			if (silverEggsPickedUp == 3)
			{
				foreach(GameObject bClam in lvl02BlackClams)// CHANGE REQUIRED FOR NEW LVL
				{
					bClam.GetComponent<ClamFadeInOut>().FadeOut();
				}
				level03Stuff.SetActive(true);// CHANGE REQUIRED FOR NEW LVL
				settingUpLevel = true;

				if (shufflingClams && settingUpLevel)
				{
					for (int i = 0; i < lvl03Clams.Count; i++)// CHANGE REQUIRED FOR NEW LVL
					{
						int availibleSpot = 0;
						GameObject availibleSpotObj = null;
						foreach (ClamSpot clam in lvl03ClamSpots)// CHANGE REQUIRED FOR NEW LVL
						{
							Debug.Log("Should check all clams");
							if (clam.occupied == false)
							{
								Debug.Log("Should populate temp list");
								tempClamSpots.Add(clam);
							}
						}
						availibleSpot = Random.Range(0,tempClamSpots.Count);
						availibleSpotObj = tempClamSpots[availibleSpot].gameObject;
						Debug.Log(availibleSpotObj);
						lvl03Clams[i].transform.position = availibleSpotObj.transform.position;// CHANGE REQUIRED FOR NEW LVL
						availibleSpotObj.GetComponent<ClamSpot>().occupied = true;
						lvl03Clams[i].GetComponent<ClamFadeInOut>().FadeIn(); // FADE CLAMS BACK IN // CHANGE REQUIRED FOR NEW LVL
						tempClamSpots.Clear();
					}
					shufflingClams = false;

					foreach (ClamSpot clam in lvl03ClamSpots)// CHANGE REQUIRED FOR NEW LVL
					{
						clam.occupied = false;
					}
				}

				// TIMER IN BETWEEN SHUFFLES FADE IN's AND OUT
				if (!shufflingClams && crntShuffleAmount < shuffleCount && settingUpLevel)
				{
					timer += Time.deltaTime;

					if (timer >= shuffleCD - 1f)
					{
						foreach(GameObject clam in lvl03Clams)// CHANGE REQUIRED FOR NEW LVL
						{
							clam.GetComponent<ClamFadeInOut>().FadeOut(); // FADE CLAMS OUT
						}
					}

					if (timer >= shuffleCD)
					{
						
						shufflingClams = true;
						timer = 0;
						crntShuffleAmount += 1;
					}
				}
				else if (!shufflingClams && crntShuffleAmount == shuffleCount && settingUpLevel) // LAST FADEOUT
				{
					timer += Time.deltaTime;

					if (timer >= shuffleCD - 1f)
					{
						foreach(GameObject clam in lvl03Clams)// CHANGE REQUIRED FOR NEW LVL
						{
							clam.GetComponent<ClamFadeInOut>().FadeOut();
						}
						timer = 0f;
						settingUpLevel = false;
						inBetweenLvls = false;
						crntShuffleAmount = 0f;
						foreach(GameObject blackClam in lvl03BlackClams)// CHANGE REQUIRED FOR NEW LVL
						{
							blackClam.GetComponent<CircleCollider2D>().enabled = true;
						}
					}
				}
			}
		}
		if (curntLvl == 3)
		{
			lvl03SilverEggs.SetActive(true);
		}

		if (silverEggsPickedUp == 6)
		{
			StartCoroutine(PuzzleComplete());
		}

		}
	}



	public IEnumerator PuzzleComplete ()
	{
		yield return new WaitForSeconds(0.5f);

		Debug.Log("Puzzle Completed cognraturations!!!");

		yield return new WaitForSeconds(0.5f);

		SceneManager.LoadScene(sceneName);
	}



	public void SaveSilverEggsToCorrectFile()
	{
		if (silverEggsPickedUp > GlobalVariables.globVarScript.beachSilverEggsCount) 
		{ 
			GlobalVariables.globVarScript.beachSilverEggsCount = silverEggsPickedUp; 
			GlobalVariables.globVarScript.SaveEggState();
		}	
	}
}
