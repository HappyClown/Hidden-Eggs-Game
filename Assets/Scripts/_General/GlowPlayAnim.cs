using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowPlayAnim : MonoBehaviour 
{
	public Animation anim;
	public SpriteRenderer spriteRend;
	public bool setStartAlphaZero = true;

	public float fadeToAlpha;
	public bool fadingIn;
	public bool fadeOnStart = true;
	public float t;
	public float fadeDuration;



	void Start () 
	{
		if (setStartAlphaZero) { spriteRend.color = new Color (1,1,1, 0); }

		if (fadeOnStart) { FadeIn(); }
	}
	


	void Update () 
	{
		if (spriteRend.color.a >= 0.6f)
		{
			anim.Play();
		}


		if (fadingIn == true)
		{
			t += Time.deltaTime / fadeDuration;
			spriteRend.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(0f, fadeToAlpha, t));
			if (t >= 1f)
			{
				fadingIn = false;
			}
		}
	}



	public void FadeIn ()
	{
		if (fadingIn == false/* && sprite.color.a <= 0.01f*/)
		{
			fadingIn = true;
			t = 0f;
			//Debug.Log("Should Fade In");
		}
	}
}




	