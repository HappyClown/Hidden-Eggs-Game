using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonGlows : MonoBehaviour {
	public bool alternatingInnerGlows;
	private float cycleThird, cycleTime, fadeInWaitTimer;
	private int startGlow;
	private bool fadingIn, inCycle;
	[Tooltip("Should be equal or higher then the glows's FadeInOutSprite script's fade duration.")]
	public float fadeInWaitTime;
	public float cycleDuration;
	public List<LevelGlow> levelGlowScripts;
	public List<FadeInOutSprite> glowFadeScripts;
	public List<SpriteRenderer> glowSprites;

	void Start () {
		cycleThird = cycleDuration * 0.333f;
	}
	
	void Update () {
		if (fadingIn) {
			fadeInWaitTimer += Time.deltaTime;
			if (fadeInWaitTimer > fadeInWaitTime) {
				fadingIn = false;
				fadeInWaitTimer = 0f;
				inCycle = true;
			}
		}
		if (inCycle) {
			cycleTime += Time.deltaTime;
			if (cycleTime > 0 && startGlow == 0) {
				startGlow++;
				levelGlowScripts[0].StartGlow();
			}
			if (cycleTime > cycleThird && startGlow == 1) {
				startGlow++;
				levelGlowScripts[1].StartGlow();
			}
			if (cycleTime > cycleThird*2 && startGlow == 2) {
				startGlow++;
				levelGlowScripts[2].StartGlow();
			}
			if (cycleTime > cycleDuration) {
				cycleTime = 0f;
				startGlow = 0;
			}
		}
	}

	public void StartLevelGlows() {
		foreach (FadeInOutSprite glowFadeScript in glowFadeScripts)
		{
			glowFadeScript.FadeIn();
		}
		if (alternatingInnerGlows) {
			fadingIn = true;
		}
	}

	public void LevelSelect(int lvlSelected) {
		inCycle = false;
		for (int i = 0; i < levelGlowScripts.Count; i++)
		{
			if (i == lvlSelected) {
				levelGlowScripts[i].StartGlow();
			}
			else {
				levelGlowScripts[i].StopGlow();
			}
		}
	}

	public void ResetGlowAlphas() {
		foreach(SpriteRenderer glowSprite in glowSprites)
		{
			glowSprite.color = new Color(glowSprite.color.r, glowSprite.color.g, glowSprite.color.b, 0);
		}
		foreach(LevelGlow levelGlowScript in levelGlowScripts)
		{
			levelGlowScript.ResetGlow();
		}
		inCycle = false;
		cycleTime = 0f;
		startGlow = 0;
	}
}