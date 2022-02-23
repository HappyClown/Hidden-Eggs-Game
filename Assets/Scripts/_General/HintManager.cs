using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintManager : MonoBehaviour {
	public GameObject featherGO;
	public GameObject hintSpaceGO;
	public List<ParticleSystem> hintObjFXs;
	public enum featherToGo {
		center,firstPoint,secondPoion,thirdPoint,fourthPoint,exit
	}
	public ClickOnEggs myClickonEggs;
	public SceneTapEnabler sceneTapScript;
	public Transform featherInitialPos;
	public HintQuadrant[] myQuadrants;
	public HintQuadrant currentQuadrant;
	public bool hintAvailable = true;
	public bool movingFeather;
	public featherToGo myDirection;
	public float minDistanceToPoint, featherMovSpeed;
	public int turnsToDo, currentTurn, eggsFound;
	public bool resetHint;
	public HintTrail hintTrailScript;
	public AudioHelperBird audioHelperBirdScript;
	private Coroutine hintRoutine;
	private WaitForSeconds waitFiveSecs = new WaitForSeconds(5f);
	
	void Start () {
		hintAvailable = true;
		currentTurn = 0;
		featherGO.transform.position = featherInitialPos.position;
		resetHint = false;
		if(!audioHelperBirdScript) {
			audioHelperBirdScript= GameObject.Find("Audio").GetComponent<AudioHelperBird>();
		}
	}

	public void StartHint() {
		if (hintAvailable && !movingFeather) {
			hintSpaceGO.SetActive(true);
			eggsFound = myClickonEggs.eggsFound;
			Vector2 eggPos = Vector2.zero;
			for (int i = 0; i < GlobalVariables.globVarScript.eggsFoundBools.Count; i++)
			{
				if(!GlobalVariables.globVarScript.eggsFoundBools[i]){
					eggPos = myClickonEggs.eggs[i].transform.position;
					break;
				}
			}
			movingFeather = true;
			hintAvailable = false;
			sceneTapScript.canTapHelpBird = false;
			//featherGO.SetActive(true); Only turning on the emission for the effects instead of the object.
			foreach(ParticleSystem fx in hintObjFXs){
				fx.Play();
			}
			SetQuadrant(eggPos);
			myDirection = featherToGo.firstPoint;
			if (hintRoutine != null) {
				StopCoroutine(hintRoutine);
			}
			hintRoutine = StartCoroutine(HintActive());
		}
	}
	IEnumerator HintActive() {
		while(movingFeather) {
			if(myClickonEggs.eggsFound  > eggsFound || resetHint) {
				print("Exiting prematurely.");
				if (hintTrailScript) {
					hintTrailScript.UnparentFromBall();
				}
				myDirection = featherToGo.exit;
				currentTurn = 0;
				resetHint = false;
			}
			MoveFeather();
			audioHelperBirdScript.hintSndOn();
			yield return null;
		}
		foreach(ParticleSystem fx in hintObjFXs){
			fx.Stop();
		}
		yield return waitFiveSecs;
		hintSpaceGO.SetActive(false);
	}
	public void ResetHint() {
		if (movingFeather) {
			resetHint = true;
		}
	}

	void SetQuadrant(Vector2 referencePosition){
		float minDist = 9999f;
		for (int i = 0; i < myQuadrants.Length ; i++)
		{
			if(Vector2.Distance(myQuadrants[i].referencePoint.position,referencePosition) < minDist) {
				minDist = Vector2.Distance(myQuadrants[i].referencePoint.position,referencePosition);
				currentQuadrant = myQuadrants[i];
			}
		}
	}

	void MoveFeather() {
		switch(myDirection) {
			case featherToGo.center:
				if(Vector2.Distance(featherGO.transform.position,gameObject.transform.position) > minDistanceToPoint) {
					featherGO.transform.position = Vector3.MoveTowards(featherGO.transform.position,gameObject.transform.position,Time.deltaTime * featherMovSpeed);
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
				else if(Vector2.Distance(featherGO.transform.position,currentQuadrant.firstPoint.position) > minDistanceToPoint) {
					featherGO.transform.position = Vector3.MoveTowards(featherGO.transform.position,currentQuadrant.firstPoint.position,Time.deltaTime * featherMovSpeed);
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
				if(Vector2.Distance(featherGO.transform.position,currentQuadrant.secondPoint.position) > minDistanceToPoint) {
					featherGO.transform.position = Vector3.MoveTowards(featherGO.transform.position,currentQuadrant.secondPoint.position,Time.deltaTime * featherMovSpeed);
				}
				else {
					myDirection = featherToGo.thirdPoint;
				}
			break;
			case featherToGo.thirdPoint:
				if(Vector2.Distance(featherGO.transform.position,currentQuadrant.thirdPoint.position) > minDistanceToPoint) {
					featherGO.transform.position = Vector3.MoveTowards(featherGO.transform.position,currentQuadrant.thirdPoint.position,Time.deltaTime * featherMovSpeed);
				}
				else {
					myDirection = featherToGo.fourthPoint;
				}
			break;
			case featherToGo.fourthPoint:
				if(Vector2.Distance(featherGO.transform.position,currentQuadrant.fourthPoint.position) > minDistanceToPoint) {
					featherGO.transform.position = Vector3.MoveTowards(featherGO.transform.position,currentQuadrant.fourthPoint.position,Time.deltaTime * featherMovSpeed);
				}
				else {
					myDirection = featherToGo.firstPoint;
				}
			break;
			case featherToGo.exit:
				if(Vector2.Distance(featherGO.transform.position,featherInitialPos.position) > minDistanceToPoint) {
					featherGO.transform.position = Vector3.MoveTowards(featherGO.transform.position,featherInitialPos.position,Time.deltaTime * featherMovSpeed);
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
