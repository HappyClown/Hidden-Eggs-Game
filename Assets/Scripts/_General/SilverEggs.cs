using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilverEggs : MonoBehaviour 
{
	public Animator eggAnim;
	public ParticleSystem clickFX;



	public void StartSilverEggAnim () 
	{
		eggAnim.SetTrigger("EggPop");
	}



	public void SetSelfInactive ()
	{
		this.gameObject.SetActive(false);
	}



	public void PlaySilverEggFX ()
	{
		clickFX.Play(true);
	}
}
