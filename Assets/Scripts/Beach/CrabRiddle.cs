using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CrabRiddle : MonoBehaviour {
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
	private Vector3 crabPos, crabPosDest;
	public float crabMoveAmnt;
	public inputDetector inputDetScript;
	public AudioSceneBeach audioSceneBeachScript;
	public SceneTapEnabler sceneTapEnabScript;
	public ClickOnEggs clickOnEggsScript;

	void Start () {
		if (GlobalVariables.globVarScript.riddleSolved == true) {
			foreach (GameObject move in moves)
			{
				move.SetActive(false);
			}
			goldenEgg.SetActive(true);
		}

		moveDest = crab.transform.position;
		crabOGPos = crab.transform.position;
		audioSceneBeachScript = GameObject.Find("Audio").GetComponent<AudioSceneBeach>();
	}


	void Update () { 
		if (Vector2.Distance(crab.transform.position, moveDest) > 0.05f) {
			crabAnim.SetBool("PlayCrabWalk", true);
			canClick = false;
		}
		else {
			crab.transform.position = moveDest;
			crabAnim.SetBool("PlayCrabWalk", false);
			if(crabReturning) {
				crabAnim.SetTrigger("PlayCrabClaws");
				audioSceneBeachScript.crabClawsSFX();
			}
			canClick = true;
			crabReturning = false;
		}

		crab.transform.position = Vector3.MoveTowards(crab.transform.position, moveDest, crabSpeed * Time.deltaTime);

		if (sceneTapEnabScript.canTapEggRidPanPuz && inputDetScript.Tapped) {
			UpdateMousePos ();
			hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f, layerMask);
			if (hit) {
				if (hit.collider.CompareTag("FruitBasket") && canClick)	{
					if (hit.collider.GetComponent<CrabRiddleTapObjects>().left == directions[moveAmount]) {
						audioSceneBeachScript.crabWalkSFX();
						//moveDest = (moves[moveAmount-1].transform.position - crab.transform.position).normalized + crab.transform.position;
						if (directions[moveAmount]) {
							moveDest = new Vector3(crab.transform.position.x - crabMoveAmnt, crab.transform.position.y, crab.transform.position.z);
						}
						else {
							moveDest = new Vector3(crab.transform.position.x + crabMoveAmnt, crab.transform.position.y, crab.transform.position.z);
						}
						moveAmount += 1;
						//hit.collider.gameObject.GetComponent<BoxCollider2D>().enabled = false;
						if (moveAmount >= movesToWin) {
							CrabRiddleSolved ();
							// Activate the Golden Egg sequence.
							goldenEgg.SetActive(true);
							goldenEggScript.waitingToStartSeq = true;
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
							audioSceneBeachScript.crabWalkSFX();
						}
						moveAmount = 0;
						moveDest = crabOGPos;
						//audioSceneBeachScript.crabWalkSFX();
					}
				}
				
				// - Player clicks anywhere else - //
				if (!hit.collider.CompareTag("FruitBasket") && canClick) {
					if (moveAmount > 0) {
						crabReturning = true;
						audioSceneBeachScript.crabWalkSFX();
					}
					moveAmount = 0;
					moveDest = crabOGPos;
					//audioSceneBeachScript.crabWalkSFX();
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

	public void CrabRiddleSolved () {
		if (clickOnEggsScript.goldenEggFound == 0) {
			clickOnEggsScript.goldenEggFound = 1;
			clickOnEggsScript.AddEggsFound();
		}
		GlobalVariables.globVarScript.riddleSolved = true;
		GlobalVariables.globVarScript.SaveEggState();
	}

}