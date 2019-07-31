using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleteBagGlow : MonoBehaviour {
	public float alphaSpeed;
	public float startAlpha;
	public List<FadeInOutSprite> bagGlowsScripts;
	public LevelCompleteEggSpawner lvlCompSpawnerScript;
	public LevelCompleteEggBag lvlCompEggBagScript;
	public LevelCompEggCounter lvlCompEggCounterScript;
	private float curAlpha, newAlpha;
	private float totalValue, curValue;
	public SpriteRenderer curGlow;
	public FadeInOutSprite curGlowScript;
	public int regValue, silValue, golValue;
	private int levelsCompleted;
	private bool glowFade;

	void Start () {
		//CalculateTotal();
	}
	
	void Update () {
		if (curAlpha != newAlpha) {
			curAlpha = Mathf.MoveTowards(curAlpha, newAlpha, alphaSpeed * Time.deltaTime);
			curGlow.color = new Color(curGlow.color.r, curGlow.color.g, curGlow.color.b, curAlpha);
		}

		if (lvlCompEggCounterScript.eggAmnt >= 1 && !glowFade) {
			
			glowFade = true;
		}
	}

	public void StartCurrentBagGlow() {
		levelsCompleted = GlobalVariables.globVarScript.levelsCompleted;
		curGlowScript = bagGlowsScripts[levelsCompleted];
		curGlowScript.gameObject.SetActive(true);
		curGlowScript.FadeIn();
	}


	public void CalculateNewAlpha(int value) {
		curValue += value;
		newAlpha = (curValue / totalValue) + startAlpha;
	}

	public void CalculateTotal() {
		// totalValue = regValue * lvlCompSpawnerScript.regEggs.Count + silValue * lvlCompSpawnerScript.silEggs.Count + golValue * lvlCompSpawnerScript.golEggs.Count;
		totalValue = lvlCompSpawnerScript.allEggs.Count;
	}
}
