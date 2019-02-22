using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzHelpBtn : MonoBehaviour {

	public SlideInHelpBird slideInHelpScript;
	public PuzzlePauseMenu puzzlePauseScript;
	public Button button;

	void Start () {
		button = this.GetComponent<Button>();
		button.onClick.AddListener(ShowTut);
	}
	
	void ShowTut () {
		slideInHelpScript.MoveBirdUpDown();
		puzzlePauseScript.menuStates = PuzzlePauseMenu.MenuStates.TurnOff;
	}
}
