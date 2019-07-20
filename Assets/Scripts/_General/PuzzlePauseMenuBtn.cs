using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzlePauseMenuBtn : MonoBehaviour {
	public MenuStatesManager menuStatesScript;
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
		menuStatesScript.menuActive = true;
		menuStatesScript.menuStates = MenuStatesManager.MenuStates.TurnOn;
	}
}
