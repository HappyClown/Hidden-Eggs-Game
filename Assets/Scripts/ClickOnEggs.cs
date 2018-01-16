using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ClickOnEggs : MonoBehaviour 
{
	Ray2D ray;
	RaycastHit2D hit;
	Vector2 mousePos2D;
	Vector3 mousePos;

	public int eggsLeft;
	private int eggsFound;
	private int totalEggs;
	private GameObject[] eggsCount;

	public Text eggCounterText;



	void Start () 
	{
		eggsCount = GameObject.FindGameObjectsWithTag("Egg");
		eggsLeft = eggsCount.Length;
		totalEggs = eggsLeft;
		eggCounterText.text = "0/" + totalEggs + " Eggs";
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

		if (hit.collider != null && Input.GetMouseButtonDown(0))
		{
			Debug.Log(hit.collider.name);
			hit.collider.gameObject.GetComponent<EggGoToCorner>().GoToCorner();
			hit.collider.enabled = false;

			eggsFound += 1;
		}
	}


}
