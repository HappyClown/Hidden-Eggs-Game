using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToRotateTile : MonoBehaviour 
{

	Ray2D ray;
	RaycastHit2D hit;
	Vector2 mousePos2D;
	Vector3 mousePos;

	public bool mouseClickHeld;
	public bool mouseClick;

	public GameObject tileClicked;
	public Vector3 tileClickedOGPos;

	public float timer;
	public float nowHoldingTime;

	public int connections;
	public int connectionsNeeded;
	public List <int> lvlConnectionAmnts;
	// public int lvlOneConnectionAmnt;
	// public int lvlTwoConnectionAmnt;
	// public int lvlThreeConnectionAmnt;

	public List<GameObject> lvlTiles;
	// public List<GameObject> lvlOneTiles;
	// public List<GameObject> lvlTwoTiles;
	// public List<GameObject> lvlThreeTiles;

	public List<GameObject> lvlSilverEggs;

	public int silverEggsPickedUp;

	public bool inBetweenLvls;

	public Camera cam;
	public float ogCamSize;
	public float camSizeIncSpeed;

	public int curntLvl;



	void Start ()
	{
		connectionsNeeded = lvlConnectionAmnts[curntLvl - 1];
	}	



	void FixedUpdate () 
	{
		mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos2D = new Vector2 (mousePos.x, mousePos.y);

		hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f);
	}


	void Update()
	{
		if (connections >= connectionsNeeded)
		{
			Debug.Log("ya win m8!");
			inBetweenLvls = true;
			curntLvl += 1;
		}

		if (!inBetweenLvls)
		{

			if (hit.collider != null && Input.GetMouseButtonDown(0) && hit.collider.CompareTag("Tile") && hit.collider.GetComponent<TileRotation>().canBeRotated)
			{
				Debug.Log("Click pressed");
				// Select tile
				mouseClick = true;
				tileClicked = hit.collider.gameObject;
				tileClickedOGPos = tileClicked.transform.position;
				tileClicked.GetComponent<BoxCollider2D>().enabled = false;
				//Debug.Log(hit.collider.name);
				//hit.collider.transform.eulerAngles = new Vector3(hit.collider.transform.eulerAngles.x, hit.collider.transform.eulerAngles.y, hit.collider.transform.eulerAngles.z - 90);
				//return;
			}

			if (Input.GetMouseButton(0) && tileClicked != null)
			{
				if (mouseClickHeld == false)
				{	
					timer += Time.deltaTime;
				}

				if (timer >= nowHoldingTime || mouseClickHeld)
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
		else if (inBetweenLvls)
		{
			connections = 0;

			//spawn silver eggs
			//lvlSilverEggs[curntLvl - 1]

			if (hit.collider != null && hit.collider.CompareTag("Egg") && Input.GetMouseButton(0))
			{
				silverEggsPickedUp += 1;
				hit.collider.gameObject.SetActive(false);
			}

			if (silverEggsPickedUp == curntLvl -1)
			{
				cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, ogCamSize * curntLvl, Time.deltaTime * camSizeIncSpeed);
			}

			if (curntLvl == 2)
			{
				foreach(Transform lvlTile in lvlTiles[curntLvl - 2].transform)
				{
					lvlTile.gameObject.GetComponent<FadeInOut>().FadeOut();
				}

				//wait until   faded out && cam zoomed out.
				//lvlTwoTiles		 //should I have a lvlXTiles holder gameobject reference or do a loop for each tile, does it matter really who knows. Not me.
				if (((ogCamSize * curntLvl) - cam.orthographicSize) <= 0.001f) //&& eggs picked up
				{
					lvlTiles[curntLvl - 1].SetActive(true);

					foreach(Transform lvlTile in lvlTiles[curntLvl - 1].transform)
					{
						lvlTile.gameObject.SetActive(true);
					}

					connectionsNeeded = lvlConnectionAmnts[curntLvl - 1];
					inBetweenLvls = false;
					return;
				}
			}

			// insert lvl 3 stuff

			//insert puzzle done stuff
		}
		
	}
}
