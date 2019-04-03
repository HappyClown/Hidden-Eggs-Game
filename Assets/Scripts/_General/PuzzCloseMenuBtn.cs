using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzCloseMenuBtn : MonoBehaviour {
	public PuzzlePauseMenu puzzPauseScript;
	public Button button;
	public SceneTapEnabler sceneTapScript;

	void Start () {
		button = this.GetComponent<Button>();
		button.onClick.AddListener(CloseMenu);
	}
	
	void CloseMenu () {
		puzzPauseScript.menuStates = PuzzlePauseMenu.MenuStates.TurnOff;
		//sceneTapScript.TapLevelStuffTrue();
	}
}
