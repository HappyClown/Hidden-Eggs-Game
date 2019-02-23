using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BirdIntroSave : MonoBehaviour {

	public SlideInHelpBird slideInHelpScript;
	
	public void SaveBirdIntro () {
		GlobalVariables.globVarScript.birdIntroDone = true;
		GlobalVariables.globVarScript.SaveEggState();
	}

	public void LoadBirdIntro () {
		slideInHelpScript.introDone = GlobalVariables.globVarScript.birdIntroDone;
	}
}