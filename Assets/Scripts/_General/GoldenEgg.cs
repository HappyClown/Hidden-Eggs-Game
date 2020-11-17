﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GoldenEgg : MonoBehaviour {
	[HideInInspector]
	public bool inGoldenEggSequence, waitingToStartSeq;
	private bool inSendingToCorner;
	private float seqTimer;
	public PolygonCollider2D goldenEggCollider;
	[Header ("Sequence")]
	public float startCover;
	public float startText, startEgg, startFireWorks;
	private bool coverB, textB, eggB, fireWorksB;

	[Header("Egg Animation")]
	public Animator anim;
	//public float eggAnimStart;
	//private float eggAnimTimer;
	private bool eggAnimStarted;

	[Header("Screen Cover")]
	public Image coverScreen;
	public float coverMaxAlpha;
	private float coverAlpha;
	private bool coverOn, coverOff;

	[Header("Congratulations")]
	public Animator titleAnim;
	public TMPTextColorFade textFadeScript;
	public List<FadeInOutSprite> starFadeScripts;
	public TMPWarpText textWarpScript;
	public ParticleSystem textFX;
	public SplineWalker textSplineWScript;
	//public SpriteRenderer[] congratsObjs;
	//public float congratsTime, congratsFadeTime;
	private bool congratsTxtOn, congratsTxtOff;
	private float congratsA;

	[Header("Particles")]
	public ParticleSystem partGlow;
	public ParticleSystem partShafts, partSparkles, partPop, partTrail;
	private float partGlowA, partGlowMaxAplha;
	private float partShaftsA, partShaftsMatA;
	private Material partShaftsMat;
	public float partShaftsFadeTime, partShaftsShrinkTime;
	private bool partShaftsFade;
	public ParticleSystem firework01, firework02;
	private float partTrailSize;

	[Header("After Tap Sequence")]
	[TooltipAttribute("The time it takes in seconds before starting this sequence after the GoldenEgg has been tapped.")]
	public float eggToCornerTime;
	[TooltipAttribute("The time it takes in seconds before starting this sequence after the GoldenEgg has been tapped.")]
	public float congratsOffTime, coverOffTime;
	private float eggToCornerTimer;

	private bool clickDown;
	private RaycastHit2D hit;

	[Header("Script References")]
	public LevelTapMannager lvlTapManScript;
	public EggGoToCorner eggGoToCornerScript;
	public ClickOnEggs clickOnEggsScript;
	public SceneTapEnabler scenTapEnabScript;
	//public AudioSceneGeneral audioSceneGeneralScript;

	public AudioRiddleSolvedAnim audioRiddleSolvedAnimScript;

	void Start () {
		partShaftsMat = partShafts.gameObject.GetComponent<ParticleSystemRenderer>().material;
		//  if (eggGoToCornerScript.eggFound) { this.transform.localScale += new Vector3(4, 4, 1); }
		//  Debug.Log("EGG FOUND? :" + eggGoToCornerScript.eggFound);
		audioRiddleSolvedAnimScript = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioRiddleSolvedAnim>();
	}

	void Update () {
		// Wait until no other sequences are playing to start the Golden Egg sequence.
		if (waitingToStartSeq && !ClickOnEggs.inASequence) {
			inGoldenEggSequence = true;
			// In a sequence.
			ClickOnEggs.inASequence = true;
			waitingToStartSeq = false;
			lvlTapManScript.ZoomOutCameraReset();
		}

		// -- START GOLDEN EGG SEQUENCE -- //
		if (inGoldenEggSequence) {
			seqTimer += Time.deltaTime;
			// Set the GoldenEgg as found and save
			if (!eggGoToCornerScript.eggFound) {
				eggGoToCornerScript.eggFound = true;
				eggGoToCornerScript.SaveEggToCorrectFile();
			}
			// Start things according to the sequence timer.
			if (seqTimer >= startCover && !coverB) { 
				DarkenScreen();
				coverB = true;
			}
			if (seqTimer >= startText && !textB) {
				titleAnim.SetTrigger("PopIn");
				textFadeScript.startFadeIn = true;
				textB = true;

				//sound text pop
				audioRiddleSolvedAnimScript.riddleSolvedTextSnd();

			}
			if (seqTimer >= startFireWorks && !fireWorksB) {
				firework01.Play(true);
				firework02.Play(true);
				fireWorksB = true;

				//explosion sounds tests
				audioRiddleSolvedAnimScript.fireworkTrailBurstSnd();
			}
			if (seqTimer >= startEgg && !eggB) {
				StartEgg();
				eggB = true;
			}

			// Fade in the Glow up to its desired value over 1 second (+= Time.deltaTime * partGlowMaxAplha & alpha's max value is 1)
			if (partGlow.isPlaying) {
				var main = partGlow.main;
				if (partGlowMaxAplha <= 0) { 
					partGlowMaxAplha = main.startColor.color.a; 
				}

				if (partGlowA < partGlowMaxAplha) {
					partGlowA += Time.deltaTime * partGlowMaxAplha;
					main.startColor = new Color (main.startColor.color.r, main.startColor.color.g, main.startColor.color.b, partGlowA);
				}
			} 
			else { 
				partGlowA = 0; partGlowMaxAplha = 0; 
			}
			// Fade in the shafts over 1 second (+= Time.deltaTime & alpha's max value is 1)
			if (partShafts.isPlaying) {
				if (partShaftsA < 1) {
					partShaftsA += Time.deltaTime;
					var main = partShafts.main;
					main.startColor = new Color (main.startColor.color.r, main.startColor.color.g, main.startColor.color.b, partShaftsA);
				}
			} 
			else { 
				partShaftsA = 0; 
			}
			// Increase the trail's size when the egg swoops in
			if (partTrail.isPlaying) {
				partTrailSize += Time.deltaTime;
				var main = partTrail.main;
				main.startSizeMultiplier = partTrailSize;
			} 
			else { 
				partTrailSize = 0; 
			}
		}

		// After the player taps the Golden Egg, timeline sequence
		if (inSendingToCorner) {
			eggToCornerTimer += Time.deltaTime;
			if (eggToCornerTimer >= coverOffTime) { LightenScreen(); }
			if (eggToCornerTimer >= congratsOffTime) { 
				textFadeScript.startFadeOut = true; 
				foreach( FadeInOutSprite starFadeScript in starFadeScripts)
				{
					starFadeScript.FadeOut();
				}
			}
			if (eggToCornerTimer >= eggToCornerTime) { eggGoToCornerScript.GoToCorner(); clickOnEggsScript.eggMoving += 1; clickOnEggsScript.openEggPanel = true; }

			if (eggToCornerTimer > coverOffTime && eggToCornerTimer > congratsOffTime && eggToCornerTimer > eggToCornerTime) { 
				inSendingToCorner = false;
				// Sequence finished.
				ClickOnEggs.inASequence = false;
			}
		}

		// Fade in darkened screen.
		if (coverOn) {
			coverAlpha += Time.deltaTime;
			coverScreen.color = new Color (coverScreen.color.r, coverScreen.color.g, coverScreen.color.b, coverAlpha);
			if (coverAlpha >= coverMaxAlpha) { coverOn = false; }
		}
		// Fade out darkened screen.
		if (coverOff) {
			coverAlpha -= Time.deltaTime;
			coverScreen.color = new Color (coverScreen.color.r, coverScreen.color.g, coverScreen.color.b, coverAlpha);
			if (coverAlpha <= 0) { coverOff = false; }
		}

		// Trying to get rid of the shaft particles in a nice way
		if (partShaftsFade) {
			float shaftX = partShafts.transform.localScale.x;
			float shaftZ = partShafts.transform.localScale.z;
			shaftX -= Time.deltaTime * partShaftsShrinkTime;
			shaftZ -= Time.deltaTime * partShaftsShrinkTime;
			partShafts.transform.localScale = new Vector3(shaftX, 1, shaftZ);
			if (shaftX <= 0 || shaftZ <= 0) { 
				partShaftsFade = false; 
				partShafts.gameObject.SetActive(false);
			}
		}

		// Set the GoldenEgg scale to 4 (its original scale) in x & y.
		if (eggGoToCornerScript.eggFound && !inGoldenEggSequence && this.transform.localScale.x != 4) {
			this.transform.localScale += new Vector3(4 - this.transform.localScale.x, 4 - this.transform.localScale.y, 0);
		}
	}

	void DarkenScreen () {
		coverAlpha = 0f;
		coverOn = true;
	}

	void LightenScreen () {
		coverAlpha = coverScreen.color.a;
		coverOff = true;
		coverOn = false;
	}

	void TextOnOff () {
		if (congratsA <= 0) congratsTxtOn = true;
		else if (congratsA >= 1) congratsTxtOff = true;
	}

	void StartEgg () {
		anim.SetTrigger("StartAnim");
		eggAnimStarted = true;
	}

	// - CALLED DURING ANIMATIONS - // (Animation Events)
	#region Animation Events
	void StartStopGlow () {
		if (!partGlow.gameObject.activeSelf && !partGlow.isPlaying) { partGlow.gameObject.SetActive(true); partGlow.Play(true); }
		else { partGlow.gameObject.SetActive(false); partGlow.Stop(true); }
	}

	void StartStopShafts() {
		if (!partShafts.gameObject.activeSelf && !partShafts.isPlaying) { partShafts.gameObject.SetActive(true); partShafts.Play(true); }
		else { partShafts.Stop(true); partShaftsFade = true; }
	}

	void StartStopSparkles() {
		if (!partSparkles.gameObject.activeSelf && !partSparkles.isPlaying) { partSparkles.gameObject.SetActive(true); partSparkles.Play(true); }
		else { partSparkles.gameObject.SetActive(false); partSparkles.Stop(true); }
	}

	void StartPop() {
		if (!partPop.isPlaying) { partPop.Play(true); }
	}

	void StartTrail() {
		if (!partTrail.isPlaying) { partTrail.Play(true); }
	}

	void ActivateCollider() {
		goldenEggCollider.enabled = true;
	}

	void SendEggToCornerSequence() {
		//CanTaps();
		inSendingToCorner = true;
	}

	void ClickFX() {
		eggGoToCornerScript.PlayEggClickFX();
	}

	void StopAnim () {
		anim.enabled = false;
	}

	public void CanTapGold () {
		scenTapEnabScript.canTapGoldEgg = true;
	}

	public void CannotTaps() {
		scenTapEnabScript.canTapEggRidPanPuz = false;
		scenTapEnabScript.canTapPauseBtn = false;
		scenTapEnabScript.canTapHelpBird = false;
	}

	public void CanTaps() {
		scenTapEnabScript.canTapEggRidPanPuz = true;
		scenTapEnabScript.canTapPauseBtn = true;
		scenTapEnabScript.canTapHelpBird = true;
	}

	public void GoldEggAnimSound() {
		audioRiddleSolvedAnimScript.goldenEggIdleSnd();
	}

	public void GoldEggShimmerPlaySound() {
		audioRiddleSolvedAnimScript.goldenEggIdleSnd();
	}
	#endregion
}