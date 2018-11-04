using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintManager : MonoBehaviour {

	public GameObject feather;
	public enum featherToGo{
		center,firstPoint,secondPoion,thirdPoint,fourthPoint,exit
	}
	public ClickOnEggs myClickonEggs;
	public Transform featherInitialPos;
	public HintQuadrant[] myQuadrants;
	public HintQuadrant currentQuadrant;
	public bool hintAvailable, movingFeather;
	public featherToGo myDirection;
	public float minDistanceToPoint, featherMovSpeed;
	public int turnsToDo, currentTurn;
	// Use this for initialization
	void Start () {
		hintAvailable = true;
		currentTurn = 0;
		feather.transform.position = featherInitialPos.position;
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKey(KeyCode.I) && myClickonEggs.eggsLeft > 0 && hintAvailable){
			Vector2 eggPos = Vector2.zero ;
			for (int i = 0; i < myClickonEggs.eggs.Count; i++)
			{
				if(!myClickonEggs.eggs[i].GetComponent<EggGoToCorner>().eggFound){
					eggPos = myClickonEggs.eggs[i].gameObject.transform.position;
					i = myClickonEggs.eggs.Count;
				}
			}
			movingFeather = true;
			feather.SetActive(true);
			SetQuadrant(eggPos);
		}
		if(movingFeather){
			MoveFeather();
		}
		else{
			feather.SetActive(false);
		}
	}

	void SetQuadrant(Vector2 referencePosition){
		float minDist = 9999999;
		for (int i = 0; i < myQuadrants.Length ; i++)
		{
			if(Vector2.Distance(myQuadrants[i].referencePoint.position,referencePosition) < minDist){
				minDist = Vector2.Distance(myQuadrants[i].referencePoint.position,referencePosition);
				currentQuadrant = myQuadrants[i];
			}
		}
	}
	void MoveFeather(){
		if(hintAvailable){
			hintAvailable = false;
			myDirection = featherToGo.center;
		}
		switch(myDirection){
			case featherToGo.center:
				if(Vector2.Distance(feather.transform.position,gameObject.transform.position) > minDistanceToPoint){
					feather.transform.position = Vector3.MoveTowards(feather.transform.position,gameObject.transform.position,Time.deltaTime * featherMovSpeed);
				}
				else{
					myDirection = featherToGo.firstPoint;
				}
			break;
			case featherToGo.firstPoint:
				if(Vector2.Distance(feather.transform.position,currentQuadrant.firstPoint.position) > minDistanceToPoint){
					feather.transform.position = Vector3.MoveTowards(feather.transform.position,currentQuadrant.firstPoint.position,Time.deltaTime * featherMovSpeed);
				}
				else{
					currentTurn ++;
					if(currentTurn > turnsToDo){
						myDirection = featherToGo.exit;
						currentTurn = 0;
					}
					else{
						myDirection = featherToGo.secondPoion;
					}
				}
			break;
			case featherToGo.secondPoion:
				if(Vector2.Distance(feather.transform.position,currentQuadrant.secondPoint.position) > minDistanceToPoint){
					feather.transform.position = Vector3.MoveTowards(feather.transform.position,currentQuadrant.secondPoint.position,Time.deltaTime * featherMovSpeed);
				}
				else{
					myDirection = featherToGo.thirdPoint;
				}
			break;
			case featherToGo.thirdPoint:
				if(Vector2.Distance(feather.transform.position,currentQuadrant.thirdPoint.position) > minDistanceToPoint){
					feather.transform.position = Vector3.MoveTowards(feather.transform.position,currentQuadrant.thirdPoint.position,Time.deltaTime * featherMovSpeed);
				}
				else{
					myDirection = featherToGo.fourthPoint;
				}
			break;
			case featherToGo.fourthPoint:
				if(Vector2.Distance(feather.transform.position,currentQuadrant.fourthPoint.position) > minDistanceToPoint){
					feather.transform.position = Vector3.MoveTowards(feather.transform.position,currentQuadrant.fourthPoint.position,Time.deltaTime * featherMovSpeed);
				}
				else{
					myDirection = featherToGo.firstPoint;
				}
			break;
			case featherToGo.exit:
				if(Vector2.Distance(feather.transform.position,featherInitialPos.position) > minDistanceToPoint){
					feather.transform.position = Vector3.MoveTowards(feather.transform.position,featherInitialPos.position,Time.deltaTime * featherMovSpeed);
				}
				else{
					movingFeather = false;
					hintAvailable = true;
				}
			break;
		}
	}
}
