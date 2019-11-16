using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintManager : MonoBehaviour {
	public GameObject feather;
	public List<ParticleSystem> hintObjFXs;
	public enum featherToGo {
		center,firstPoint,secondPoion,thirdPoint,fourthPoint,exit
	}
	public ClickOnEggs myClickonEggs;
	public SceneTapEnabler sceneTapScript;
	public Transform featherInitialPos;
	public HintQuadrant[] myQuadrants;
	public HintQuadrant currentQuadrant;
	public bool hintAvailable, movingFeather, startHint;
	public featherToGo myDirection;
	public float minDistanceToPoint, featherMovSpeed;
	public int turnsToDo, currentTurn, eggsFound;
	public bool resetHint;
	public HintTrail hintTrailScript;

	
	public AudioHelperBird audioHelperBirdScript;
	
	void Start () {
		hintAvailable = true;
		currentTurn = 0;
		feather.transform.position = featherInitialPos.position;
		resetHint = false;
		
		if(!audioHelperBirdScript){audioHelperBirdScript= GameObject.Find("Audio").GetComponent<AudioHelperBird>();}
	}
	
	void Update () {
		if(/* Input.GetKey(KeyCode.I) */startHint && hintAvailable && !movingFeather){
			eggsFound = myClickonEggs.eggsFound;
			Vector2 eggPos = Vector2.zero ;
			for (int i = 0; i < myClickonEggs.eggs.Count; i++)
			{
				if(!myClickonEggs.eggs[i].GetComponent<EggGoToCorner>().eggFound){
					eggPos = myClickonEggs.eggs[i].gameObject.transform.position;
					i = myClickonEggs.eggs.Count;
				}
			}
			movingFeather = true;
			sceneTapScript.canTapHelpBird = false;
			//feather.SetActive(true); Only turning on the emission for the effects instead of the object.
			foreach(ParticleSystem fx in hintObjFXs){
				var em = fx.emission;
				em.enabled = true;
			}
			SetQuadrant(eggPos);
			startHint = false;
		}
		if(movingFeather){
			if(myClickonEggs.eggsFound  > eggsFound || resetHint) {
				if (hintTrailScript) {
					hintTrailScript.UnparentFromBall();
				}
				myDirection = featherToGo.exit;
				currentTurn = 0;
				resetHint = false;
			}
			MoveFeather();

			
			//SOUND
			audioHelperBirdScript.hintSndOn();
		}
		else{
			//feather.SetActive(false);
			foreach(ParticleSystem fx in hintObjFXs){
				var em = fx.emission;
				em.enabled = false;
			}
		}
	}

	void SetQuadrant(Vector2 referencePosition){
		float minDist = 9999999;
		for (int i = 0; i < myQuadrants.Length ; i++)
		{
			if(Vector2.Distance(myQuadrants[i].referencePoint.position,referencePosition) < minDist) {
				minDist = Vector2.Distance(myQuadrants[i].referencePoint.position,referencePosition);
				currentQuadrant = myQuadrants[i];
			}
		}
	}

	void MoveFeather() {
		if(hintAvailable) {
			hintAvailable = false;
			myDirection = featherToGo.firstPoint;
			hintTrailScript.ClearTrail();
		}
		switch(myDirection) {
			case featherToGo.center:
				if(Vector2.Distance(feather.transform.position,gameObject.transform.position) > minDistanceToPoint) {
					feather.transform.position = Vector3.MoveTowards(feather.transform.position,gameObject.transform.position,Time.deltaTime * featherMovSpeed);
				}
				else {
					myDirection = featherToGo.firstPoint;
				}
			break;
			case featherToGo.firstPoint:
				if(currentTurn == turnsToDo){
					if (hintTrailScript) {
						hintTrailScript.UnparentFromBall();
					}
					myDirection = featherToGo.exit;
					currentTurn = 0;
				}
				else if(Vector2.Distance(feather.transform.position,currentQuadrant.firstPoint.position) > minDistanceToPoint) {
					feather.transform.position = Vector3.MoveTowards(feather.transform.position,currentQuadrant.firstPoint.position,Time.deltaTime * featherMovSpeed);
				}
				else {
					if (hintTrailScript && currentTurn == 0) {
						hintTrailScript.ParentTHintBall();
						hintTrailScript.ClearTrail();
					}
					currentTurn ++;
					myDirection = featherToGo.secondPoion;
				}
			break;
			case featherToGo.secondPoion:
				if(Vector2.Distance(feather.transform.position,currentQuadrant.secondPoint.position) > minDistanceToPoint) {
					feather.transform.position = Vector3.MoveTowards(feather.transform.position,currentQuadrant.secondPoint.position,Time.deltaTime * featherMovSpeed);
				}
				else {
					myDirection = featherToGo.thirdPoint;
				}
			break;
			case featherToGo.thirdPoint:
				if(Vector2.Distance(feather.transform.position,currentQuadrant.thirdPoint.position) > minDistanceToPoint) {
					feather.transform.position = Vector3.MoveTowards(feather.transform.position,currentQuadrant.thirdPoint.position,Time.deltaTime * featherMovSpeed);
				}
				else {
					myDirection = featherToGo.fourthPoint;
				}
			break;
			case featherToGo.fourthPoint:
				if(Vector2.Distance(feather.transform.position,currentQuadrant.fourthPoint.position) > minDistanceToPoint) {
					feather.transform.position = Vector3.MoveTowards(feather.transform.position,currentQuadrant.fourthPoint.position,Time.deltaTime * featherMovSpeed);
				}
				else {
					myDirection = featherToGo.firstPoint;
				}
			break;
			case featherToGo.exit:
				if(Vector2.Distance(feather.transform.position,featherInitialPos.position) > minDistanceToPoint) {
					feather.transform.position = Vector3.MoveTowards(feather.transform.position,featherInitialPos.position,Time.deltaTime * featherMovSpeed);
				}
				else {
					movingFeather = false;
					hintAvailable = true;
					sceneTapScript.canTapHelpBird = true;

				//STOP
				audioHelperBirdScript.hintSndOnLongStop();
				}
			break;
		}
	}
}
