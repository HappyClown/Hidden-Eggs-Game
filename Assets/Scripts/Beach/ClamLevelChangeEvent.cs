using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClamLevelChangeEvent : MonoBehaviour {
	private float endEventTimer;
	public SpriteRenderer bootFront;
	public List<ParticleSystem> sandPartSys;
	public bool endEventOn;
	public float stuffFadeOutF, animStartF, activateEggsF, changeBootOrderF, finished;
	private bool stuffFadeOutB, activateEggsB, changeBootOrderB;
	public bool animStartB;
	public Animator bootAnim;
	public ClamPuzzle clamPuzzleScript;
	public SilverEggsManager silverEggManScript;

	void Update () {
		if (endEventOn) {
			endEventTimer += Time.deltaTime;
			// Fade out the level stuff.
			if (endEventTimer >= stuffFadeOutF && !stuffFadeOutB) {
				stuffFadeOutB = true;
				clamPuzzleScript.LvlStuffFadeOut();
			}
			// Start the boot anim.
			if (endEventTimer >= animStartF && !animStartB) {
				bootAnim.SetTrigger("BootShake");
				foreach (ParticleSystem sandPart in sandPartSys) 
				{
					sandPart.GetComponent<Renderer>().sortingLayerName = "SilverEgg";
				}
				bootFront.sortingLayerName = "SilverEgg";
				animStartB = true;
			}
			if (endEventTimer >= activateEggsF && !activateEggsB) {
				activateEggsB = true;
				clamPuzzleScript.scrnDarkImgScript.FadeIn();
				foreach(GameObject silEgg in silverEggManScript.activeSilverEggs)
				{
					silEgg.GetComponent<SilverEggSequence>().StartSequence();
				}
			}
			// Put the boot and the sand particle FX back on the default layer, so that the SilverEggs and the level stuff go in front of the boot.
			if (endEventTimer >= changeBootOrderF && !changeBootOrderB) {
				foreach (ParticleSystem sandPart in sandPartSys) 
				{
					sandPart.GetComponent<Renderer>().sortingLayerName = "Default";
				}
				bootFront.sortingLayerName = "Default";
				if (bootFront.sortingOrder != 0) {
					bootFront.sortingOrder = 0;
				}
				changeBootOrderB = true;
			}
			// Reset all the variables.
			if (endEventTimer >= finished) {
				endEventOn = false;
				endEventTimer = 0f;
				stuffFadeOutB = false;
				animStartB = false;
				activateEggsB = false;
				changeBootOrderB = false;
			}
			// If the silver egg sequence was skipped, end the boot anim and reset all the variables.
			if (silverEggManScript.skippedSeq) {
				bootAnim.SetTrigger("StopBootAnim");
				endEventOn = false;
				endEventTimer = 0f;
				stuffFadeOutB = false;
				animStartB = false;
				activateEggsB = false;
				changeBootOrderB = false;
				foreach (ParticleSystem sandPart in sandPartSys) 
				{
					sandPart.GetComponent<Renderer>().sortingLayerName = "Default";
				}
				bootFront.sortingLayerName = "Default";
				if (bootFront.sortingOrder != 0) {
					bootFront.sortingOrder = 0;
				}
			}
		}
	}

	public void LevelChangeEvent() {
		endEventOn = true;
		bootFront.sortingLayerName = "SilverEgg";
		foreach (ParticleSystem sandPart in sandPartSys) 
		{
			sandPart.GetComponent<Renderer>().sortingLayerName = "SilverEgg";
		}
		if (bootFront.sortingOrder != 1) {
			bootFront.sortingOrder = 1;
		}
		//Debug.Log("Hello, LevelChangeEvent speaking.");
	}
}
