using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MonoBehaviour {
	private Button button;
	public string sceneName;
	public SceneTapEnabler sceneTapEnabScript;

	void Start () {
		button = this.GetComponent<Button>();
		button.onClick.AddListener(OpenScene);
	}

	public void OpenScene () {
		if (sceneName == GlobalVariables.globVarScript.menuName) {
			GlobalVariables.globVarScript.toHub = true;
		}
		GlobalVariables.globVarScript.sceneFadeScript.SwitchScene(sceneName);
		 
		sceneTapEnabScript.canTapEggRidPanPuz = false;
		sceneTapEnabScript.canTapHelpBird = false;
		sceneTapEnabScript.canTapPauseBtn = false;
		
	}
}
