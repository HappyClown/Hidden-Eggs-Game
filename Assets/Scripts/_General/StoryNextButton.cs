using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryNextButton : MonoBehaviour {
	public Button thisNextButton;
	public StoryIntro storyIntroScript;

	void Start () {
		thisNextButton.onClick.AddListener(NextButtonTapped);
	}
	
	void NextButtonTapped () {
		storyIntroScript.nextBtnPressed = true;
	}
}
