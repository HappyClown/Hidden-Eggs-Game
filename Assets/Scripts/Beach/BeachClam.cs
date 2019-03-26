﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeachClam : MonoBehaviour {

	// Use this for initialization
	public BeachClam myMatch;
	public FadeInOutSprite myOpenClam;
	public FadeInOutSprite myClosedClam;
	public GameObject ClamSpriteParent;
	public BeachBubbles[] myBubbles;
	private CircleCollider2D myCollider;
	[Tooltip("Time before clam dissapears after match")]
	public float timeDelay;
	private float timer;
	public bool Tapped, open, matched, failed, closed, forceClose;


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
	
	// Update is called once per frame
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
	}
	public void ResetClams(){
		myCollider = this.gameObject.GetComponent<CircleCollider2D>();
		myCollider.enabled = true;
		Tapped = open = matched = failed =  false;
		closed = true;
		timer = 0;
			myClosedClam.fadeDelay = false;
			myClosedClam.FadeIn();
		if(myOpenClam.shown){
			myOpenClam.fadeDelay = true;
			myOpenClam.FadeOut();
		}
		
		foreach (BeachBubbles bubbles in myBubbles)
		{
			bubbles.activeClam = false;
		}
	}
}
