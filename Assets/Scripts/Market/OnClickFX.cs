using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickFX : MonoBehaviour 
{
	public ParticleSystem particle;



	public void PlayFX()
	{
		//if (!particle.isPlaying)
		//{
			particle.Play();
		//}
	}
}
