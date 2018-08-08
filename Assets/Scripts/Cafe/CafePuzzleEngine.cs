using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CafePuzzleEngine : MonoBehaviour {

	// Use this for initialization
	public SwipeDetector mySwipeControls;
	private RaycastHit2D hit;
	public CafePuzzleCell[] gridCells;
	public GameObject[] mylevels;
	public CafePuzzleLevel mylvl;
	public int currentLevel;
	private bool raycastDone;
	void Start () {
		raycastDone = false;
		for (int i = 0; i < mylevels.Length; i++)
		{
			if(i == currentLevel){
				mylevels[i].SetActive(true);
				mylvl = mylevels[i].GetComponent<CafePuzzleLevel>();
				CleanGrid();
				mylvl.SetUp();
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(!mylvl.movigCups){
			if(mySwipeControls.SwipeLeft || mySwipeControls.SwipeRight || mySwipeControls.SwipeUp || mySwipeControls.SwipeDown){
				Vector2 touchPosition = Camera.main.ScreenToWorldPoint(mySwipeControls.FirstTouch);
				hit = Physics2D.Raycast(touchPosition, Vector3.forward, 50f);
				if (hit)
				{
					if (hit.collider.CompareTag("Tile"))
					{
						Debug.DrawRay(touchPosition, Vector3.forward, Color.red, 60f);
						string color = hit.collider.gameObject.GetComponent<CafePuzzleCup>().myColor.ToString();

						if(mySwipeControls.SwipeLeft)
						{
							mylvl.Swipe(color,"left");
						}
						if(mySwipeControls.SwipeRight)
						{
							mylvl.Swipe(color,"right");							
						}
						if(mySwipeControls.SwipeUp)
						{
							mylvl.Swipe(color,"up");							
						}
						if(mySwipeControls.SwipeDown)
						{
							mylvl.Swipe(color,"down");							
						}
					}
				}
			}
			else{
				raycastDone = false;
			}

		}
		if(Input.GetKey("r")){
			ResetLevel();
		}
	}

	void CleanGrid(){
		for (int i = 0; i < gridCells.Length; i++)
		{
			gridCells[i].occupied = false;
		}
	}
	void ResetLevel(){
		CleanGrid();
		mylvl.SetUp();
	}
		
}
