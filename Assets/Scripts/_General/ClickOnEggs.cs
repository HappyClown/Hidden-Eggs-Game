using System.Collections;
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

	[Header("Egg Info")]
	public int eggsLeft;
	[HideInInspector]
	public int eggsFound;
	private int totalEggs;
	private GameObject[] eggsCount;

	public TextMeshProUGUI eggCounterText;

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
	public int eggMoving;
	public GameObject eggPanelHidden;
	public GameObject eggPanelShown;
	public float panelMoveSpeed;
	public float panelOpenTime;
	public List<GameObject> silverEggsInPanel;

	public List<GameObject> eggs;

	public float timer;



	void Start () 
	{
		eggsCount = GameObject.FindGameObjectsWithTag("Egg");
		eggsLeft = eggsCount.Length;
		totalEggs = eggsLeft;
		eggCounterText.text = "Eggs Found: 0/" + (totalEggs + 7);
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
		eggCounterText.text = "Eggs Found: " + (eggsFound) + "/" + (totalEggs + 7);

		if (hit)
		{
			if (hit.collider.CompareTag("Egg") && Input.GetMouseButtonDown(0) || (hit.collider.CompareTag("GoldenEgg") && Input.GetMouseButtonDown(0)))
			{
				Debug.Log(hit.collider.name);
				//newCornerPos = eggSpots[eggsFound].transform.position;
				//hit.collider.gameObject.transform.parent = eggPanel.transform;
				hit.collider.gameObject.GetComponent<EggGoToCorner>().StartEggAnim();
				hit.collider.enabled = false;

				eggsFound += 1;
				eggMoving += 1;
			}


			if (hit.collider.CompareTag("Puzzle") && Input.GetMouseButtonDown(0))
			{
				SceneManager.LoadScene(puzzleSceneName);
				PlayerPrefs.SetString ("LastLoadedScene", SceneManager.GetActiveScene().name);
			}


			if (hit.collider.CompareTag("EggPanel") && Input.GetMouseButtonDown(0))
			{
				if (eggMoving <= 0)
				{
					eggMoving += 1;
					Debug.Log("Hello.");
					StartCoroutine(MoveEggPanelTimer());
					Debug.Log("Hello.");
				}
			}
		}


		if (eggMoving <= 0)
		{
			eggPanel.transform.position = Vector3.MoveTowards(eggPanel.transform.position, eggPanelHidden.transform.position, Time.deltaTime * panelMoveSpeed);
		}

		if (eggMoving > 0)
		{
			eggPanel.transform.position = Vector3.MoveTowards(eggPanel.transform.position, eggPanelShown.transform.position, Time.deltaTime * panelMoveSpeed);
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
		Debug.Log("Hello.");
		while (timer < panelOpenTime)
		{
			timer += Time.deltaTime;
			if (timer >= panelOpenTime)
			{
				eggMoving -= 1;
			}
		yield return null;
		}	
		Debug.Log("Hello.");
		timer = 0f;
	}



	public void MakeSilverEggsAppear ()
	{
		for (int i = 0; i < GlobalVariables.globVarScript.silverEggsCount; i++)
		{
			silverEggsInPanel[i].SetActive(true);
			eggsFound += 1;
		}
	}
}