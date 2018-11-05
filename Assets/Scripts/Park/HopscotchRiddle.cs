﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class HopscotchRiddle : MonoBehaviour 
{
	Ray2D ray;
	RaycastHit2D hit;
	Vector2 mousePos2D;
	Vector3 mousePos;

	[Header("Hopscotch Riddle")]
	public List<GameObject> numbers;
	public int numberAmount;
	public GameObject numberOne;
	public GameObject goldenEgg;
	public GoldenEgg goldenEggScript;

    public LayerMask layerMask;

	// public bool desktopDevice = false;
	// public bool handheldDevice = false;
	public ParticleSystem hopscotchFX;

	public ParticleSystem firework01; 
	public ParticleSystem firework02;

	public bool fireworksFired;



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
			if (!GlobalVariables.globVarScript.hopscotchRiddleSolved && Input.GetMouseButtonDown(0))
			{
				mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				mousePos2D = new Vector2 (mousePos.x, mousePos.y);

				hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f, layerMask);
				
				if (hit)
				{
					if (hit.collider.CompareTag("OnClickFX"))
					{
						hopscotchFX.gameObject.transform.position = mousePos2D;
						hopscotchFX.Play();
					}

					if (hit.collider.CompareTag("FruitBasket"))
					{
						hopscotchFX.gameObject.transform.position = mousePos2D;
						hopscotchFX.Play();

						//numberAmount += 1;

						//hit.collider.gameObject.GetComponent<CircleCollider2D>().enabled = false;

						if (numberAmount == 0 && hit.collider.gameObject == numberOne)
						{
							numberAmount++;
						}
						else if (numberAmount >= 0 && hit.collider.gameObject != numberOne)
						{
							numberAmount++;
						}

						if (hit.collider.gameObject != numberOne)
						{
							hit.collider.gameObject.GetComponent<CircleCollider2D>().enabled = false;
						}
						
						if (numberAmount >= 7)
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
						else
						{
							numbers[numberAmount].GetComponent<CircleCollider2D>().enabled = true;
						}
					}

					if (numberAmount > 1 && hit.collider.gameObject == numberOne)
					{
						numberAmount = 1;
					}

					if (!hit.collider.CompareTag("FruitBasket") && !GlobalVariables.globVarScript.hopscotchRiddleSolved)
					{
						numberAmount = 0;
						foreach (GameObject number in numbers)
						{
							number.GetComponent<CircleCollider2D>().enabled = false;
						}	
						numbers[0].GetComponent<CircleCollider2D>().enabled = true;
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
