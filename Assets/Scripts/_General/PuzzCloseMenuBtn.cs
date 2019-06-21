using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzCloseMenuBtn : MonoBehaviour {
	public PuzzlePauseMenu puzzPauseScript;
	public Button button;
	public bool forMainMenu, forPuzzleMenu;

	void Start () {
		button = this.GetComponent<Button>();
		if(forMainMenu)
		button.onClick.AddListener(CloseMenu);
		if(forPuzzleMenu)
		button.onClick.AddListener(closePuzzleMenu);
	}
	
	void CloseMenu () {
		puzzPauseScript.menuStates = PuzzlePauseMenu.MenuStates.TurnOff;
	}
	void closePuzzleMenu () {
		puzzPauseScript.puzzleConfStates = PuzzlePauseMenu.PuzzleConfStates.TurnOff;
	}
}
