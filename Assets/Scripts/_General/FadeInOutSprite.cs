using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOutSprite : MonoBehaviour {
	//[HideInInspector]
	public bool fadingOut, fadingIn, hidden, shown;
	private float t;
	public float fadeDelayDur;
	[Range(0f, 1f)]
	public float maxAlpha = 1f;
	public float fadeDuration;
	public bool fadeOnStart = true;
	public bool fadeDelay, disableOnFadeOut;
	public SpriteRenderer sprite;
	public enum StartState {
		startShown, startHidden
	}
	public StartState myStartState;

	void Start () {
		sprite = this.gameObject.GetComponent<SpriteRenderer>();
		if (myStartState == StartState.startShown) {
			sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f);
			shown = true;
		}
		else if (myStartState == StartState.startHidden) {
			sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0f);
			hidden = true;
		}
		if (maxAlpha <= 0f) { 
			maxAlpha = 1f; 
		}
		if (fadeOnStart) { 
			FadeIn();  
			new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0f); 
		}
	}

	void Update () {
		if (fadingOut == true) {
			t += Time.deltaTime / fadeDuration;
			sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, Mathf.SmoothStep(1f * maxAlpha, 0f, t));
			if (t >= 1f) {
				fadingOut = false;
				hidden = true;
				if(shown)
				shown = false;				
				if(disableOnFadeOut) {
					this.gameObject.SetActive(false);
				}
			}
		}

		if (fadingIn == true) {
			t += Time.deltaTime / fadeDuration;
			sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, Mathf.SmoothStep(0f, 1f * maxAlpha, t));
			if (t >= 1f) {
				shown = true;
				fadingIn = false;
				if(hidden)
				hidden = false;
			}
		}
	}

	public void FadeOut () {
		if (fadingOut == false) { // Potentially implement a waitmode, to wait until it is faded in/out to fade it in/out.
			fadingIn = false;
			fadingOut = true;
			if(fadeDelay) { t = 0f - fadeDelayDur; }
			else { t = 0f; }
			shown = false;
		}
	}

	public void FadeIn () {
		if (!this.gameObject.activeSelf) {
			this.gameObject.SetActive(true);
		}
		if (fadingIn == false) {
			fadingOut = false;
			fadingIn = true;
			if(fadeDelay) { t = 0f - fadeDelayDur; }
			else { t = 0f; }
			hidden = false;
		}
	}
	public void ResetAplpha(float value){
		sprite.color = new Color(1f, 1f, 1f, value);
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
