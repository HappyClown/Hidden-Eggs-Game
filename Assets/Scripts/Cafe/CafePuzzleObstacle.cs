using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CafePuzzleObstacle : MonoBehaviour {

	public CafePuzzleCell myCell;
	// Use this for initialization
	public void SetUp () {
		myCell.occupied = true;
		Vector2 startingPos = new Vector3(myCell.gameObject.transform.position.x, myCell.transform.position.y);
		transform.position = startingPos;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
