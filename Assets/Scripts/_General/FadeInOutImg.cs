using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOutImg : MonoBehaviour 
{
	public bool fadingOut;
	public bool fadingIn;
	public float t; 
	public float fadeDuration;

	public bool fadeOnStart = true;
	private Image img;



	void Start ()
	{
		img = this.gameObject.GetComponent<Image>();

		//sprite.color = new Color(1f, 1f, 1f, 0f);

		if (fadeOnStart) { FadeIn(); }
	}



	void Update () 
	{
		if (fadingOut == true)
		{
			t += Time.deltaTime / fadeDuration;
			img.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(1f, 0f, t));
			if (t >= 1f)
			{
				this.gameObject.SetActive(false);
				fadingOut = false;
			}
		}

		if (fadingIn == true)
		{
			t += Time.deltaTime / fadeDuration;
			img.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(0f, 1f, t));
			if (t >= 1f)
			{
				fadingIn = false;
			}
		}
	}



	public void FadeOut ()
	{
		if (fadingOut == false && img.color.a >= 0.01f)
		{
			fadingOut = true;
			t = 0f;
			//Debug.Log("Should Fade Out");
		}
	}



	public void FadeIn ()
	{
		if (!this.gameObject.activeSelf)
		{
			this.gameObject.SetActive(true);
		}

		if (fadingIn == false/* && sprite.color.a <= 0.01f*/)
		{
			fadingIn = true;
			t = 0f;
			//Debug.Log("Should Fade In");
		}
	}
}


										// //// SAME THING BUT CHECKS IF IT HAS SPRITE RENDERER //////
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class FadeInOut : MonoBehaviour 
// {
// 	public bool fadingOut;
// 	public bool fadingIn;
// 	public float t;
// 	public float fadeDuration;
// 	private SpriteRenderer sprite;
// 	public bool hasSprite;



// 	void Start ()
// 	{
// 		if (this.GetComponent<SpriteRenderer>())
// 		{
// 			sprite = this.gameObject.GetComponent<SpriteRenderer>();
// 			hasSprite = true;
// 		}

// 		if (hasSprite) { sprite.color = new Color(1f, 1f, 1f, 0f); }
// 	}



// 	void Update () 
// 	{
// 		if (hasSprite && fadingOut == true)
// 		{
// 			t += Time.deltaTime / fadeDuration;
// 			sprite.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(1f, 0f, t));
// 			if (t >= 1f)
// 			{
// 				this.gameObject.SetActive(false);
// 				fadingOut = false;
// 			}
// 		}
// 		else if (!hasSprite && fadingOut)
// 		{
// 			this.gameObject.SetActive(false);
// 		}

// 		if (hasSprite && fadingIn == true)
// 		{
// 			t += Time.deltaTime / fadeDuration;
// 			sprite.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(0f, 1f, t));
// 			if (t >= 1f)
// 			{
// 				fadingIn = false;
// 			}
// 		}
// 	}



// 	public void FadeOut ()
// 	{
// 		if (hasSprite && fadingOut == false && sprite.color.a >= 0.01f)
// 		{
// 			fadingOut = true;
// 			t = 0f;
// 			//Debug.Log("Should Fade Out");
// 		}
// 		else if (!hasSprite)
// 		{
// 			fadingOut = true;
// 		}
// 	}



// 	public void FadeIn ()
// 	{
// 		if (hasSprite && fadingIn == false/* && sprite.color.a <= 0.01f*/)
// 		{
// 			fadingIn = true;
// 			t = 0f;
// 			//Debug.Log("Should Fade In");
// 		}
// 	}
// }
