using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompEggAnimEvents : MonoBehaviour {
	public LevelCompleteEggMoveSpin levelCompEggScript;

	void MoveEggTrue() {
		levelCompEggScript.moveEgg = true;
	}

	public void GetReferences() {
		levelCompEggScript = this.GetComponentInChildren<LevelCompleteEggMoveSpin>();
	}
}
