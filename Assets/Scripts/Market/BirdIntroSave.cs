using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BirdIntroSave : MonoBehaviour {

	public SlideInHelpBird slideInHelpScript;
	
	public void SaveBirdIntro () {
		if (SceneManager.GetActiveScene().name == GlobalVariables.globVarScript.marketName) {
			GlobalVariables.globVarScript.birdIntroDone = true;
			GlobalVariables.globVarScript.SaveEggState();
		}
	}

	public void LoadBirdIntro () {
		if (SceneManager.GetActiveScene().name == GlobalVariables.globVarScript.marketName) {
			slideInHelpScript.introDone = GlobalVariables.globVarScript.birdIntroDone;
		}
	}
}