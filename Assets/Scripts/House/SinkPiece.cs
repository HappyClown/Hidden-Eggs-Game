using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkPiece : MonoBehaviour {
	 public enum pieceTypes
	 {
		 dish,
		 soap,
		 sponge,
		 bubble
	 }
	public pieceTypes pieceType;
	public PuzzleCell StartingCell, currentCell, nextCell;
	public bool selected, falling;
	public float minDistance, gravity, speed;
	// Use this for initialization
	void Start () {
		currentCell = StartingCell;
	}
	
	// Update is called once per frame
	void Update () {
		if(falling){
			if(this.transform.position.y < (currentCell.gameObject.transform.position.y + minDistance)){
				falling = false;
				speed = 0;
			}
			else{
				speed += gravity*Time.deltaTime;
				this.transform.position = Vector2.MoveTowards(this.transform.position,currentCell.gameObject.transform.position,speed);
			}
		}
	}
}
