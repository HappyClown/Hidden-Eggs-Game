﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilverEggsManager : MonoBehaviour {
	private RaycastHit2D hit;
	private Vector2 mousePos2D;
	private Vector3 mousePos;

	public int silverEggsPickedUp;
	public Sprite hollowSilEgg;
	public List<GameObject> lvlSilverEggs, activeSilverEggs, allSilEggs;
	public List<SilverEggs> allSilverEggsScripts;
	public bool silverEggsActive;
	public bool skippedSeq;
	public int amntSilEggsTapped;

	public inputDetector inputDetScript;
	public MainPuzzleEngine mainPuzzleEngScript;
	public AudioSceneParkPuzzle audioSceneParkPuzzScript;

	void Start() {
		silverEggsPickedUp = GlobalVariables.globVarScript.silverEggsCount;
	}

	void Update() {
		// Clicking on a silver egg.
		if (inputDetScript.Tapped && !skippedSeq) {
			UpdateMousePos();
			hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f);
			if (hit) {
			//Debug.Log("Tapped and hit something. Apparently: " + hit.collider.gameObject);
				if (hit.collider.CompareTag("Egg")) {
					//Debug.Log("Tapped on an egg");
					SilverEggs silEggTappedScript = hit.collider.gameObject.GetComponent<SilverEggs>();
					silEggTappedScript.StartSilverEggAnim();
					hit.collider.enabled = false;
					//audioSceneParkPuzzScript.silverEgg();
					if (!silEggTappedScript.hollow) { silverEggsPickedUp++; }
					SaveSilverEggsToCorrectFile();
					SaveNewSilEggsFound(allSilEggs.IndexOf(hit.collider.gameObject));
					amntSilEggsTapped++;
					mainPuzzleEngScript.SilverEggsCheck(); // Check if the Silver Eggs have all been collected.
				}
			}
		}
		skippedSeq = false;
	}

	public void UpdateMousePos() {
		mousePos = Camera.main.ScreenToWorldPoint(inputDetScript.TapPosition);
		mousePos2D = new Vector2 (mousePos.x, mousePos.y);
	}

	// Silver Eggs counter Saving Funcion
	public void SaveSilverEggsToCorrectFile() {
		if (silverEggsPickedUp > GlobalVariables.globVarScript.silverEggsCount) {
			GlobalVariables.globVarScript.totalEggsFound += 1;
			GlobalVariables.globVarScript.silverEggsCount = silverEggsPickedUp; 
			GlobalVariables.globVarScript.SaveEggState();
		}
	}
	//New Silver Eggs Found Saving Function
	public void SaveNewSilEggsFound(int newSilEggFound) {
		//bool alreadySaved = false;
		foreach (int silEggNumber in GlobalVariables.globVarScript.puzzSilEggsCount)
		{
			if (silEggNumber == newSilEggFound) {
				return;
			}
		}
		GlobalVariables.globVarScript.puzzSilEggsCount.Add(newSilEggFound);
		GlobalVariables.globVarScript.SaveEggState();
	}
}
