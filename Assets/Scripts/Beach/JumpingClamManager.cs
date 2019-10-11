using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingClamManager : MonoBehaviour {
	public JumpingClam[] jumpingClamScripts;
	public bool startAnim;
	public PuzzleUnlock puzzUnlockScript;

	void Update () 
	{
		if (puzzUnlockScript.puzzUnlocked && !startAnim)
		{
			foreach(JumpingClam jumpClamScript in jumpingClamScripts)
			{
				jumpClamScript.StartClamAnim();
			}
			startAnim = true;
		}
	}
}
