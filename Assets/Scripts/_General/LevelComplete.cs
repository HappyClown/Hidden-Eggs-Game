using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelComplete : MonoBehaviour 
{
	public ClickOnEggs clickOnEggsScript;

	private bool inLvlCompSeqSetup;
	private bool inLvlCompSeqEnd;
	private float timer;

	[Header("Setup - When to Play?")]
	public float darkenScreen;
	private bool darkenScreenStarted;
	public float showCongrats;
	private bool showCongratsStarted;
	public float showEggs;
	private bool showEggsStarted;

	[Header("End - When to Play?")]
	public float lightenScreen;
	public float hideCongrats;
	public float hideEggs;

	[Header("Screen Cover")]
	public Image coverScreen;
	public float coverMaxAlpha, coverFadeTime;
	private bool coverOn,coverOff;
	private float coverA;

	[Header("Congratulation Sceen")]
	public SpriteRenderer[] congratsSprts;
	public float congratsFadeTime;
	private bool congratsTxtOn, congratsTxtOff;
	private float congratsA;

	[Header("Eggs")]
	public SpriteRenderer[] eggs;
	public SpriteRenderer regularEgg;
	public SpriteRenderer silverEgg;
	public SpriteRenderer goldenEgg;
	public float eggFadeTime;
	public float eggShowDelay;
	private bool eggsOn, eggsOff;
	private float eggsA;

	[Header("Egg Counters")]
	public float eggCounterSpeed; // do i need one for each
	public int regularEggTot, silverEggTot, goldenEggTot;



	void Update () 
	{
		if (Input.GetKeyDown("space"))
		{
			if (!inLvlCompSeqSetup) 
			{ 
				timer = 0; 
				inLvlCompSeqSetup = true; 
				inLvlCompSeqEnd = false; 
				darkenScreenStarted = false; showCongratsStarted = false;
			}
			else if (inLvlCompSeqSetup)
			{ 
				timer = 0; 
				inLvlCompSeqSetup = false; 
				inLvlCompSeqEnd = true; 
				darkenScreenStarted = false; showCongratsStarted = false;
			}
		}

		if (inLvlCompSeqSetup)
		{
			timer += Time.deltaTime;

			if (timer > darkenScreen && !darkenScreenStarted) { DarkenScreenOnOff();}
			if (timer > showCongrats && !showCongratsStarted) { CongratsOnOff();}
			if (timer > showEggs && !showEggsStarted) { EggsOnOff();}
		}
		else if (inLvlCompSeqEnd)
		{
			timer += Time.deltaTime;

			if (timer > lightenScreen && !darkenScreenStarted) { DarkenScreenOnOff();}
			if (timer > hideCongrats && !showCongratsStarted) { CongratsOnOff();}
			if (timer > hideEggs && !showEggsStarted) { EggsOnOff(); }
		}


		// --- EVENTS TRIGGERED BY TIMER --- //
		// Fade in/out darkened screen.
		if (coverOn)
		{
			coverA += Time.deltaTime / coverFadeTime;
			coverScreen.color = new Color (coverScreen.color.r, coverScreen.color.g, coverScreen.color.b, coverA);
			if (coverA >= coverMaxAlpha) { coverOn = false; }
		}
		if (coverOff)
		{
			coverA -= Time.deltaTime;
			coverScreen.color = new Color (coverScreen.color.r, coverScreen.color.g, coverScreen.color.b, coverA);
			if (coverA <= 0) { coverOff = false; }
		}

		//Fade in the "Congratulations" game objects at the same time.
		if (congratsTxtOn)
		{
			congratsA += Time.deltaTime / congratsFadeTime;
			foreach(SpriteRenderer sprite in congratsSprts) { sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, congratsA); }
			if (congratsA >= 1) { congratsTxtOn = false; }
		}
		if (congratsTxtOff)
		{
			congratsA -= Time.deltaTime / congratsFadeTime;
			foreach(SpriteRenderer sprite in congratsSprts) { sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, congratsA); }
			if (congratsA <= 0) { congratsTxtOff = false; }
		}

		// Fade in regular eggs.
		if (eggsOn)
		{
			eggsA += Time.deltaTime / eggFadeTime;
			regularEgg.color = new Color (regularEgg.color.r, regularEgg.color.g, regularEgg.color.b, eggsA);
			silverEgg.color = new Color (silverEgg.color.r, silverEgg.color.g, silverEgg.color.b, eggsA - eggShowDelay);
			goldenEgg.color = new Color (goldenEgg.color.r, goldenEgg.color.g, goldenEgg.color.b, eggsA - (eggShowDelay * 2));
			if (eggsA <= 0 - (eggShowDelay * 2)) { eggsOn = false; }
		}

			// regular egg count goes up incrementaly
			// silver egg appears 
			// silver egg count "++"
			// golden egg appears
			// golden egg count goes from 0 to 1
			// egg total appears
			// eggs and totals fade out and something to tap appears OR soemthing to tap appears
			// play end sequence, particles & fade to white back to hub
	}


	void DarkenScreenOnOff ()
	{
		if (coverA <= 0)
		{
			darkenScreenStarted = true;
			coverA = 0f;
			coverOn = true;
			coverOff = false;
		}
		else if (coverA >= coverMaxAlpha)
		{
			darkenScreenStarted = true;
			coverA = coverScreen.color.a;
			coverOn = false;
			coverOff = true;
		}
	}

	void CongratsOnOff ()
	{
		if (congratsA <= 0) 
		{ 
			congratsTxtOn = true;
			showCongratsStarted = true;
		}
		else if (congratsA >= 1) 
		{
			congratsTxtOff = true;
			showCongratsStarted = true;
		}
	}

	void EggsOnOff()
	{
		if (eggsA <= 0)
		{
			eggsOn = true;
			showEggsStarted = true;
		}
		else if (eggsA >= 1)
		{
			eggsOff = true;
			showEggsStarted = true;
		}
	}


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
	// 		totalEggsFound = GlobalVariables.globVarScript.marketTotalEggsFound + GlobalVariables.globVarScript.marketSilverEggsCount;
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
}
