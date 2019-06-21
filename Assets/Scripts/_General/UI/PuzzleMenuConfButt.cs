using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleMenuConfButt : MonoBehaviour {

	public ClickOnEggs myClickOnEggs;
	public Button button;

	void Start () {
		button = this.GetComponent<Button>();
		button.onClick.AddListener(OpenPuzzle);
	}
	
	void OpenPuzzle () {
		myClickOnEggs.LoadPuzzle();
	}
}
