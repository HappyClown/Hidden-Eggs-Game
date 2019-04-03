using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzlePauseMenu : MonoBehaviour {
	public enum MenuStates {
		TurnOn, TurningOn, IsOn, TurnOff, TurningOff, IsOff
	}
	public MenuStates menuStates;
	public Collider2D col;
	
	private RaycastHit2D hit;
	private Vector2 mousePos2D;
	private Vector3 mousePos;

	public inputDetector inputDetScript;
	public MainPuzzleEngine puzzEngScript;

	public bool menuActive;
	public CanvasGroup menuCG, sceneUICG;
	private float lerpValue;
	public float fadeDuration;
	//public bool inScene;
	public SceneTapEnabler sceneTapScript;

	void Start () {
		menuCG.alpha = 0;
		menuCG.interactable = false;
	}
	
	void Update () {
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
			UpdateMousePos();
			hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f);
			if (hit && !hit.collider.CompareTag("PuzzlePauseMenu")) {
				menuStates = MenuStates.TurnOff;
			}
		}
	}

	void TurnOff() {
		menuCG.interactable = false;
		menuStates = MenuStates.TurningOff;
		if (sceneTapScript) {
			sceneTapScript.TapLevelStuffTrue();
		}
		col.enabled = false;
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
		}
	}

	void IsOff() {
		
	}

	public void UpdateMousePos() {
		mousePos = Camera.main.ScreenToWorldPoint(inputDetScript.TapPosition);
		mousePos2D = new Vector2 (mousePos.x, mousePos.y);
	}
}
