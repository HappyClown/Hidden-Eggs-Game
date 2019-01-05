﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeachBubbles : MonoBehaviour {

	[Range(0.1f,1f)]
	public float bubbleSize = 1f;
	[Range(1f,7f)]
	public float Speed;
	[Range(1f,10f)]
	public float curveMultiplier = 1;
	[Range(0f,2f)]
	public float lifeTimedelay;
	public AnimationCurve curveTrail;
	public FadeInOutSprite myFade;
	public float lifeTime;
	public bool activeClam;
	private bool activeSprite;
	private bool fadeInOutSprite;
	private float currentTime;
	private Vector3 StartPosition;
	private SpriteRenderer mySprite;
	// Use this for initialization
	void Start () {
		activeClam = false;
		gameObject.transform.localScale = gameObject.transform.localScale * (bubbleSize);
		StartPosition = gameObject.transform.localPosition;
		currentTime = 0;
		mySprite = this.gameObject.GetComponent<SpriteRenderer>();
		myFade = this.gameObject.GetComponent<FadeInOutSprite>();
		mySprite.enabled = false;
		activeSprite = false;
		fadeInOutSprite = false;
		myFade.FadeOut();	
	}
	// Update is called once per frame
	void Update () {
		if(activeClam && !activeSprite){
			currentTime += Time.deltaTime;
			if(currentTime > lifeTimedelay){
				mySprite.enabled = true;
				activeSprite = true;
				myFade.FadeIn();
			}
		}
		if(activeSprite){
			currentTime += Time.deltaTime;
			if(currentTime < (lifeTime+lifeTimedelay) && currentTime > lifeTimedelay){
				float yPos = gameObject.transform.localPosition.y + (Time.deltaTime*Speed);
				float xPos = curveTrail.Evaluate(currentTime + lifeTimedelay)*curveMultiplier;
				Vector3 newPos = new Vector3(xPos,yPos,gameObject.transform.localPosition.z);
				gameObject.transform.localPosition = newPos;
			}
			else{
				if(!fadeInOutSprite){
					myFade.FadeOut();
					fadeInOutSprite = true;
				}
				currentTime = 0;
				mySprite.enabled = false;
				gameObject.transform.localPosition =StartPosition;
				activeSprite = false;
				activeClam = false;
			}
		}
	}
	public void ResetBubble(){
		currentTime = 0;
		gameObject.transform.localPosition = StartPosition;
		activeSprite = false;
		mySprite.enabled = false;
	}
}
