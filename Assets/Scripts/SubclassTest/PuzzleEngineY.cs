using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleEngineY : GreatBigPuzzleEngine {
	public int puzzYFive = 5;

	void Start () {
		PlayMessage(puzzYFive);
	}
	
	public new void PlayMessage (int diffNumber) {
		Debug.Log("This plays a message not a massege. BIG difference. Also the number you have inpotatoed is: " + diffNumber);
	}
}
