using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneKiteAnim : ScenePuzzObjectAnim 
{
	public Animator anim;
	public float gustMinCD, gustMaxCD, gustCD;
	private float gustTimer;
	public bool animEnabled, startAnim;
	public PuzzleUnlock puzzUnlockScript;


	void Update () 
	{
		if (animEnabled)
		{
			gustTimer -= Time.deltaTime;
			if (gustTimer <= 0)
			{
				anim.SetTrigger("PlayStateOne");
				gustCD = Random.Range(gustMinCD, gustMaxCD);
				gustTimer = gustCD;
			}
		}
	}

	override public void StartAnim()
	{
		anim.SetTrigger("StartAnim");
		animEnabled = true;
		gustCD = Random.Range(gustMinCD, gustMaxCD);
		gustTimer = gustCD;
	}
}