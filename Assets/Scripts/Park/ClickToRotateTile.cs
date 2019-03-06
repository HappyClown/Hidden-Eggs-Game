using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickToRotateTile : MonoBehaviour {
	RaycastHit2D hit;
	Vector2 mousePos2D;
	Vector3 mousePos;
	[Header("Mouse/Finger Stuff")]
	public bool mouseClick;
	public Vector3 mouseClickOGPos;
	private bool smallReset;
	[Header("Tile Stuff")]
	public GameObject tileClicked;
	public Vector3 tileClickedOGPos;
	public int connectionsNeeded;
	public List<GameObject> lvlTiles;
	private bool desktopDevice = false, handheldDevice = false;
	[Header("Scripts")]
	public inputDetector myInput;
	public KitePuzzEngine kitePuzzEngineScript;
	public LeafTurnPooler LeafTurnPoolScript;
	public List<ParticleSystem> leafBurstFXs;
	public AudioSceneParkPuzzle audioSceneParkPuz;

	void Awake () {
		if (connectionsNeeded == 0) { 
			connectionsNeeded = 99; 
		}
	}

	void Start () {
		if (SystemInfo.deviceType == DeviceType.Handheld) {
			handheldDevice = true;
		}
		else if (SystemInfo.deviceType == DeviceType.Desktop) {
			desktopDevice = true;
		}
	}	

	void Update () {
		if (myInput.Tapped) {
			mousePos = Camera.main.ScreenToWorldPoint(myInput.TapPosition);
			mousePos2D = new Vector2 (mousePos.x, mousePos.y);
			hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f);
		}
		else if (myInput.dragStarted) {
			mousePos = Camera.main.ScreenToWorldPoint(myInput.startDragTouch);
			mousePos2D = new Vector2 (mousePos.x, mousePos.y);
			hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f);			
		}
		else if (myInput.dragReleased) {
			mousePos = Camera.main.ScreenToWorldPoint(myInput.releaseDragPos);
			mousePos2D = new Vector2 (mousePos.x, mousePos.y);
			hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f);	
		}

		if (kitePuzzEngineScript.canPlay) {
			// --- Click detected check to see what tile is hit
			if (hit.collider != null && myInput.dragStarted && hit.collider.CompareTag("Tile") && hit.collider.GetComponent<TileRotation>().canBeRotated && !hit.collider.GetComponent<TileRotation>().isEmpty) {
				smallReset = true;
				mouseClickOGPos = mousePos;
				// Select tile
				mouseClick = true;
				tileClicked = hit.collider.gameObject;
				tileClickedOGPos = tileClicked.transform.position;
				tileClicked.GetComponent<BoxCollider2D>().enabled = false;

				// SFX PICK UP TILE
				if(mouseClick) { audioSceneParkPuz.pickupTile(); }
			}

			if (tileClicked != null) {
				// --- Check to see if holding click
				if (myInput.isDragging) {
					mousePos = Camera.main.ScreenToWorldPoint(myInput.draggingPosition);
					float tileClickedX =  mousePos.x;
					float tileClickedY = mousePos.y;

					tileClicked.transform.position = new Vector3(tileClickedX, tileClickedY, -5f);
				}

				// --- Click released just a click
				if (myInput.Tapped) {

					TileRotation tileRotScript = tileClicked.GetComponent<TileRotation>();
					tileRotScript.RotateTile();
					tileRotScript.movedTile = true;
					PlayTurnFX();

					// SFX PICK UP TILE
					audioSceneParkPuz.rotateTile();

					tileClicked.GetComponent<BoxCollider2D>().enabled = true;
					tileClicked = null;
					kitePuzzEngineScript.connections = 0;

					foreach (Transform tile in lvlTiles[kitePuzzEngineScript.curntLvl - 1].transform)
					{
						GameObject tileGO = tile.gameObject;
						tileGO.GetComponent<TileRotation>().CheckNeighbors();
						tileGO.GetComponent<BoxCollider2D>().enabled = true;
					}
				}

				// --- Click released after holding
				if (myInput.dragReleased) {
					// SFK DROP TILE
					audioSceneParkPuz.dropTile();

					TileRotation tileRotScript = tileClicked.GetComponent<TileRotation>();
					tileRotScript.movedTile = true;

					if (hit.collider != null && hit.collider.CompareTag("Tile") && hit.collider.GetComponent<TileRotation>().canBeRotated) {
						tileClicked.transform.position = hit.collider.gameObject.transform.position;
						hit.collider.gameObject.transform.position = tileClickedOGPos;
						PlayDropFX();
					} 
					else {
						tileClicked.transform.position = tileClickedOGPos;
						PlayDropFX();
					}

					tileClicked.GetComponent<BoxCollider2D>().enabled = true;
					tileClicked = null;
					kitePuzzEngineScript.connections = 0;

					foreach (Transform tile in lvlTiles[kitePuzzEngineScript.curntLvl - 1].transform)
					{
						GameObject tileGO = tile.gameObject;
						tileGO.GetComponent<TileRotation>().CheckNeighbors();
						tileGO.GetComponent<BoxCollider2D>().enabled = true;
					}
				}
			}
		}
		if(handheldDevice && Input.touchCount <= 0 && smallReset) {
			tileClicked = null;
			foreach (Transform tile in lvlTiles[kitePuzzEngineScript.curntLvl - 1].transform)
			{
				GameObject tileGO = tile.gameObject;
				//tileGO.GetComponent<TileRotation>().CheckNeighbors();
				tileGO.GetComponent<BoxCollider2D>().enabled = true;
			}
			smallReset = false;
		}
	}

	public int CalculateConnectionsNeeded() {
		connectionsNeeded = 0;
		foreach(Transform tile in lvlTiles[kitePuzzEngineScript.curntLvl - 1].transform)
		{
			if (tile.GetComponent<TileRotation>().topConnection == true) { connectionsNeeded += 1; }
			if (tile.GetComponent<TileRotation>().rightConnection == true) { connectionsNeeded += 1; }
			if (tile.GetComponent<TileRotation>().bottomConnection == true) { connectionsNeeded += 1; }
			if (tile.GetComponent<TileRotation>().leftConnection == true) { connectionsNeeded += 1; }
		}
		return connectionsNeeded;
	}

	void PlayDropFX() {
		foreach(ParticleSystem leafBurst in leafBurstFXs)
		{
			leafBurst.gameObject.transform.position = tileClicked.transform.position;
			leafBurst.Play(true);
		}
	}

	void PlayTurnFX() {
		LeafTurnPoolScript.PlayFXFromPool();
	}
}