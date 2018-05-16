using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CrabRiddle : MonoBehaviour 
{
	Ray2D ray;
	RaycastHit2D hit;
	Vector2 mousePos2D;
	Vector3 mousePos;

	[Header("Crab Riddle")]
	public int moveAmount;
	public List<GameObject> moves;
	public GameObject crab;
	public Animator crabAnim;
	public Vector3 moveDest;
	public float crabSpeed;
	public Vector3 crabOGPos;
	public bool crabReturning = false;

	public GameObject goldenEgg;
    public LayerMask layerMask;
	
	public bool canClick = true;

	// public bool desktopDevice = false;
	// public bool handheldDevice = false;

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

		if (GlobalVariables.globVarScript.crabRiddleSolved == true)
		{
			foreach (GameObject move in moves)
			{
				move.GetComponent<BoxCollider2D>().enabled = false;
			}
			goldenEgg.SetActive(true);
		}

		moveDest = crab.transform.position;
		crabOGPos = crab.transform.position;
	}
	


	void FixedUpdate () 
	{
		// if (desktopDevice)
		// {
			mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			mousePos2D = new Vector2 (mousePos.x, mousePos.y);

			hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f, layerMask);
		//}
	}


	void Update ()
    { 
		if (Vector2.Distance(crab.transform.position, moveDest) > 0.05f)
		{
			crabAnim.SetBool("PlayCrabWalk", true);
			canClick = false;
		}
		else
		{
			crab.transform.position = moveDest;
			crabAnim.SetBool("PlayCrabWalk", false);
			if(crabReturning)
			{
				crabAnim.SetTrigger("PlayCrabClaws");
			}
			canClick = true;
			crabReturning = false;
		}

		crab.transform.position = Vector3.MoveTowards(crab.transform.position, moveDest, crabSpeed * Time.deltaTime);

		// if (desktopDevice)
		// {
			if (hit)
			{
				if (hit.collider.CompareTag("FruitBasket") && Input.GetMouseButtonDown(0) && canClick)
				{
					moveAmount += 1;

					moveDest = (moves[moveAmount-1].transform.position - crab.transform.position).normalized + crab.transform.position;
					//StartCoroutine(CrabMove());
					

					hit.collider.gameObject.GetComponent<BoxCollider2D>().enabled = false;
					
					if (moveAmount >= 5)
					{
						CrabRiddleSolved ();
						//SpawnGoldenEgg;
						goldenEgg.SetActive(true);

						if (!fireworksFired)
						{
							firework01.Play(true);
							firework02.Play(true);
							fireworksFired = true;
						}
						//Disable/destroy all basket colliders;
						foreach (GameObject move in moves)
						{
							move.SetActive(false);
						}	
						return;
					}
					else
					{
						moves[moveAmount].GetComponent<BoxCollider2D>().enabled = true;
					}
				}
				
				// - Player clicks anywhere else - //
				if (Input.GetMouseButtonDown(0) && !hit.collider.CompareTag("FruitBasket") && !goldenEgg.activeSelf && canClick)
				{
					if (moveAmount > 0)
					{
						crabReturning = true;
					}
					moveAmount = 0;
					moveDest = crabOGPos;
					foreach (GameObject move in moves)
					{
						move.GetComponent<BoxCollider2D>().enabled = false;
					}	
					moves[0].GetComponent<BoxCollider2D>().enabled = true;
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



	// IEnumerator CrabMove()
	// {
	// 	while (crab.transform.position != moveDest)
	// 	{
	// 		Debug.Log("Should move towards");
	// 		canClick = false;
	// 		crab.transform.position = Vector3.MoveTowards(crab.transform.position, moveDest, crabSpeed * Time.deltaTime);
	// 		yield return null;
	// 	}
	// 	canClick = true;
	// }



	public void CrabRiddleSolved ()
	{
		GlobalVariables.globVarScript.crabRiddleSolved = true;
		GlobalVariables.globVarScript.SaveEggState();
	}

}