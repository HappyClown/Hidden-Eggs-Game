using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineCaller : MonoBehaviour {
	public PuzzleEngineY puzzYScript;
	public GreatBigPuzzleEngine greatBigScript;

	void Start () {
		Debug.Log("Just called PuzzleEngineY.puzzYFive: " + puzzYScript.puzzYFive);
		Debug.Log("Just called PuzzleEngineY.theNumberOne (only declared in the GreatBigPuzzleENgine script): " + greatBigScript.theNumberOne);
	}
	
	void Update () {
		
	}
}
