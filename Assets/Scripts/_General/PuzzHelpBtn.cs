using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzHelpBtn : MonoBehaviour {

	public SlideInHelpBird slideInHelpScript;
	public MenuStatesManager menuStatesScript;
	public Button button;
	public bool inScene;

	void Start () {
		button = this.GetComponent<Button>();
		button.onClick.AddListener(ShowTut);
	}

	void Update () {
		if (inScene && !slideInHelpScript.introDone) {
			button.interactable = false;
		}
		else {
			button.interactable = true;
		}
	}
	
	void ShowTut () {
		// if (inScene) {
		// 	if (slideInHelpScript.introDone) {
		// 		slideInHelpScript.MoveBirdUpDown();
		// 		menuStatesScript.menuStates = PuzzlePauseMenu.MenuStates.TurnOff;
		// 	}
		// }
		// else {
			if (inScene) {
				slideInHelpScript.introDone = false;
			}
			menuStatesScript.menuStates = MenuStatesManager.MenuStates.TurnOff;
			slideInHelpScript.MoveBirdUpDown();
		// }
	}
}
