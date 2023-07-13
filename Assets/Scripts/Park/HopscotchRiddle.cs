using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class HopscotchRiddle : MonoBehaviour 
{
	Ray2D ray;
	RaycastHit2D hit, hit2;
	Vector2 mousePos2D, mousePos2D2;
	Vector3 mousePos, mousePos2;

	[Header("Hopscotch Riddle")]
	public inputDetector myInput;
	public List<GameObject> numbers;
	public int numberAmount;
	public HopscotchCell numberOne;
	public HopscotchCell[] allCells;
	public GameObject goldenEgg;
	public GoldenEgg goldenEggScript;
    public LayerMask layerMask;
	// public bool desktopDevice = false;
	// public bool handheldDevice = false;
	public ParticleSystem hopscotchFX;
	public bool touchOne, touchTwo, startMinSecTimer;
	public float minSecTapTime, minSecTapTimer;
	//public LevelTapMannager lvlTapManScript;
	public SceneTapEnabler sceneTapEnaScript;
	public ClickOnEggs clickOnEggsScript;
	public inputDetector inputDetScript;
	public AudioScenePark audioSceneParkScript;

	void Start () {
		// if (SystemInfo.deviceType == DeviceType.Handheld)
		// {
		// 	handheldDevice = true;
		// }
		// else if (SystemInfo.deviceType == DeviceType.Desktop)
		// {
		// 	desktopDevice = true;
		// }

		if (GlobalVariables.globVarScript.riddleSolved == true)	{
			foreach (GameObject number in numbers)
			{
				number.SetActive(false);
			}
			goldenEgg.SetActive(true);
		}
	}

	void Update () {
		// if (desktopDevice)
		// {
			// if (!GlobalVariables.globVarScript.hopscotchRiddleSolved && (myInput.Tapped || myInput.DoubleTouched) )
			// {
			// 	mousePos = Camera.main.ScreenToWorldPoint(myInput.TapPosition);
			// 	if(myInput.DoubleTouched){
			// 		mousePos = Camera.main.ScreenToWorldPoint(myInput.touchOne);
			// 		mousePos2 = Camera.main.ScreenToWorldPoint(myInput.touchTwo);
			// 		mousePos2D2 = new Vector2 (mousePos2.x, mousePos2.y);
			// 		hit2 = Physics2D.Raycast(mousePos2D2, Vector3.forward, 50f, layerMask);
			// 	}
			// 	mousePos2D = new Vector2 (mousePos.x, mousePos.y);

			// 	hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f, layerMask);

			if (startMinSecTimer) {
				minSecTapTimer-=Time.deltaTime;
				if (minSecTapTimer <= 0) {
					startMinSecTimer = false;
					minSecTapTimer = minSecTapTime;
					touchOne = false;
					foreach ( HopscotchCell myCells in allCells)
					{
						myCells.ResetCell();
					}
					//reset riddle
				}
			}

			if (touchOne && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) {
				startMinSecTimer = false;
				minSecTapTimer = minSecTapTime;
				touchOne = false;
				//if ()
				//foreach ( HopscotchCell myCells in allCells)
				//{
				//	myCells.ResetCell();
				//}
			}

			//if (Input.touchCount == 1)
			//{
			//	touchOne = true;
			//}

			if (!GlobalVariables.globVarScript.riddleSolved && myInput.Tapped && sceneTapEnaScript.canTapEggRidPanPuz/* Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began */) {
				mousePos = Camera.main.ScreenToWorldPoint( myInput.TapPosition/* Input.GetTouch(0).position */);
				mousePos2D = new Vector2 (mousePos.x, mousePos.y);
				hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f, layerMask);
				//Debug.Log("SHOT FIRST RAYCAST AND HIT");
				if (hit) {
					if (hit.collider.CompareTag("OnClickFX")) {
						hopscotchFX.gameObject.transform.position = mousePos2D;
						hopscotchFX.Play();
						//SFX hit number of Hopscotch puzzle
						audioSceneParkScript.goldenEggGameSFX();
					}

					if (hit.collider.CompareTag("FruitBasket")) {
						hopscotchFX.gameObject.transform.position = mousePos2D;
						hopscotchFX.Play();
						// SFX hit number of Hopscotch puzzle.
						audioSceneParkScript.goldenEggGameSFX();
						HopscotchCell currentCell = hit.collider.gameObject.GetComponent<HopscotchCell>();
						//Debug.Log("THIS IS THE FIRST TAPPED CELL" + currentCell);
						if (currentCell.doubleCell) { touchOne = true; startMinSecTimer = true; }
						//numberAmount += 1;
						//hit.collider.gameObject.GetComponent<CircleCollider2D>().enabled = false;
						if(currentCell.myNumber == numberOne.myNumber) { // If its One reset all cells.
							foreach ( HopscotchCell myCells in allCells)
							{
								myCells.ResetCell();
							}
						}
						currentCell.checkCell(); // If its One it will set its own collider true, if not One col false, and next ones true.
						inputDetScript.ResetDoubleTap();
						
						if (currentCell.goalCell) { // Tapped last cell.
							clickOnEggsScript.RiddleSolved();
							// Activate the Golden Egg sequence.
							goldenEgg.SetActive(true);
							goldenEggScript.waitingToStartSeq = true;
							goldenEggScript.CannotTaps();
							//Disable/destroy all basket colliders;
							foreach (GameObject number in numbers)
							{
								number.SetActive(false);
							}	
							return;
						}
					}

					if (!hit.collider.CompareTag("FruitBasket") && !GlobalVariables.globVarScript.riddleSolved) { // Tapped not on the good riddle number or anywhere else, reset all cells.
						foreach ( HopscotchCell myCells in allCells)
						{
							myCells.ResetCell();

						}
					}
				}
			}
    }

	public void HopscotchRiddleSolved () {
		// if (clickOnEggsScript.goldenEggFound == 0) {
		// 	clickOnEggsScript.goldenEggFound = 1;
		// 	clickOnEggsScript.AddEggsFound();
		// }
		GlobalVariables.globVarScript.riddleSolved = true;
		GlobalVariables.globVarScript.SaveEggState();
		// Activate the Golden Egg sequence.
		QueueSequenceManager.AddSequenceToQueue(goldenEggScript.StartGoldenEggSequence);
	}
}
