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
							HopscotchRiddleSolved();
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
							//startMinSecTimer = false;
							//minSecTapTimer = minSecTapTime;
							//touchOne = false;

						}
					}
				}
			}

			// if (!GlobalVariables.globVarScript.hopscotchRiddleSolved && touchOne && Input.touchCount > 1 && Input.GetTouch(1).phase == TouchPhase.Began && startMinSecTimer)
			// {
			// 	mousePos2 = Camera.main.ScreenToWorldPoint(Input.GetTouch(1).position);
			// 	mousePos2D2 = new Vector2 (mousePos2.x, mousePos2.y);
			// 	hit2 = Physics2D.Raycast(mousePos2D2, Vector3.forward, 50f, layerMask);
				
			// 	if (hit2)
			// 	{
			// 		Debug.Log("SHOT SECOND RAYCAST AND HIT2");
			// 		if (hit2.collider.CompareTag("OnClickFX"))
			// 		{
			// 			hopscotchFX.gameObject.transform.position = mousePos2D2;
			// 			hopscotchFX.Play();
			// 			//SFX hit number of Hopscotch puzzle
			// 			audioSceneParkScript.goldenEggGameSFX();
			// 		}

			// 		if (hit.collider.CompareTag("FruitBasket"))
			// 		{
						
			// 			hopscotchFX.gameObject.transform.position = mousePos2D;
			// 			hopscotchFX.Play();
			// 			//SFX hit number of Hopscotch puzzle
			// 			audioSceneParkScript.goldenEggGameSFX();

			// 			HopscotchCell currentCell = hit2.collider.gameObject.GetComponent<HopscotchCell>();
			// 			Debug.Log("THIS IS THE SECOND TAPPED CELL" + currentCell);

			// 			if(currentCell.myNumber == numberOne.myNumber){                                  // if its One reset all
			// 				foreach ( HopscotchCell myCells in allCells)
			// 				{
			// 					myCells.ResetCell();
			// 					startMinSecTimer = false;
			// 					minSecTapTimer = minSecTapTime;
			// 					touchOne = false;
			// 				}
			// 			}
			// 			currentCell.checkCell();                                  // if its One it will set its own collider true Else false and next ones true. Unless double		
						
			// 			if (currentCell.goalCell)                                  // tapped last cell
			// 			{
			// 				HopscotchRiddleSolved ();
			// 				//SpawnGoldenEgg;
			// 				goldenEgg.SetActive(true);
			// 				goldenEggScript.inGoldenEggSequence = true;

			// 				if (!fireworksFired)
			// 				{
			// 					firework01.Play(true);
			// 					firework02.Play(true);
			// 					fireworksFired = true;
			// 				}
			// 				//Disable/destroy all basket colliders;
			// 				foreach (GameObject number in numbers)
			// 				{
			// 					number.SetActive(false);
			// 				}	
			// 				return;
			// 			}
			// 		}

					// if (!hit.collider.CompareTag("FruitBasket") && !GlobalVariables.globVarScript.hopscotchRiddleSolved)        // tapped not on the good riddle numb or anywhere else reset all cells  
					// {
					// 	foreach ( HopscotchCell myCells in allCells)
					// 	{
					// 		myCells.ResetCell();
					// 		startMinSecTimer = false;
					// 		minSecTapTimer = minSecTapTime;
					// 		touchOne = false;
					// 	}
					// }
				// }
			//}


				// if (hit2)
				// {
				// 	if (hit2.collider.CompareTag("OnClickFX"))
				// 	{
				// 		hopscotchFX.gameObject.transform.position = mousePos2D;
				// 		hopscotchFX.Play();
				// 		//SFX hit number of Hopscotch puzzle
				// 		audioSceneParkScript.goldenEggGameSFX();
				// 	}

				// 	if (hit2.collider.CompareTag("FruitBasket"))
				// 	{
				// 		hopscotchFX.gameObject.transform.position = mousePos2D;
				// 		hopscotchFX.Play();
				// 		HopscotchCell currentCell = hit2.collider.gameObject.GetComponent<HopscotchCell>();

				// 		//SFX hit number of Hopscotch puzzle
				// 		audioSceneParkScript.goldenEggGameSFX();

				// 		//numberAmount += 1;

				// 		//hit.collider.gameObject.GetComponent<CircleCollider2D>().enabled = false;
				// 		if(currentCell.myNumber == numberOne.myNumber){
				// 			foreach ( HopscotchCell myCells in allCells)
				// 			{
				// 				myCells.ResetCell();
				// 			}
				// 		}
				// 		currentCell.checkCell();
						
						
				// 		if (currentCell.goalCell)
				// 		{
				// 			HopscotchRiddleSolved ();
				// 			//SpawnGoldenEgg;
				// 			goldenEgg.SetActive(true);
				// 			goldenEggScript.inGoldenEggSequence = true;

				// 			if (!fireworksFired)
				// 			{
				// 				firework01.Play(true);
				// 				firework02.Play(true);
				// 				fireworksFired = true;
				// 			}
				// 			//Disable/destroy all basket colliders;
				// 			foreach (GameObject number in numbers)
				// 			{
				// 				number.SetActive(false);
				// 			}	
				// 			return;
				// 		}
				// 	}

				// 	if (!hit2.collider.CompareTag("FruitBasket") && !GlobalVariables.globVarScript.hopscotchRiddleSolved)
				// 	{
				// 		foreach ( HopscotchCell myCells in allCells)
				// 		{
				// 			myCells.ResetCell();
				// 		}
				// 	}
				// }
		//}

		// if (handheldDevice)
		// {
		// 	Touch myTouch = Input.GetTouch(0);
			
		// 	Touch[] myTouches = Input.touches;

		// 	for (int i = 0; i < Input.touchCount; i++)
		// 	{
		// 		// If one of my touches touches 2 and the other touches 3
		// 	}
		// }  
    }

	public void HopscotchRiddleSolved () {
		if (clickOnEggsScript.goldenEggFound == 0) {
			clickOnEggsScript.goldenEggFound = 1;
			clickOnEggsScript.AddEggsFound();
		}
		GlobalVariables.globVarScript.riddleSolved = true;
		GlobalVariables.globVarScript.SaveEggState();
	}
}
