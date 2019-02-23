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
	public int movesToWin;
	public List<GameObject> moves;
	public GameObject crab;
	public Animator crabAnim;
	public Vector3 moveDest;
	public float crabSpeed;
	public Vector3 crabOGPos;
	public bool crabReturning = false;

	public GameObject goldenEgg;
	public GoldenEgg goldenEggScript;

    public LayerMask layerMask;
	
	public bool canClick = true;
	public List<bool> directions;

	// public bool desktopDevice = false;
	// public bool handheldDevice = false;

	public ParticleSystem firework01; 
	public ParticleSystem firework02;
	public bool fireworksFired;
	private Vector3 crabPos, crabPosDest;
	public float crabMoveAmnt;
	public inputDetector inputDetScript;



	void Start () 
	{
		if (GlobalVariables.globVarScript.riddleSolved == true)
		{
			foreach (GameObject move in moves)
			{
				move.SetActive(false);
			}
			goldenEgg.SetActive(true);
		}

		moveDest = crab.transform.position;
		crabOGPos = crab.transform.position;
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

		if (inputDetScript.Tapped) {
			UpdateMousePos ();
			hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f, layerMask);
			if (hit)
			{
				if (hit.collider.CompareTag("FruitBasket") && canClick)
				{
					if (hit.collider.GetComponent<CrabRiddleTapObjects>().left == directions[moveAmount]) {
						//moveDest = (moves[moveAmount-1].transform.position - crab.transform.position).normalized + crab.transform.position;
						if (directions[moveAmount]) {
							moveDest = new Vector3(crab.transform.position.x - crabMoveAmnt, crab.transform.position.y, crab.transform.position.z);
						}
						else {
							moveDest = new Vector3(crab.transform.position.x + crabMoveAmnt, crab.transform.position.y, crab.transform.position.z);
						}
						moveAmount += 1;
						//hit.collider.gameObject.GetComponent<BoxCollider2D>().enabled = false;
						if (moveAmount >= movesToWin)
						{
							CrabRiddleSolved ();
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
							foreach (GameObject move in moves)
							{
								move.SetActive(false);
							}	
							return;
						}
					}
					else {
						if (moveAmount > 0) {
							crabReturning = true;
						}
						moveAmount = 0;
						moveDest = crabOGPos;
					}
				}
				
				// - Player clicks anywhere else - //
				if (!hit.collider.CompareTag("FruitBasket") && canClick)
				{
					if (moveAmount > 0)
					{
						crabReturning = true;
					}
					moveAmount = 0;
					moveDest = crabOGPos;
					// foreach (GameObject move in moves)
					// {
					// 	move.GetComponent<BoxCollider2D>().enabled = false;
					// }	
					// moves[0].GetComponent<BoxCollider2D>().enabled = true;
				}
			} 
		}
    }

	void UpdateMousePos () {
		mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos2D = new Vector2 (mousePos.x, mousePos.y);
	}

	public void CrabRiddleSolved ()
	{
		GlobalVariables.globVarScript.riddleSolved = true;
		GlobalVariables.globVarScript.SaveEggState();
	}

}