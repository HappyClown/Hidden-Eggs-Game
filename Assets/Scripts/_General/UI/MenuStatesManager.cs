﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuStatesManager : MonoBehaviour {
	public enum MenuStates {
		TurnOn, TurningOn, IsOn, TurnOff, TurningOff, IsOff
	}
	public enum PuzzleConfStates{
		TurnOn, TurningOn, IsOn, TurnOff, TurningOff, IsOff
	}
	public MenuStates menuStates;
	public PuzzleConfStates puzzleConfStates;
	public Collider2D col, colPuzz;
	
	private RaycastHit2D hit;
	private Vector2 mousePos2D;
	private Vector3 mousePos;

	public inputDetector inputDetScript;
	public MainPuzzleEngine puzzEngScript;

	public bool menuActive, puzzleConfActive, putDragOff;
	public CanvasGroup menuCG, sceneUICG, puzzleConfCG;
	private float lerpValue;
	public float fadeDuration;
	//public bool inScene;
	public SceneTapEnabler sceneTapScript;

	void Start () {
		menuCG.alpha = 0;
		menuCG.interactable = false;
		if(puzzleConfCG){
			puzzleConfCG.alpha = 0;
			puzzleConfCG.interactable = false;
		}
	}
	
	void Update () {
		if (puzzEngScript) {
			if (!puzzEngScript.canPlay) {
				sceneTapScript.canTapPauseBtn = false;
			}
			else {
				sceneTapScript.canTapPauseBtn = true;
			}
		}

		if (menuActive) {
			switch (menuStates) {
				case MenuStates.TurnOn:
					TurnOn(); break;
				case MenuStates.TurningOn:
					TurningOn(); break;
				case MenuStates.IsOn:
					IsOn(); break;
				case MenuStates.TurnOff:
					TurnOff(); break;
				case MenuStates.TurningOff:
					TurningOff(); break;
				case MenuStates.IsOff:
					IsOff(); break;
				default:
				break;
			}
		}
		
		if(puzzleConfActive){
			switch (puzzleConfStates) {
				case PuzzleConfStates.TurnOn:
					PuzzleConfTurnOn(); break;
				case PuzzleConfStates.TurningOn:
					PuzzleConfTurningOn(); break;
				case PuzzleConfStates.IsOn:
					PuzzleConfIsOn(); break;
				case PuzzleConfStates.TurnOff:
					PuzzleConfTurnOff(); break;
				case PuzzleConfStates.TurningOff:
					PuzzleConfTurningOff(); break;
				case PuzzleConfStates.IsOff:
					PuzzleConfIsOff(); break;
				default:
				break;
			}
		}
	}

	void TurnOn() {
		sceneUICG.interactable = false;
		menuStates = MenuStates.TurningOn;
		if (puzzEngScript) {
			puzzEngScript.canPlay = false;
		}
		if (sceneTapScript) {
			sceneTapScript.TapLevelStuffFalse();
		}
		col.enabled = true;
		
		if(inputDetScript.detectDrag){
			putDragOff = false;
		}else{
			inputDetScript.detectDrag = true;
			putDragOff = true;
		}
	}

	void TurningOn() {
		lerpValue += Time.deltaTime / fadeDuration;
		menuCG.alpha = Mathf.Lerp(0, 1, lerpValue);

		if (lerpValue >= 1) {
			menuCG.alpha = 1;
			lerpValue = 0;
			menuStates = MenuStates.IsOn;
		}
	}

	void IsOn() {
		if (!menuCG.interactable) {
			menuCG.interactable = true;
		}

		if (inputDetScript.Tapped) {
			Debug.Log("lalallaa");
			UpdateMousePos();
			hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f);
			if (hit && !hit.collider.CompareTag("PuzzlePauseMenu") || !hit) {
				menuStates = MenuStates.TurnOff;
				if (sceneTapScript) {
					sceneTapScript.TapLevelStuffTrue();
				}
			}
			
		}
	}

	void TurnOff() {
		menuCG.interactable = false;
		menuStates = MenuStates.TurningOff;
		col.enabled = false;
		if(putDragOff){
			inputDetScript.detectDrag = false;
		}
	}

	void TurningOff() {
		lerpValue += Time.deltaTime / fadeDuration;
		menuCG.alpha = Mathf.Lerp(1, 0, lerpValue);

		if (lerpValue >= 1) {
			menuCG.alpha = 0;
			lerpValue = 0;
			menuStates = MenuStates.IsOff;
			sceneUICG.interactable = true;
			menuActive = false;
			if (puzzEngScript) {
				puzzEngScript.canPlay = true;
			}
			if (sceneTapScript) {
					sceneTapScript.TapLevelStuffTrue();
			}
		}
	}

	void IsOff() {
		
	}

	void PuzzleConfTurnOn() {
		sceneUICG.interactable = false;
		puzzleConfStates = PuzzleConfStates.TurningOn;
		if (puzzEngScript) {
			puzzEngScript.canPlay = false;
		}
		if (sceneTapScript) {
			sceneTapScript.TapLevelStuffFalse();
		}
		colPuzz.enabled = true;
		if(inputDetScript.detectDrag){
			putDragOff = false;
		}else{
			inputDetScript.detectDrag = true;
			putDragOff = true;
		}
	}

	void PuzzleConfTurningOn() {
		lerpValue += Time.deltaTime / fadeDuration;
		puzzleConfCG.alpha = Mathf.Lerp(0, 1, lerpValue);

		if (lerpValue >= 1) {
			puzzleConfCG.alpha = 1;
			lerpValue = 0;
			puzzleConfStates = PuzzleConfStates.IsOn;
		}
	}

	void PuzzleConfIsOn() {
		if (!puzzleConfCG.interactable) {
			puzzleConfCG.interactable = true;
		}

		if (inputDetScript.Tapped) {
			UpdateMousePos();
			hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f);
			if (hit && !hit.collider.CompareTag("PuzzlePauseMenu") || !hit) {
				puzzleConfStates = PuzzleConfStates.TurnOff;
				if (sceneTapScript) {
					sceneTapScript.TapLevelStuffTrue();
				}
			}
		}
	}

	void PuzzleConfTurnOff() {
		puzzleConfCG.interactable = false;
		puzzleConfStates = PuzzleConfStates.TurningOff;
		colPuzz.enabled = false;
		if(putDragOff){
			inputDetScript.detectDrag = false;
		}
	}

	void PuzzleConfTurningOff() {
		lerpValue += Time.deltaTime / fadeDuration;
		puzzleConfCG.alpha = Mathf.Lerp(1, 0, lerpValue);

		if (lerpValue >= 1) {
			puzzleConfCG.alpha = 0;
			lerpValue = 0;
			puzzleConfStates = PuzzleConfStates.IsOff;
			sceneUICG.interactable = true;
			puzzleConfActive = false;
			if (puzzEngScript) {
				puzzEngScript.canPlay = true;
			}
			if (sceneTapScript) {
					sceneTapScript.TapLevelStuffTrue();
			}
		}
	}

	void PuzzleConfIsOff() {
		
	}

	public void UpdateMousePos() {
		mousePos = Camera.main.ScreenToWorldPoint(inputDetScript.TapPosition);
		mousePos2D = new Vector2 (mousePos.x, mousePos.y);
	}
}