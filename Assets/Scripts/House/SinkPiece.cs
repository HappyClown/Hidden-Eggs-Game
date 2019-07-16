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
	private Vector3 initialPos, initialScale;
	public PuzzleCell StartingCell, currentCell, nextCell;
	public AnimationCurve scalingCurve;
	public bool active, selected, matched, falling, changeCell,showConnections;
	public float minDistance, gravity, speed, maxFallingSpeed, maxFallingWaterSpeed, waterYpos;
	// Use this for initialization
	void Awake () {		
		initialScale = this.gameObject.transform.localScale;
		initialPos = this.gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if(active){
			if(changeCell){
				falling = true;
				currentCell.occupied = false;
				currentCell = nextCell;
				currentCell.occupied = true;
				changeCell = false;
			}
			else if(falling){
				if(this.transform.position.y < (currentCell.gameObject.transform.position.y + minDistance)){
					this.transform.position = currentCell.gameObject.transform.position;
					falling = false;
					speed = 0;
				}
				else{
					speed += gravity*Time.deltaTime;
					if(this.transform.position.y > waterYpos){
						if(speed > maxFallingSpeed){
							speed = maxFallingSpeed;
						}
					}
					else{
						if(speed > maxFallingWaterSpeed){
							speed = maxFallingWaterSpeed;
						}
					}
					this.transform.position = Vector2.MoveTowards(this.transform.position,currentCell.gameObject.transform.position,speed);
				}
			}
			CheckCell();
		}
			if(matched){
				currentCell.occupied = false;
				active = false;
				this.gameObject.SetActive(false);
			}
			if(selected){
				float scaleValue = scalingCurve.Evaluate(Time.time);
				this.gameObject.transform.localScale = initialScale * scaleValue;
			}else{
				this.gameObject.transform.localScale = initialScale;
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
		this.gameObject.SetActive(false);			
		currentCell = StartingCell;
		currentCell.occupied = falling =  false;
		changeCell = active = matched = false;
	}
	public void SetUp(){
		this.gameObject.SetActive(true);	
		this.transform.position = initialPos;
		currentCell = StartingCell;
		currentCell.occupied = falling =  true;
		changeCell = active = matched = false;
	}
}
