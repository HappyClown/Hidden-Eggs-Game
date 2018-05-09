using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickToRotateTile : MonoBehaviour 
{

	Ray2D ray;
	RaycastHit2D hit;
	Vector2 mousePos2D;
	Vector3 mousePos;

	public bool mouseClickHeld;
	public bool mouseClick;

	public float distToDrag;
	public Vector3 mouseClickOGPos;

	public GameObject tileClicked;
	public Vector3 tileClickedOGPos;

	public float timer;
	public float nowHoldingTime;

	public int connections;
	public int connectionsNeeded;

	public List <int> lvlConnectionAmnts;
	public List<GameObject> lvlTiles;
	public List<GameObject> lvlSilverEggs;
	public List<GameObject> lvlBackShadows;
	public List<GameObject> lvlKites;
	public List<float> lvlCamSizes;

	public int silverEggsPickedUp;

	public bool inBetweenLvls;

	public Camera cam;
	public float ogCamSize;
	public float camSizeIncSpeed;

	public int curntLvl;



	void Start ()
	{
		//connectionsNeeded = lvlConnectionAmnts[curntLvl - 1];

		// for (int i = 0; i < lvlConnectionAmnts.Count; i++)
		// {
			foreach(Transform tile in lvlTiles[curntLvl - 1].transform)
			{
				Debug.Log("Counter");
				if (tile.GetComponent<TileRotation>().topConnection == true) { connectionsNeeded += 1; }
				if (tile.GetComponent<TileRotation>().rightConnection == true) { connectionsNeeded += 1; }
				if (tile.GetComponent<TileRotation>().bottomConnection == true) { connectionsNeeded += 1; }
				if (tile.GetComponent<TileRotation>().leftConnection == true) { connectionsNeeded += 1; }
			}
		// }
	}	



	void FixedUpdate () 
	{
		mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos2D = new Vector2 (mousePos.x, mousePos.y);

		hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f);
	}


	void Update()
	{
		if (connections == connectionsNeeded)
		{
			Debug.Log("ya win m8!");
			inBetweenLvls = true;
			curntLvl += 1;
			//connectionsNeeded = 1;

		}


		if (!inBetweenLvls)
		{
			// --- Click detected check to see what tile is hit
			if (hit.collider != null && Input.GetMouseButtonDown(0) && hit.collider.CompareTag("Tile") && hit.collider.GetComponent<TileRotation>().canBeRotated)
			{
				Debug.Log("Click pressed");
				mouseClickOGPos = mousePos;
				timer = 0f;
				// Select tile
				mouseClick = true;
				tileClicked = hit.collider.gameObject;
				tileClickedOGPos = tileClicked.transform.position;
				tileClicked.GetComponent<BoxCollider2D>().enabled = false;
				
				//Debug.Log(hit.collider.name);
				//hit.collider.transform.eulerAngles = new Vector3(hit.collider.transform.eulerAngles.x, hit.collider.transform.eulerAngles.y, hit.collider.transform.eulerAngles.z - 90);
				//return;
			}

			// --- Check to see if holding click
			if (Input.GetMouseButton(0) && tileClicked != null)
			{
				if (mouseClickHeld == false)
				{	
					timer += Time.deltaTime;
				}

				// if (Vector3.Distance(mouseClickOGPos, mousePos) > distToDrag)
				// {

				// }

				// --- Clicked long enough 
				if (timer >= nowHoldingTime || mouseClickHeld || Vector3.Distance(mouseClickOGPos, mousePos) > distToDrag) // OR MOVED MOUSE FARTHER THEN xx
				{
					Debug.Log("Click now held");
					mouseClickHeld = true;
					mouseClick = false;
					timer = 0f;

					float tileClickedX = tileClicked.transform.position.x;
					float tileClickedY = tileClicked.transform.position.y;

					tileClickedX = mousePos.x;
					tileClickedY = mousePos.y;

					tileClicked.transform.position = new Vector3(tileClickedX, tileClickedY, -5f);
				}
			}

			// --- Click released after holding
			if (hit.collider != null && Input.GetMouseButtonUp(0) && hit.collider.CompareTag("Tile") && tileClicked != null)
			{
				Debug.Log("Click released after held");
				if (mouseClickHeld && hit.collider.GetComponent<TileRotation>().canBeRotated)
				{
					tileClicked.transform.position = hit.collider.gameObject.transform.position;
					hit.collider.gameObject.transform.position = tileClickedOGPos;
				}
				else
				{
					tileClicked.transform.position = tileClickedOGPos;
				}

				tileClicked.GetComponent<BoxCollider2D>().enabled = true;
				mouseClickHeld = false;
				mouseClick = false;
				tileClicked = null;
				timer = 0f;
				connections = 0;

				foreach (Transform tile in lvlTiles[curntLvl - 1].transform)
				{
					Debug.Log("make tiles check neighbors");
					tile.gameObject.GetComponent<TileRotation>().CheckNeighbors();
				}

			}

			// --- Click released just a click
			if (hit.collider == null && Input.GetMouseButtonUp(0) && tileClicked != null)
			{
				Debug.Log("Click released after click");
				if (mouseClick)
				{
					//tileClicked.transform.eulerAngles = new Vector3(tileClicked.transform.eulerAngles.x, tileClicked.transform.eulerAngles.y, tileClicked.transform.eulerAngles.z - 90);
					// make tile roo roororo tate
					tileClicked.GetComponent<TileRotation>().RotateTile();
				}

				tileClicked.transform.position = tileClickedOGPos;

				tileClicked.GetComponent<BoxCollider2D>().enabled = true;
				mouseClickHeld = false;
				mouseClick = false;
				tileClicked = null;
				timer = 0f;
				connections = 0;

				foreach (Transform tile in lvlTiles[curntLvl - 1].transform)
				{
					Debug.Log("make tiles check neighbors");
					tile.gameObject.GetComponent<TileRotation>().CheckNeighbors();
				}
			}
		}
		// --- IN BETWEEN LEVELS --- //
		else if (inBetweenLvls)
		{
			connections = 0;
			foreach(Transform lvlTile in lvlTiles[curntLvl - 2].transform)
			{
				lvlTile.gameObject.GetComponent<FadeInOut>().FadeOut();
				if (lvlTile.transform.childCount > 0)
				{
					lvlTile.transform.GetChild(0).GetComponent<FadeInOut>().FadeOut();
				}
			}
			lvlBackShadows[curntLvl - 2].GetComponent<FadeInOut>().FadeOut();
			if (lvlKites[curntLvl - 2].transform.childCount > 0)
			{
				foreach(Transform lvlKite in lvlKites[curntLvl - 2].transform)
				{
					lvlKite.GetComponent<FadeInOut>().FadeOut();
				}
			}
			lvlKites[curntLvl - 2].GetComponent<FadeInOut>().FadeOut();


			// -- SPAWN SILVER EGGS -- //
			lvlSilverEggs[curntLvl - 2].SetActive(true);

			if (hit.collider != null && hit.collider.CompareTag("Egg") && Input.GetMouseButton(0))
			{
				silverEggsPickedUp += 1;
				hit.collider.gameObject.SetActive(false);
				SaveSilverEggsToCorrectFile();
			}


			// - SET UP LEVEL 02 - //
			if (silverEggsPickedUp == 1)
			{
				cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, lvlCamSizes[curntLvl-1], Time.deltaTime * camSizeIncSpeed);
			}

			if (curntLvl == 2 && silverEggsPickedUp == 1 && cam.orthographicSize == lvlCamSizes[curntLvl-1])
			{
				// Should I have a lvlXTiles holder gameobject reference or do a loop for each tile, does it matter really who knows. Not me.
				if (((lvlCamSizes[curntLvl-1]) - cam.orthographicSize) <= 0.001f)
				{
					lvlBackShadows[curntLvl - 1].SetActive(true);
					lvlTiles[curntLvl - 1].SetActive(true);
					lvlKites[curntLvl - 1].SetActive(true);

					foreach(Transform lvlTile in lvlTiles[curntLvl - 1].transform)
					{
						lvlTile.gameObject.SetActive(true);
					}

					// Calculate connections needed in the new level //
					connectionsNeeded = 0;
					foreach(Transform tile in lvlTiles[curntLvl - 1].transform)
					{
						Debug.Log("Counter");
						if (tile.GetComponent<TileRotation>().topConnection == true) { connectionsNeeded += 1; }
						if (tile.GetComponent<TileRotation>().rightConnection == true) { connectionsNeeded += 1; }
						if (tile.GetComponent<TileRotation>().bottomConnection == true) { connectionsNeeded += 1; }
						if (tile.GetComponent<TileRotation>().leftConnection == true) { connectionsNeeded += 1; }
					}

					inBetweenLvls = false;
					return;
				}
			}


			// - SET UP LEVEL 03 - //
			if (silverEggsPickedUp == 3)
			{
				cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, lvlCamSizes[curntLvl-1], Time.deltaTime * camSizeIncSpeed);

				foreach(Transform lvlTile in lvlTiles[curntLvl - 2].transform)
				{
					lvlTile.gameObject.GetComponent<FadeInOut>().FadeOut();
				}
			}

			if (curntLvl == 3 && silverEggsPickedUp == 3 && cam.orthographicSize == lvlCamSizes[curntLvl-1])
			{
				// Should I have a lvlXTiles holder gameobject reference or do a loop for each tile, does it matter really who knows. Not me.
				if (((lvlCamSizes[curntLvl-1]) - cam.orthographicSize) <= 0.001f)
				{
					lvlBackShadows[curntLvl - 1].SetActive(true);
					lvlTiles[curntLvl - 1].SetActive(true);
					lvlKites[curntLvl - 1].SetActive(true);

					

					foreach(Transform lvlTile in lvlTiles[curntLvl - 1].transform)
					{
						lvlTile.gameObject.SetActive(true);
					}

					// Calculate connections needed in the new level //
					connectionsNeeded = 0;
					foreach(Transform tile in lvlTiles[curntLvl - 1].transform)
					{
						Debug.Log("Counter");
						if (tile.GetComponent<TileRotation>().topConnection == true) { connectionsNeeded += 1; }
						if (tile.GetComponent<TileRotation>().rightConnection == true) { connectionsNeeded += 1; }
						if (tile.GetComponent<TileRotation>().bottomConnection == true) { connectionsNeeded += 1; }
						if (tile.GetComponent<TileRotation>().leftConnection == true) { connectionsNeeded += 1; }
					}

					inBetweenLvls = false;

					return;
				}
			}

			// - PUZZLE COMPLETE - //
			if (curntLvl == 4 && silverEggsPickedUp == 6)
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

		SceneManager.LoadScene("Park");
	}



	public void SaveSilverEggsToCorrectFile()
	{
		if (silverEggsPickedUp > GlobalVariables.globVarScript.parkSilverEggsCount) 
		{ 
			GlobalVariables.globVarScript.parkSilverEggsCount = silverEggsPickedUp; 
			GlobalVariables.globVarScript.SaveEggState();
		}	
	}
}
