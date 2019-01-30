using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HelpButton : MonoBehaviour 
{
	private Button button;
	public SlideInHelpBird birdScript;
	public SceneTapEnabler sceneTapScript;

	void Start () 
	{
		button = this.GetComponent<Button>();
		button.onClick.AddListener(showBird);
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