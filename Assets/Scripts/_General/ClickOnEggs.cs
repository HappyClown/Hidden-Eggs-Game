using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class ClickOnEggs : MonoBehaviour 
{
	Ray2D ray;
	RaycastHit2D hit;
	Vector2 mousePos2D;
	Vector3 mousePos;

	[Header("Egg Info")]
	public int eggsLeft;
	private int eggsFound;
	private int totalEggs;
	private GameObject[] eggsCount;

	public Text eggCounterText;

	[Header("Picked Up Eggs")]
	public Vector3 newCornerPos;
	public float cornerExtraPos;
	public Transform cornerPos;

	[Header("Puzzle")]
	public GameObject puzzleClickArea;
	public string puzzleSceneName;



	void Start () 
	{
		eggsCount = GameObject.FindGameObjectsWithTag("Egg");
		eggsLeft = eggsCount.Length;
		totalEggs = eggsLeft + 1;
		eggCounterText.text = "0/" + totalEggs + " Eggs";
		newCornerPos = cornerPos.position;
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
		eggCounterText.text = (eggsFound) + "/" + totalEggs + " Eggs";

		if (hit)
		{
			if (hit.collider.CompareTag("Egg") && Input.GetMouseButtonDown(0))
			{
				Debug.Log(hit.collider.name);
				newCornerPos = new Vector3(cornerPos.position.x + (eggsFound * cornerExtraPos), cornerPos.position.y, cornerPos.position.z - (eggsFound * 0.01f));  
				hit.collider.gameObject.GetComponent<EggGoToCorner>().GoToCorner();
				hit.collider.enabled = false;

				eggsFound += 1;
			}


			if (hit.collider.CompareTag("Puzzle") && Input.GetMouseButtonDown(0))
			{
				SceneManager.LoadScene(puzzleSceneName);
				PlayerPrefs.SetString ("LastLoadedScene", SceneManager.GetActiveScene().name);
			}
		}


		if (puzzleClickArea.activeSelf == false && eggsFound == Mathf.Ceil(totalEggs*0.5f))
		{
			puzzleClickArea.SetActive(true);
		}
	}
}
