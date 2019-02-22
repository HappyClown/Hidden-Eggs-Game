using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleEngineX : GreatBigPuzzleEngine {
	public int puzzXFour = 4;

	void Start () {
		PlayMessage(theNumberOne);
	}
	
	// public void PlayMessage (int diffNumber) {
	// 	Debug.Log("This plays a message not a massege. BIG difference. Also the number you have inpotatoed is: " + diffNumber);
	// }
}
