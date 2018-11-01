using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilverEggs : MonoBehaviour 
{
	public Animator eggAnim;
	public ParticleSystem clickFX;
	public SpriteRenderer spriteRen;
	public CircleCollider2D col;
	public SilverEggSequence silEggSeqScript;
	private Vector3 iniPos, iniRot, iniScale;
	public bool hollow;
	//public GrabItem grabItemScript;

	void Awake()
	{
		iniPos = this.transform.localPosition; 
		iniRot = this.transform.rotation.eulerAngles; 
		iniScale = this.transform.localScale;
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

	public void ResetSilEgg ()
	{
		//this.gameObject.SetActive(true);
		spriteRen.color = new Color(spriteRen.color.r, spriteRen.color.g, spriteRen.color.b, 1f);
		//col.enabled = true;	
		this.transform.localPosition = iniPos;
		this.transform.eulerAngles = iniRot;
		this.transform.localScale = iniScale;
		silEggSeqScript.ResetHover();
	}

	public void PlaySilverEggFX ()
	{
		clickFX.Play(true);
	}
}
