using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilverEggs : MonoBehaviour 
{
	public Animator eggAnim;
	public ParticleSystem clickFX;
	public SpriteRenderer spriteRen;
	public CircleCollider2D col;
	//public GrabItem grabItemScript;

	void Update()
	{

	}

	public void StartSilverEggAnim () 
	{
		eggAnim.SetTrigger("EggPop");
	}

	public void SetSelfInactive ()
	{
		
		//this.gameObject.SetActive(false);
		spriteRen.color = new Color(spriteRen.color.r, spriteRen.color.g, spriteRen.color.b, 0f);
	}

	public void SilverEggOn ()
	{
		//this.gameObject.SetActive(true);
		spriteRen.color = new Color(spriteRen.color.r, spriteRen.color.g, spriteRen.color.b, 1f);
		col.enabled = true;	
	}

	public void PlaySilverEggFX ()
	{
		clickFX.Play(true);
	}
}
