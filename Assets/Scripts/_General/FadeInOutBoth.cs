using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOutBoth : MonoBehaviour 
{
	public bool fadeOnStart = true;
	public float fadeDuration;

	[TooltipAttribute("Set inactive on fade out?")]
	public bool setInactiveOnFadeOut;
	
	public enum MyDisplayType{Sprite, Image};

	[TooltipAttribute("Chose what type of display the object uses.")]
	public MyDisplayType myDisplayType;
	private bool fadingOut;
	private bool fadingIn;
	public float t; 
	//public float T{get{return t;}}
	private SpriteRenderer mySprite;
	private Image myImg;
	private Color myColor;
	


	void Awake ()
	{
		if (myDisplayType == MyDisplayType.Sprite) { mySprite = this.gameObject.GetComponent<SpriteRenderer>(); }
		else if (myDisplayType == MyDisplayType.Image) { myImg = this.gameObject.GetComponent<Image>(); }

		// - Assign The Object's Initial Color - //
		if (mySprite) { myColor = mySprite.color; }
		if (myImg) { myColor = myImg.color; }

		if (fadeOnStart) { FadeIn(); myColor = new Color(myColor.r, myColor.g, myColor.b, 0f); }

		// - Assign The Color To What is Appropriate - //
		if (mySprite) { mySprite.color = myColor; }
		if (myImg) { myImg.color = myColor; }
	}



	void Update () 
	{
		if (fadingOut == true)
		{
			t += Time.deltaTime / fadeDuration;
			myColor = new Color(myColor.r, myColor.g, myColor.b, Mathf.SmoothStep(1f, 0f, t));
			if (t >= 1f)
			{
				if (setInactiveOnFadeOut) { this.gameObject.SetActive(false); }
				fadingOut = false;
			}
		}

		if (fadingIn == true)
		{
			t += Time.deltaTime / fadeDuration;
			myColor = new Color(myColor.r, myColor.g, myColor.b, Mathf.SmoothStep(0f, 1f, t));
			if (t >= 1f)
			{
				fadingIn = false;
			}
		}

		// - Assign The Color To What is Appropriate - //
		if (mySprite) { mySprite.color = myColor; }
		if (myImg) { myImg.color = myColor; }
	}



	public void FadeOut ()
	{
		if (fadingOut == false && myColor.a >= 0.01f)
		{
			fadingIn = false;
			fadingOut = true;
			t = 0f;
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
			fadingOut = false;
			fadingIn = true;
			t = 0f;
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
