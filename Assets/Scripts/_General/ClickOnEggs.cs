﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ClickOnEggs : MonoBehaviour 
{
	Ray2D ray;
	RaycastHit2D hit;
	Vector2 mousePos2D;
	Vector3 mousePos;

	//public List<GameObject> eggTrailPool;

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
	public GameObject goldenEggSpot;
	public int goldenEggFound;
	public int eggMoving;
	public GameObject eggPanelHidden;
	public GameObject eggPanelShown;
	public float panelMoveSpeed;
	public float panelOpenTime;
	public List<GameObject> silverEggsInPanel;
	public GameObject dropDrowArrow;

	public List<GameObject> eggs;

	public float timer;

	public bool lockDropDownPanel;



	void Start () 
	{
		eggsCount = GameObject.FindGameObjectsWithTag("Egg");
		eggsLeft = eggsCount.Length;
		totalEggs = eggsLeft;
		eggCounterText.text = "Eggs Found: 0/" + (totalEggs);
		silverEggCounterText.text = "Silver:" + (GlobalVariables.globVarScript.marketSilverEggsCount);
		goldenEggCounterText.text = "Golden:" + (GlobalVariables.globVarScript.rainbowRiddleSolved);
		newCornerPos = cornerPos.position;
		MakeSilverEggsAppear ();
	}



	void FixedUpdate () 
	{
		mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos2D = new Vector2 (mousePos.x, mousePos.y);

		hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f);
	}



	void Update()
	{		
		Debug.DrawRay(mousePos2D, Vector3.forward, Color.red, 60f);

		eggsCount = GameObject.FindGameObjectsWithTag("Egg");
		eggsLeft = eggsCount.Length;
		eggCounterText.text = "Eggs Found: " + (eggsFound) + "/" + (totalEggs);
		AdjustSilverEggCount();
		AdjustGoldenEggCount();

		if (hit)
		{
			if (hit.collider.CompareTag("Egg") && Input.GetMouseButtonDown(0) || (hit.collider.CompareTag("GoldenEgg") && Input.GetMouseButtonDown(0)))
			{
				Debug.Log(hit.collider.name);

				hit.collider.gameObject.GetComponent<EggGoToCorner>().StartEggAnim();
				hit.collider.enabled = false;

				if (hit.collider.CompareTag("Egg"))
				{
					eggsFound += 1;
				}
				eggMoving += 1;
				lockDropDownPanel = true;
			}


			if (hit.collider.CompareTag("Puzzle") && Input.GetMouseButtonDown(0))
			{
				SceneManager.LoadScene(puzzleSceneName);
				PlayerPrefs.SetString ("LastLoadedScene", SceneManager.GetActiveScene().name);
			}


			if (hit.collider.CompareTag("EggPanel") && Input.GetMouseButtonDown(0))
			{
				if (eggMoving > 0 && !lockDropDownPanel)
				{
					eggMoving  = 0;
					return;
				}

				if (eggMoving <= 0)
				{
					eggMoving += 1;
					StartCoroutine(MoveEggPanelTimer());
				}
			}
		}


		if (eggMoving <= 0)
		{
			eggPanel.transform.position = Vector3.MoveTowards(eggPanel.transform.position, eggPanelHidden.transform.position, Time.deltaTime * panelMoveSpeed);
			dropDrowArrow.transform.eulerAngles = new Vector3(dropDrowArrow.transform.eulerAngles.x, dropDrowArrow.transform.eulerAngles.y , 180);
			lockDropDownPanel = false;
		}

		if (eggMoving > 0)
		{
			eggPanel.transform.position = Vector3.MoveTowards(eggPanel.transform.position, eggPanelShown.transform.position, Time.deltaTime * panelMoveSpeed);
			dropDrowArrow.transform.eulerAngles = new Vector3(dropDrowArrow.transform.eulerAngles.x, dropDrowArrow.transform.eulerAngles.y , 0);
		}


		if (puzzleClickArea.activeSelf == false && eggsFound >= puzzleUnlock)
		{
			puzzleClickArea.SetActive(true);
			var emission = puzzleParticles.emission;
			emission.enabled = true;
			scaleAnim.Play();
		}


	}



	public IEnumerator MoveEggPanelTimer ()
	{
		while (timer < panelOpenTime)
		{
			if (eggMoving == 0)
			{
				timer = 0f;
				break;
			}
			timer += Time.deltaTime;
			if (timer >= panelOpenTime)
			{
				eggMoving -= 1;
			}
		yield return null;
		}	

		timer = 0f;
	}



	public void MakeSilverEggsAppear ()
	{
		if (SceneManager.GetActiveScene().name == "Market")
		{
			for (int i = 0; i < GlobalVariables.globVarScript.marketSilverEggsCount; i++)
			{
				silverEggsInPanel[i].SetActive(true);
				//eggsFound += 1;
			}
		}

		if (SceneManager.GetActiveScene().name == "Park")
		{
			for (int i = 0; i < GlobalVariables.globVarScript.parkSilverEggsCount; i++)
			{
				silverEggsInPanel[i].SetActive(true);
				//eggsFound += 1;
			}
		}

		if (SceneManager.GetActiveScene().name == "Beach")
		{
			for (int i = 0; i < GlobalVariables.globVarScript.beachSilverEggsCount; i++)
			{
				silverEggsInPanel[i].SetActive(true);
				//eggsFound += 1;
			}
		}
	}



	public void AdjustSilverEggCount()
	{
		if (SceneManager.GetActiveScene().name == "Market")
		{
			silverEggCounterText.text = "Silver: " + (Mathf.Clamp(GlobalVariables.globVarScript.marketSilverEggsCount, 0, 6)) + "/6";
		}

		if (SceneManager.GetActiveScene().name == "Park")
		{
			silverEggCounterText.text = "Silver: " + (Mathf.Clamp(GlobalVariables.globVarScript.parkSilverEggsCount, 0, 6)) + "/6";
		}

		if (SceneManager.GetActiveScene().name == "Beach")
		{
			silverEggCounterText.text = "Silver: " + (Mathf.Clamp(GlobalVariables.globVarScript.beachSilverEggsCount, 0, 6)) + "/6";
		}
	}



	public void AdjustGoldenEggCount()
	{
		if (SceneManager.GetActiveScene().name == "Market")
		{
			if (GlobalVariables.globVarScript.rainbowRiddleSolved) { goldenEggFound = 1; } else { goldenEggFound = 0; }
			goldenEggCounterText.text = "Golden: " + (goldenEggFound) + "/1";
			
		}

		if (SceneManager.GetActiveScene().name == "Park")
		{
			if (GlobalVariables.globVarScript.hopscotchRiddleSolved) { goldenEggFound = 1; } else { goldenEggFound = 0; }
			goldenEggCounterText.text = "Golden: " + (goldenEggFound) + "/1";
		}

		if (SceneManager.GetActiveScene().name == "Beach")
		{
			if (GlobalVariables.globVarScript.crabRiddleSolved) { goldenEggFound = 1; } else { goldenEggFound = 0; }
			goldenEggCounterText.text = "Golden: " + (goldenEggFound) + "/1";
		}
	}
}