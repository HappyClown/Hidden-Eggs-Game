using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneKiteAnim : MonoBehaviour 
{
	public Animator anim;
	public float gustMinCD, gustMaxCD, gustCD;
	private float gustTimer;
	public bool animEnabled, startAnim;
	public PuzzleUnlock puzzUnlockScript;


	void Update () 
	{
		if (puzzUnlockScript.puzzUnlocked && !startAnim)
		{
			SceneKiteEnabled();
			startAnim = true;
		}

		if (animEnabled)
		{
			gustTimer -= Time.deltaTime;
			if (gustTimer <= 0)
			{
				anim.SetTrigger("KiteGust");
				gustCD = Random.Range(gustMinCD, gustMaxCD);
				gustTimer = gustCD;
			}
		}
	}

	public void SceneKiteEnabled()
	{
		anim.SetTrigger("KiteLoop");
		animEnabled = true;
		gustCD = Random.Range(gustMinCD, gustMaxCD);
		gustTimer = gustCD;
	}
}