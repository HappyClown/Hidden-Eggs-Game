using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzCloseMenuBtn : MonoBehaviour {
	public MenuStatesManager menuStatesScript;
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
		menuStatesScript.menuStates = MenuStatesManager.MenuStates.TurnOff;
	}
	void closePuzzleMenu () {
		menuStatesScript.puzzleConfStates = MenuStatesManager.MenuStates.TurnOff;
	}
}
