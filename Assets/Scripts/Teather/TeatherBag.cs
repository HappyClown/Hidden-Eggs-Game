using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeatherBag : MonoBehaviour {

	public float Value;
	public bool up, down;
	public int movesQuantity;
	private int currentMove;
	public float amountToMove;
	// Use this for initialization
	void Start () {
		currentMove = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void MoveBagUp(){
		currentMove ++;
		if(down){
			down = false;
		}
		else if(Mathf.Abs(currentMove) == movesQuantity){
			up = true;
		}
		Vector3 currentPos = this.gameObject.GetComponent<RectTransform>().position;

		this.gameObject.GetComponent<RectTransform>().position = new Vector3(currentPos.x, currentPos.y+amountToMove,currentPos.z);
	}
	public void MoveBagDown(){
		currentMove --;
		if(up){
			up = false;
		}
		else if(Mathf.Abs(currentMove) == movesQuantity){
			down = true;
		}
		Vector3 currentPos = this.gameObject.GetComponent<RectTransform>().position;

		this.gameObject.GetComponent<RectTransform>().position = new Vector3(currentPos.x, currentPos.y-amountToMove,currentPos.z);
	}
}
