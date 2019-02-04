using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzlePauseMenuBtn : MonoBehaviour {
	public PuzzlePauseMenu puzzlePauseScript;
	public SceneTapEnabler sceneTapEnaScript;
	public Button button;

	void Start () {
		button.onClick.AddListener(PauseMenuOn);
	}

	void Update () {
		if (!sceneTapEnaScript.canTapPauseBtn) {
			button.interactable = false;
		}
		else {
			button.interactable = true;
		}
	}

	void PauseMenuOn() {
		puzzlePauseScript.menuActive = true;
		puzzlePauseScript.menuStates = PuzzlePauseMenu.MenuStates.TurnOn;
	}
}
