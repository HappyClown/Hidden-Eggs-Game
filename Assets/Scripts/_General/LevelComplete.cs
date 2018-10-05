﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelComplete : MonoBehaviour 
{
	#region LevelComplete Script Variables
	private bool inLvlCompSeqSetup;
	private bool inLvlCompSeqEnd;
	private float timer;

	[Header("References")]
	public ClickOnEggs clickOnEggsScript;

	[Header("Setup - When to Play?")]
	public float darkenScreen; public float showCongrats, showEggs, showCounters, showTap; // At what time do the Methods get called.
	private bool darkenScreenStarted, showCongratsStarted, showEggsStarted, showCountersStarted, showTapStarted; // To only run the Methods once.

	[Header("End - When to Play?")]
	public float lightenScreen; public float hideCongrats, hideEggs, hideCounters, hideTap; // Probably not needed, except to facilitate testing.

	[Header("Screen Cover")]
	public float coverMaxAlpha; public float coverFadeTime;
	public Image coverScreen;
	private bool coverOn,coverOff;
	private float coverA;
	
	[Header("Congratulation Screen")]
	public float congratsFadeTime;
	public SpriteRenderer[] congratsSprts;
	private bool congratsTxtOn, congratsTxtOff;
	private float congratsA;

	[Header("Eggs")]
	public float eggFadeTime; public float eggShowDelay;
	public SpriteRenderer regEgg, silEgg, golEgg;
	private bool eggsOn, eggsOff;
	private float eggsA;

	[Header("Egg Counters")]
	public float eggCounterSpeed; public float eggCountWait, eggCountFadeTime, eggCountOffDelay, eggCountOffDelayTimer; // do i need to expose one for each
	public AnimationCurve animCur;
	public int regEggTot, silEggTot, golEggTot; // Total amount in this Level
	public TextMeshProUGUI regEggCounterTxt, silEggCounterTxt, golEggCounterTxt;
	private float regEggVal, silEggVal, golEggVal, regEggDisp, silEggDisp, golEggDisp; // Values for AnimCurve evaluate & Display for the TMP text on screen.
	private bool eggCountersOn, eggCountersOff, regEggCountInc, silEggCountInc, golEggCountInc; // Bools to start Increasing the wait time between each counter going up.
	private float eggCounterA;
	private float regEggWait, silEggWait, golEggWait;

	[Header("Tap-able")]
	public float tapFadeTime;
	public Button tapBtn;
	public Image tapImg;
	private bool tapOn, tapOff, canTap, btnPressed;
	private float tapA;
	#endregion

	void Start ()
	{
		regEggWait = eggCountWait; silEggWait = eggCountWait; golEggWait = eggCountWait;
		tapBtn.onClick.AddListener(TapBtnPress);
	}
	

	void Update () 
	{
		#region Manually Start or End Sequence
		if (Input.GetKeyDown("space"))
		{
			if (!inLvlCompSeqSetup) 
			{ 
				timer = 0; 
				inLvlCompSeqSetup = true; 
				inLvlCompSeqEnd = false; 
				darkenScreenStarted = false; showCongratsStarted = false; showEggsStarted = false; showCountersStarted = false; showTapStarted = false;
			}
			else if (inLvlCompSeqSetup)
			{ 
				timer = 0; 
				inLvlCompSeqSetup = false; 
				inLvlCompSeqEnd = true; 
				darkenScreenStarted = false; showCongratsStarted = false; showEggsStarted = false; showCountersStarted = false; showTapStarted = false;
			}
		}
		#endregion
		
		#region Start/End Level Complete Timer Sequence
		if (inLvlCompSeqSetup)
		{
			timer += Time.deltaTime;

			if (timer > darkenScreen && !darkenScreenStarted) { DarkenScreenOnOff();}
			if (timer > showCongrats && !showCongratsStarted) { CongratsOnOff();}
			if (timer > showEggs && !showEggsStarted) { EggsOnOff();}
			if (timer > showCounters && !showCountersStarted) { EggCountersOnOff();}
			if (timer > showTap && !showTapStarted) { TapOnOff();}
		}
		else if (inLvlCompSeqEnd)
		{
			timer += Time.deltaTime;

			if (timer > lightenScreen && !darkenScreenStarted) { DarkenScreenOnOff();}
			if (timer > hideCongrats && !showCongratsStarted) { CongratsOnOff();}
			if (timer > hideEggs && !showEggsStarted) { EggsOnOff(); }
			if (timer > hideCounters && !showCountersStarted) { EggCountersOnOff();}
			if (timer > hideTap && !showTapStarted) { TapOnOff();}
		}
		#endregion
	
		#region Events Triggered By Methods
		// --- EVENTS TRIGGERED BY METHODS --- //
		// Fade in/out darkened screen.
		if (coverOn)
		{
			coverA += Time.deltaTime / coverFadeTime;
			coverScreen.color = new Color (coverScreen.color.r, coverScreen.color.g, coverScreen.color.b, coverA);
			if (coverA >= coverMaxAlpha) { coverOn = false; coverA = coverMaxAlpha;}
		}
		if (coverOff)
		{
			coverA -= Time.deltaTime;
			coverScreen.color = new Color (coverScreen.color.r, coverScreen.color.g, coverScreen.color.b, coverA);
			if (coverA <= 0) { coverOff = false; coverA = 0;}
		}

		//Fade in the "Congratulations" game objects at the same time.
		if (congratsTxtOn)
		{
			congratsA += Time.deltaTime / congratsFadeTime;
			foreach(SpriteRenderer sprite in congratsSprts) { sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, congratsA); }
			if (congratsA >= 1) { congratsTxtOn = false; congratsA = 1;}
		}
		if (congratsTxtOff)
		{
			congratsA -= Time.deltaTime / congratsFadeTime;
			foreach(SpriteRenderer sprite in congratsSprts) { sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, congratsA); }
			if (congratsA <= 0) { congratsTxtOff = false; congratsA = 0;}
		}

		// Fade in eggs.
		if (eggsOn)
		{
			eggsA += Time.deltaTime / eggFadeTime;
			regEgg.color = new Color (regEgg.color.r, regEgg.color.g, regEgg.color.b, eggsA);
			silEgg.color = new Color (silEgg.color.r, silEgg.color.g, silEgg.color.b, eggsA - eggShowDelay);
			golEgg.color = new Color (golEgg.color.r, golEgg.color.g, golEgg.color.b, eggsA - (eggShowDelay * 2));
			if (eggsA >= 1 + (eggShowDelay * 2)) {eggsOn = false; eggsA = 1 + (eggShowDelay * 2);}
		}
		if (eggsOff)
		{
			eggsA -= Time.deltaTime / eggFadeTime;
			regEgg.color = new Color (regEgg.color.r, regEgg.color.g, regEgg.color.b, eggsA);
			silEgg.color = new Color (silEgg.color.r, silEgg.color.g, silEgg.color.b, eggsA - eggShowDelay);
			golEgg.color = new Color (golEgg.color.r, golEgg.color.g, golEgg.color.b, eggsA - (eggShowDelay * 2));
			if (eggsA <= 0) { eggsOff = false; eggsA = 0;}
		}

		// Fade in egg counters and increment them after x time.
		if (eggCountersOn)
		{
			if (eggCounterA < 1 + (eggShowDelay * 2)) 
			{ 
				eggCounterA += Time.deltaTime / eggCountFadeTime;
				regEggCounterTxt.color = new Color(regEggCounterTxt.color.r, regEggCounterTxt.color.g, regEggCounterTxt.color.b, eggCounterA);
				silEggCounterTxt.color = new Color(silEggCounterTxt.color.r, silEggCounterTxt.color.g, silEggCounterTxt.color.b, eggCounterA - eggShowDelay);
				golEggCounterTxt.color = new Color(golEggCounterTxt.color.r, golEggCounterTxt.color.g, golEggCounterTxt.color.b, eggCounterA - (eggShowDelay * 2));
			}
			else if (eggCounterA >= 1 + (eggShowDelay * 2)) { regEggCountInc = true; eggCounterA = 1 + (eggShowDelay * 2);}

			// Pause for regEggWait amount of time, then increment the reg eggs. --> Pause for silEggWait amount of...
			if(regEggCountInc) {regEggWait -= Time.deltaTime; if (regEggWait <= 0) {regEggWait = 0; regEggCountInc = false;}}
			// We use the Mathf.Floor value compared to the Total because it is the value that is displayed in-game.
			if(Mathf.Floor(regEggDisp) < regEggTot && regEggWait <= 0) 
			{ 
				regEggVal += (Time.deltaTime/eggCounterSpeed); 
				regEggDisp = Mathf.Lerp(0, regEggTot, animCur.Evaluate(regEggVal));
				if (Mathf.Floor(regEggDisp) >= regEggTot) { regEggDisp = regEggTot; silEggCountInc = true; }
				regEggCounterTxt.text = "x" + Mathf.Floor(regEggDisp);
			}
			
			if (silEggCountInc) {silEggWait -= Time.deltaTime; if (silEggWait <= 0) {silEggWait = 0; silEggCountInc = false;}}
			
			if(Mathf.Floor(silEggDisp) < silEggTot && silEggWait <= 0) 
			{
				silEggVal += (Time.deltaTime/eggCounterSpeed); 
				silEggDisp = Mathf.Lerp(0, silEggTot, animCur.Evaluate(silEggVal));
				if (Mathf.Floor(silEggDisp) >= silEggTot) { silEggDisp = silEggTot; golEggCountInc = true;}
				silEggCounterTxt.text = "x" + Mathf.Floor(silEggDisp);
			}

			if(golEggCountInc) {golEggWait -= Time.deltaTime; if (golEggWait <= 0) {golEggWait = 0; golEggCountInc = false;}}

			if (golEggVal < golEggTot && golEggWait <= 0) { golEggVal += 1; eggCountersOn = false;}
			golEggCounterTxt.text = "x" + Mathf.Floor(golEggVal);
		}
		if (eggCountersOff)
		{
			if (eggCountOffDelayTimer < eggCountOffDelay) { eggCountOffDelayTimer += Time.deltaTime;}
			else 
			{
				eggCounterA -= Time.deltaTime / eggCountFadeTime;
				regEggCounterTxt.color = new Color(regEggCounterTxt.color.r, regEggCounterTxt.color.g, regEggCounterTxt.color.b, eggCounterA);
				silEggCounterTxt.color = new Color(silEggCounterTxt.color.r, silEggCounterTxt.color.g, silEggCounterTxt.color.b, eggCounterA - eggShowDelay);
				golEggCounterTxt.color = new Color(golEggCounterTxt.color.r, golEggCounterTxt.color.g, golEggCounterTxt.color.b, eggCounterA - (eggShowDelay * 2));
				if (eggCounterA <= 0) 
				{ 
					eggCounterA = 0f;
					regEggCountInc = false; silEggCountInc = false; golEggCountInc = false;
					regEggCounterTxt.text = "x0"; silEggCounterTxt.text = "x0"; golEggCounterTxt.text = "x0";
					regEggWait = eggCountWait; silEggWait = eggCountWait; golEggWait = eggCountWait;
					regEggVal = 0f; silEggVal = 0f; golEggVal = 0f; regEggDisp = 0f; silEggDisp = 0f; golEggDisp = 0f; // golEggDisp is never used.
					eggCountOffDelayTimer = 0f;
					eggCountersOff = false;
				}
			}
		}

		// Fade in the tap-able button.
		if (tapOn)
		{
			if (eggsA <= 0f && eggCounterA <= 0f && tapA < 1f)
			{
				tapA += Time.deltaTime / tapFadeTime;
				tapImg.color = new Color(tapImg.color.r, tapImg.color.g, tapImg.color.b, tapA);
				if (tapA >= 1f) { tapA = 1f; tapBtn.interactable = true; } // If Tap-able button is fully visible make it interactable.
			}

			if (btnPressed)
			{
				tapBtn.interactable = false;
				SceneFade.SwitchSceneWhiteFade(GlobalVariables.globVarScript.menuName);
			}
		}
		if (tapOff)
		{
			if (tapBtn.interactable) { tapBtn.interactable = false;}
			if (tapA > 0f)
			{
				
				tapA -= Time.deltaTime / tapFadeTime;
				tapImg.color = new Color(tapImg.color.r, tapImg.color.g, tapImg.color.b, tapA);
				if (tapA <= 0f) { tapA = 0f; }
			}
		}
		#endregion
	}

	#region Methods Triggered By Timer
	// --- METHODS TRIGGERED BY TIMER --- //
	void DarkenScreenOnOff ()
	{
		if (coverA <= 0)
		{
			darkenScreenStarted = true;
			coverOn = true;
			coverOff = false;
		}
		else if (coverA >= coverMaxAlpha)
		{
			darkenScreenStarted = true;
			coverOn = false;
			coverOff = true;
		}
	}

	// void LightenScreen ()
	// {
	// 	coverA = coverScreen.color.a;
	// 	coverOff = true;
	// 	coverOn = false;
	// }

	void CongratsOnOff ()
	{
		if (congratsA <= 0) 
		{ 
			showCongratsStarted = true;
			congratsTxtOn = true;
			congratsTxtOff = false;
		}
		else if (congratsA >= 1) 
		{
			showCongratsStarted = true;
			congratsTxtOn = false;
			congratsTxtOff = true;
		}
	}

	void EggsOnOff()
	{
		if (eggsA <= 0)
		{
			showEggsStarted = true;
			eggsOn = true;
			eggsOff = false;
		}
		else if (eggsA >= 1)
		{
			showEggsStarted = true;
			eggsOn = false;
			eggsOff = true;
		}
	}

	void EggCountersOnOff()
	{
		if (eggCounterA <= 0)
		{
			showCountersStarted = true;
			eggCountersOn = true;
			eggCountersOff = false;
		}
		else if (eggCounterA >= 1)
		{
			showCountersStarted = true;
			eggCountersOn = false;
			eggCountersOff = true;
		}
	}

	void TapOnOff()
	{
		if (tapA <= 0)
		{
			showTapStarted = true;
			tapOn = true;
			tapOff = false;
			EggsOnOff(); EggCountersOnOff();
		}
		else if (tapA >= 1)
		{
			showTapStarted = true;
			tapOn = false;
			tapOff = true;
			eggsA = 1; eggCounterA = 1;
		}
	}

	void TapBtnPress()
	{
		Debug.Log("Level Completuruuuu! You go noew.");
		btnPressed = true;
		// play some FX
		// Fade to white
		// wait a certain amount of time OR on screen fully white, then load menu scene at the hub
	}
	#endregion

	#region OLD, To Be Terminated On Script Completion

	// EGG INCREMENTATION WITHOUT LERP OR ANIM CURVE (Linear) //
	// if(regEggVal < regEggTot && regEggWait <= 0) { regEggVal += (Time.deltaTime/eggCounterSpeed) * regEggTot;}
	// else if (regEggVal >= regEggTot) { regEggVal = regEggTot; silEggWait -= Time.deltaTime;}
	// regEggCounterTxt.text = "x" + Mathf.Round(regEggVal);


	// public void CalculateTotalEggsFound()
	// {
	// 	for(int i = 0; i < GlobalVariables.globVarScript.marketEggsFoundBools.Count; i ++)
	// 	{
	// 		if (GlobalVariables.globVarScript.marketEggsFoundBools[i] == true)
	// 		{
	// 			totalEggsFound += 1;
	// 		}
	// 	}
	// }



	// public void UpdateTotalEggsFound()
	// {
	// 	if (SceneManager.GetActiveScene().name == "Market")
	// 	{
	// 		totalEggsFound = GlobalVariables.globVarScript.marketTotalEggsFound + GlobalVariables.globVarScript.marketsilEggsCount;
	// 	}

	// 	if (SceneManager.GetActiveScene().name == "Park")
	// 	{
	// 		totalEggsFound = GlobalVariables.globVarScript.parkTotalEggsFound;
	// 	}

	// 	if (SceneManager.GetActiveScene().name == "Beach")
	// 	{
	// 		totalEggsFound = GlobalVariables.globVarScript.beachTotalEggsFound;
	// 	}
		
	// }
	#endregion 
}
