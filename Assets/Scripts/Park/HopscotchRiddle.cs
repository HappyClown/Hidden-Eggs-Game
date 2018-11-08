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

	public ParticleSystem firework01; 
	public ParticleSystem firework02;

	public bool fireworksFired;

	public AudioScenePark audioSceneParkScript;



	void Start () 
	{
		// if (SystemInfo.deviceType == DeviceType.Handheld)
		// {
		// 	handheldDevice = true;
		// }
		// else if (SystemInfo.deviceType == DeviceType.Desktop)
		// {
		// 	desktopDevice = true;
		// }

		if (GlobalVariables.globVarScript.hopscotchRiddleSolved == true)
		{
			foreach (GameObject number in numbers)
			{
				number.SetActive(false);
			}
			goldenEgg.SetActive(true);
		}
	}
	


	void FixedUpdate () 
	{
		// if (desktopDevice)
		// {
			
		//}
	}



	void Update ()
    { 
		// if (desktopDevice)
		// {
			if (!GlobalVariables.globVarScript.hopscotchRiddleSolved && (myInput.Tapped || myInput.DoubleTouched) )
			{
				mousePos = Camera.main.ScreenToWorldPoint(myInput.TapPosition);
				if(myInput.DoubleTouched){
					mousePos = Camera.main.ScreenToWorldPoint(myInput.touchOne);
					mousePos2 = Camera.main.ScreenToWorldPoint(myInput.touchTwo);
					mousePos2D2 = new Vector2 (mousePos2.x, mousePos2.y);
					hit2 = Physics2D.Raycast(mousePos2D2, Vector3.forward, 50f, layerMask);
				}
				mousePos2D = new Vector2 (mousePos.x, mousePos.y);

				hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f, layerMask);
				
				if (hit)
				{
					if (hit.collider.CompareTag("OnClickFX"))
					{
						hopscotchFX.gameObject.transform.position = mousePos2D;
						hopscotchFX.Play();
						//SFX hit number of Hopscotch puzzle
						audioSceneParkScript.goldenEggGameSFX();
					}

					if (hit.collider.CompareTag("FruitBasket"))
					{
						hopscotchFX.gameObject.transform.position = mousePos2D;
						hopscotchFX.Play();
						HopscotchCell currentCell = hit.collider.gameObject.GetComponent<HopscotchCell>();

						//SFX hit number of Hopscotch puzzle
						audioSceneParkScript.goldenEggGameSFX();

						//numberAmount += 1;

						//hit.collider.gameObject.GetComponent<CircleCollider2D>().enabled = false;
						if(currentCell.myNumber == numberOne.myNumber){
							foreach ( HopscotchCell myCells in allCells)
							{
								myCells.ResetCell();
							}
						}
						currentCell.checkCell();
						
						
						if (currentCell.goalCell)
						{
							HopscotchRiddleSolved ();
							//SpawnGoldenEgg;
							goldenEgg.SetActive(true);
							goldenEggScript.inGoldenEggSequence = true;

							if (!fireworksFired)
							{
								firework01.Play(true);
								firework02.Play(true);
								fireworksFired = true;
							}
							//Disable/destroy all basket colliders;
							foreach (GameObject number in numbers)
							{
								number.SetActive(false);
							}	
							return;
						}
					}

					if (!hit.collider.CompareTag("FruitBasket") && !GlobalVariables.globVarScript.hopscotchRiddleSolved)
					{
						foreach ( HopscotchCell myCells in allCells)
						{
							myCells.ResetCell();
						}
					}
				}
				if (hit2)
				{
					if (hit2.collider.CompareTag("OnClickFX"))
					{
						hopscotchFX.gameObject.transform.position = mousePos2D;
						hopscotchFX.Play();
						//SFX hit number of Hopscotch puzzle
						audioSceneParkScript.goldenEggGameSFX();
					}

					if (hit2.collider.CompareTag("FruitBasket"))
					{
						hopscotchFX.gameObject.transform.position = mousePos2D;
						hopscotchFX.Play();
						HopscotchCell currentCell = hit2.collider.gameObject.GetComponent<HopscotchCell>();

						//SFX hit number of Hopscotch puzzle
						audioSceneParkScript.goldenEggGameSFX();

						//numberAmount += 1;

						//hit.collider.gameObject.GetComponent<CircleCollider2D>().enabled = false;
						if(currentCell.myNumber == numberOne.myNumber){
							foreach ( HopscotchCell myCells in allCells)
							{
								myCells.ResetCell();
							}
						}
						currentCell.checkCell();
						
						
						if (currentCell.goalCell)
						{
							HopscotchRiddleSolved ();
							//SpawnGoldenEgg;
							goldenEgg.SetActive(true);
							goldenEggScript.inGoldenEggSequence = true;

							if (!fireworksFired)
							{
								firework01.Play(true);
								firework02.Play(true);
								fireworksFired = true;
							}
							//Disable/destroy all basket colliders;
							foreach (GameObject number in numbers)
							{
								number.SetActive(false);
							}	
							return;
						}
					}

					if (!hit2.collider.CompareTag("FruitBasket") && !GlobalVariables.globVarScript.hopscotchRiddleSolved)
					{
						foreach ( HopscotchCell myCells in allCells)
						{
							myCells.ResetCell();
						}
					}
				}
			}
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



	public void HopscotchRiddleSolved ()
	{
		GlobalVariables.globVarScript.hopscotchRiddleSolved = true;
		GlobalVariables.globVarScript.SaveEggState();
	}
}
