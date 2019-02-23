using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HelpButton : MonoBehaviour 
{
	private Button button;
	public SlideInHelpBird birdScript;
	public SceneTapEnabler sceneTapScript;
	public BirdIntroSave birdIntroSaveScript;

	void Start () 
	{
		button = this.GetComponent<Button>();
		button.onClick.AddListener(showBird);
		birdIntroSaveScript.LoadBirdIntro();
		if (!birdScript.introDone) {
			button.enabled = false;
		}
	}

	public void showBird() {
		if (sceneTapScript.canTapHelpBird) {
			birdScript.MoveBirdUpDown();
		}
	} 

}