﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeachClam : MonoBehaviour {
	public BeachClam myMatch;
	public FadeInOutSprite myOpenClam;
	public FadeInOutSprite myClosedClam;
	public Animator clamAnim;
	public GameObject ClamSpriteParent;
	public BeachBubbles[] myBubbles;
	private CircleCollider2D myCollider;
	[Tooltip("Time before clam dissapears after match")]
	public float timeDelay;
	private float timer;
	public bool Tapped, open, matched, failed, closed, forceClose;

	public bool clamWaiting;
	private bool setFadeDurToPlay;
	public float clamUpDelay;
	private float showClamTimer;
	public float iniFadeInDur, playFadeInDur;


	//tests for sounds
	public AudioSceneBeachPuzzle audioBeachPuzzleScript;
	public string clamSound;
	//

	void Start () {
		myCollider = this.gameObject.GetComponent<CircleCollider2D>();
		Tapped = open = matched = failed = forceClose =  false;
		closed = true;
		timer = 0;

		//snd
		audioBeachPuzzleScript =  GameObject.Find ("Audio").GetComponent<AudioSceneBeachPuzzle>();
	}
	
	void Update () {
		if(Tapped){
			if(closed){
				//clam sound
				audioBeachPuzzleScript.playOceanSound(clamSound);

				myCollider.enabled = false;
				open = true;
				closed = false;
				myOpenClam.fadeDelay = false;
				myOpenClam.FadeIn();
				myClosedClam.fadeDelay = true;
				myClosedClam.FadeOut();

				foreach (BeachBubbles bubbles in myBubbles)
				{
					bubbles.ResetBubble();
					bubbles.activeClam = true;
				}
				Tapped = false;
			}
		}
		if(failed){
			if(closed){
				myCollider.enabled = true;
				myClosedClam.fadeDelay = false;
				myClosedClam.FadeIn();
				myOpenClam.fadeDelay = true;
				myOpenClam.FadeOut();
				failed = false;
				
			}
			if(open /* && put delay for sound*/|| forceClose){
				open = false;
				closed = true;
				forceClose = false;

				//failed match sound .. should put a delay or something
				//audioBeachPuzzleScript.failSFX();
			}
		}
		if(open && myMatch.matched){
			timer += Time.deltaTime;
			if(timer >= timeDelay  && open){
				myOpenClam.FadeOut();
				matched = true;
				open = false;

				//"matched" and "dissolve" sound
				audioBeachPuzzleScript.BubblesSFX();
				audioBeachPuzzleScript.addToMusicList(clamSound);
			}
		}
	
		if (setFadeDurToPlay) {
			showClamTimer += Time.deltaTime;
			if (showClamTimer >= clamUpDelay && clamWaiting) {
				clamAnim.SetTrigger("ShowClam");
				myClosedClam.FadeIn();
				clamWaiting = false;

				//sound clam pop
				audioBeachPuzzleScript.clamPopOutSFX();
			}
			if (showClamTimer >= (clamUpDelay + iniFadeInDur) && setFadeDurToPlay) {
				myClosedClam.fadeDuration = playFadeInDur;
				showClamTimer = 0f;
				setFadeDurToPlay = false;
			}
		}
	}
	public void ResetClams(){
		myCollider = this.gameObject.GetComponent<CircleCollider2D>();
		myCollider.enabled = true;
		Tapped = open = matched = failed =  false;
		closed = true;
		timer = 0;
			myClosedClam.fadeDelay = false;
			//myClosedClam.FadeIn();
		if(myOpenClam.shown){
			myOpenClam.fadeDelay = true;
			myOpenClam.FadeOut();
		}
		
		foreach (BeachBubbles bubbles in myBubbles)
		{
			bubbles.activeClam = false;
		}
	}
	public void CleanBubbles(){
		foreach (BeachBubbles bubbles in myBubbles)
		{
			bubbles.ResetBubble();
		}
	}

	public void ShowClams() {
		clamWaiting = true;
		setFadeDurToPlay = true;
		myClosedClam.fadeDuration = iniFadeInDur;
	}
}