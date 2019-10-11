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
	public Collider2D crabCollider;
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
			
			crabCollider.enabled = false;
			
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

		if (sceneTapEnabScript.canTapEggRidPanPuz && inputDetScript.Tapped  && canClick) {
			UpdateMousePos ();
			hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f, layerMask);
			if (hit) {
				if (hit.collider.CompareTag("Riddle"))	{
					inputDetScript.ResetDoubleTap();
					if (hit.point.x > crab.transform.position.x && !directions[moveAmount]) {
						audioSceneBeachScript.crabWalkSFX();
						moveDest = new Vector3(crab.transform.position.x + crabMoveAmnt, crab.transform.position.y, crab.transform.position.z);
						moveAmount += 1;
					}
					else if(hit.point.x < crab.transform.position.x && directions[moveAmount]){
						audioSceneBeachScript.crabWalkSFX();
						moveDest = new Vector3(crab.transform.position.x - crabMoveAmnt, crab.transform.position.y, crab.transform.position.z);
						//moveDest = (moves[moveAmount-1].transform.position - crab.transform.position).normalized + crab.transform.position;
						moveAmount += 1;
						//hit.collider.gameObject.GetComponent<BoxCollider2D>().enabled = false;
					}
					else {
						if (moveAmount > 0 && moveAmount < movesToWin) {
							crabReturning = true;
							audioSceneBeachScript.crabWalkSFX();
						}
						if (moveAmount == 0) {
							audioSceneBeachScript.crabClawsSFX();
							crabAnim.SetTrigger("PlayCrabClaws");
						}
						moveAmount = 0;
						moveDest = crabOGPos;
						//audioSceneBeachScript.crabWalkSFX();
					}
					if (moveAmount >= movesToWin) {
						CrabRiddleSolved ();
						// Activate the Golden Egg sequence.
						goldenEgg.SetActive(true);
						goldenEggScript.waitingToStartSeq = true;
						goldenEggScript.CannotTaps();
						//Disable/destroy all basket colliders;
						crabCollider.enabled = false;
							
						return;
					}
				}
				else{
					if (moveAmount > 0 && moveAmount < movesToWin) {
						crabReturning = true;
						audioSceneBeachScript.crabWalkSFX();
						Debug.Log("Crabby should be going back.");
					}
					moveAmount = 0;
					moveDest = crabOGPos;
				}
				// - Player clicks anywhere else - //
				// if (!hit.collider.CompareTag("Riddle")) {
				// 	if (moveAmount > 0) {
				// 		crabReturning = true;
				// 		audioSceneBeachScript.crabWalkSFX();
				// 	}
				// 	moveAmount = 0;
				// 	moveDest = crabOGPos;
					//audioSceneBeachScript.crabWalkSFX();
					// foreach (GameObject move in moves)
					// {
					// 	move.GetComponent<BoxCollider2D>().enabled = false;
					// }	
					// moves[0].GetComponent<BoxCollider2D>().enabled = true;
				//}
			}
			else{
				if (moveAmount > 0 && moveAmount < movesToWin) {
					crabReturning = true;
					audioSceneBeachScript.crabWalkSFX();
					Debug.Log("Crabby should be going back.");
				}
				moveAmount = 0;
				moveDest = crabOGPos;
			}
		}
		else if (sceneTapEnabScript.canTapEggRidPanPuz && inputDetScript.Tapped && !canClick) {
			UpdateMousePos ();
			hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f, layerMask);
			if (hit) {
				if (hit.collider.CompareTag("Riddle"))	{
					inputDetScript.ResetDoubleTap();
					Debug.Log("Hit riddle while crabby was walkin should reset double tap");
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