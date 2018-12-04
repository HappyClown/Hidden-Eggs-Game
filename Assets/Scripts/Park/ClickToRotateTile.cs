using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickToRotateTile : MonoBehaviour 
{
	
	[Header("Input Detector")]
	public inputDetector myInput;
	Ray2D ray;
	RaycastHit2D hit;
	Vector2 mousePos2D;
	Vector3 mousePos;
	//public string sceneName;

	public bool mouseClickHeld;
	public bool mouseClick;

	public float distToDrag;
	public Vector3 mouseClickOGPos;
	
	public Vector3 updateMousePos;

	public GameObject tileClicked;
	public Vector3 tileClickedOGPos;

	public float timer;
	public float nowHoldingTime;

	//public int connections;
	public int connectionsNeeded;

	//public List <int> lvlConnectionAmnts;
	public List<GameObject> lvlTiles;

	//public List<GameObject> lvlSilverEggs;
	//public List<GameObject> lvlKites;
	//public List<float> lvlCamSizes;
	//public int silverEggsPickedUp;
	//public bool inBetweenLvls;
	//public Camera cam;
	//public float ogCamSize;
	//public float camSizeIncSpeed;

	//public int curntLvl;

	public bool desktopDevice = false;
	public bool handheldDevice = false;

	public KitePuzzEngine kitePuzzEngineScript;

	public AudioSceneParkPuzzle audioSceneParkPuz;

	
	void Awake ()
	{
		if (connectionsNeeded == 0) { connectionsNeeded = 99; }
	}

	void Start ()
	{
		if (SystemInfo.deviceType == DeviceType.Handheld)
		{
			handheldDevice = true;
		}
		else if (SystemInfo.deviceType == DeviceType.Desktop)
		{
			desktopDevice = true;
		}
		//connectionsNeeded = lvlConnectionAmnts[curntLvl - 1];

		// for (int i = 0; i < lvlConnectionAmnts.Count; i++)
		// {
			// foreach(Transform tile in lvlTiles[kitePuzzEngineScript.curntLvl - 1].transform)
			// {
			// 	Debug.Log("Counter");
			// 	if (tile.GetComponent<TileRotation>().topConnection == true) { connectionsNeeded += 1; }
			// 	if (tile.GetComponent<TileRotation>().rightConnection == true) { connectionsNeeded += 1; }
			// 	if (tile.GetComponent<TileRotation>().bottomConnection == true) { connectionsNeeded += 1; }
			// 	if (tile.GetComponent<TileRotation>().leftConnection == true) { connectionsNeeded += 1; }
			// }
		// }
	}	



	void Update()
	{
		if (myInput.Tapped)
		{
			mousePos = Camera.main.ScreenToWorldPoint(myInput.TapPosition);
			mousePos2D = new Vector2 (mousePos.x, mousePos.y);
			hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f);
		}
		else if(myInput.dragStarted){
			mousePos = Camera.main.ScreenToWorldPoint(myInput.startDragTouch);
			mousePos2D = new Vector2 (mousePos.x, mousePos.y);
			hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f);			
		}
		else if(myInput.dragReleased){
			mousePos = Camera.main.ScreenToWorldPoint(myInput.releaseDragPos);
			mousePos2D = new Vector2 (mousePos.x, mousePos.y);
			hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f);	
		}


		//if (handheldDevice) { if (Input.touchCount > 0) { updateMousePos = Camera.main.ScreenToWorldPoint(Input.touches[0].position); } }
		//if (desktopDevice) { updateMousePos = Camera.main.ScreenToWorldPoint(myInput.TapPosition); }

		// if (connections == connectionsNeeded)
		// {
		// 	Debug.Log("ya win m8!");
		// 	inBetweenLvls = true;
		// 	curntLvl += 1;
		// 	//connectionsNeeded = 1;
		// }


		if (kitePuzzEngineScript.canPlay)
		{
			// --- Click detected check to see what tile is hit
			if (hit.collider != null && myInput.dragStarted && hit.collider.CompareTag("Tile") && hit.collider.GetComponent<TileRotation>().canBeRotated)
			{
				Debug.Log("Click pressed");
				mouseClickOGPos = mousePos;
				timer = 0f;
				// Select tile
				mouseClick = true;
				tileClicked = hit.collider.gameObject;
				tileClickedOGPos = tileClicked.transform.position;
				tileClicked.GetComponent<BoxCollider2D>().enabled = false;

				// SFX PICK UP TILE
				if(mouseClick) { audioSceneParkPuz.pickupTile(); }

				//StartCoroutine(SkipAFrame());
			
			}

			// --- Check to see if holding click
			if (myInput.isDragging && tileClicked != null)
			{
				// if (mouseClickHeld == false)
				// {	
				// 	timer += Time.deltaTime;
				// }

				// --- Clicked long enough 
				// if (timer >= nowHoldingTime || mouseClickHeld || Vector3.Distance(mouseClickOGPos, updateMousePos) > distToDrag) 
				// {
					//Debug.Log("Click now held");
					// mouseClickHeld = true;
					// mouseClick = false;
					// timer = 0f;
					mousePos = Camera.main.ScreenToWorldPoint(myInput.draggingPosition);
					float tileClickedX =  mousePos.x;
					float tileClickedY = mousePos.y;

					tileClicked.transform.position = new Vector3(tileClickedX, tileClickedY, -5f);
				// }
			}


			// --- Click released just a click
			if (myInput.Tapped && tileClicked != null)
			{
				// Debug.Log("Click released after click");
				// if (mouseClick)
				// {
					//tileClicked.transform.eulerAngles = new Vector3(tileClicked.transform.eulerAngles.x, tileClicked.transform.eulerAngles.y, tileClicked.transform.eulerAngles.z - 90);
					// make tile roo roororo tate
					tileClicked.GetComponent<TileRotation>().RotateTile();

					// SFX PICK UP TILE
					audioSceneParkPuz.rotateTile();
				// }

				// tileClicked.transform.position = tileClickedOGPos;

				tileClicked.GetComponent<BoxCollider2D>().enabled = true;
				// mouseClickHeld = false;
				// mouseClick = false;
				tileClicked = null;
				// timer = 0f;
				kitePuzzEngineScript.connections = 0;

				foreach (Transform tile in lvlTiles[kitePuzzEngineScript.curntLvl - 1].transform)
				{
					//Debug.Log("make tiles check neighbors");
					tile.gameObject.GetComponent<TileRotation>().CheckNeighbors();
				}
			}


			// --- Click released after holding
			if (myInput.dragReleased && tileClicked != null)
			{
				// SFK DROP TILE
				audioSceneParkPuz.dropTile();

				if(hit.collider != null && hit.collider.CompareTag("Tile") && hit.collider.GetComponent<TileRotation>().canBeRotated){
					tileClicked.transform.position = hit.collider.gameObject.transform.position;
					hit.collider.gameObject.transform.position = tileClickedOGPos;

				}else{
					tileClicked.transform.position = tileClickedOGPos;
				}
				
				Debug.Log("Click released after held");
				// if (mouseClickHeld && hit.collider.GetComponent<TileRotation>().canBeRotated)
				// {
				// 	tileClicked.transform.position = hit.collider.gameObject.transform.position;
				// 	hit.collider.gameObject.transform.position = tileClickedOGPos;
				// }
				// else
				// {
				// 	tileClicked.transform.position = tileClickedOGPos;
				// }

				tileClicked.GetComponent<BoxCollider2D>().enabled = true;
				// mouseClickHeld = false;
				// mouseClick = false;
				tileClicked = null;
				// timer = 0f;
				kitePuzzEngineScript.connections = 0;

				foreach (Transform tile in lvlTiles[kitePuzzEngineScript.curntLvl - 1].transform)
				{
					Debug.Log("make tiles check neighbors");
					tile.gameObject.GetComponent<TileRotation>().CheckNeighbors();
				}
			}
		}
	}

	public int CalculateConnectionsNeeded()
	{
		connectionsNeeded = 0;

		foreach(Transform tile in lvlTiles[kitePuzzEngineScript.curntLvl - 1].transform)
		{
			//Debug.Log("Counter");
			if (tile.GetComponent<TileRotation>().topConnection == true) { connectionsNeeded += 1; }
			if (tile.GetComponent<TileRotation>().rightConnection == true) { connectionsNeeded += 1; }
			if (tile.GetComponent<TileRotation>().bottomConnection == true) { connectionsNeeded += 1; }
			if (tile.GetComponent<TileRotation>().leftConnection == true) { connectionsNeeded += 1; }
		}
		return connectionsNeeded;
	}
	// public IEnumerator SkipAFrame()
	// {
	// 	yield return null;
	// }
}
