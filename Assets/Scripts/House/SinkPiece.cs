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
	private Vector3 initialPos;
	public PuzzleCell StartingCell, currentCell, nextCell;
	public bool active, selected, matched, falling, changeCell,showConnections;
	public float minDistance, gravity, speed, maxFallingSpeed;
	// Use this for initialization
	void Start () {
		initialPos = this.gameObject.transform.position;
		currentCell = StartingCell;
		currentCell.occupied = falling =  true;
		changeCell = active = matched = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(active){
			if(changeCell){
				falling = true;
				currentCell.occupied = false;
				currentCell = nextCell;
				currentCell.occupied = true;
			}
			if(falling){
				if(this.transform.position.y < (currentCell.gameObject.transform.position.y + minDistance)){
					this.transform.position = currentCell.gameObject.transform.position;
					falling = false;
					speed = 0;
				}
				else{
					speed += gravity*Time.deltaTime;
					if(speed > maxFallingSpeed){
						speed = maxFallingSpeed;
					}
					this.transform.position = Vector2.MoveTowards(this.transform.position,currentCell.gameObject.transform.position,speed);
				}
			}
			CheckCell();
		}
	}
	public void CheckCell(){
		if(!currentCell.edgeDown){
			if(!currentCell.cellDown.occupied){
				nextCell = currentCell.CheckDown();
				changeCell = true;
			}
		}
	}
	public void ResetCell(){
		this.transform.position = initialPos;
		currentCell = StartingCell;
		currentCell.occupied = falling =  true;
		changeCell = active = matched = false;
	}
}
