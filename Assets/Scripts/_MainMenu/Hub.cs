using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hub : MonoBehaviour {
	public float hubActiveWait;
	private float hubActiveWaitTimer;
	public float hubActiveFaster = 0f;
	public List<FadeInOutCanvasGroup> hubCanvasGroupFadeScripts;
	public FadeInOutCanvasGroup hubCGInteractFadeScript;
	public FadeInOutCanvasGroup hubCGUninteractFadeScript;
	[Header("Dissolve")]
	public float dissAmnt;
	public float dissSpeed;
	public List<Material> dissolveMats;
	public List<Material> matsToDissolve;
	public bool dissolveDone;
	[Header("Season Objects")]
	public List<GameObject> summerButtons;
	public List<GameObject> summerGlows;
	public List<SeasonGlows> seasonGlowsScripts;
	public List<GameObject> fallObjs;
	[Header("What To Do Bools")]
	public bool dissolving;
	public bool inHub;
	public bool startHubActive;
	[Header("References")]
	public DissolveSeasons dissolveSeasonsScript;
	public BackToMenu backToMenuScript;
	public EdgeFireflies edgeFirefliesScript;
	
	void Start () {
		ResetHubSeasons();
		hubActiveWaitTimer = hubActiveWait;
	}

	void Update () {
		// - Start Countdown Timer before Dissolves - //
		if (startHubActive) {
			if (hubActiveWaitTimer == hubActiveWait) {
				dissolveSeasonsScript.SeasonDissolveCheck(); 
				DecideDissolve();
				hubActiveWaitTimer -= hubActiveFaster;
			}
			hubActiveWaitTimer -= Time.deltaTime;
		}
		// - Start Dissolving Unlocked Seasons - //
		if(hubActiveWaitTimer <= 0f && !inHub) {
			startHubActive = false;
			hubActiveWaitTimer = hubActiveWait;
			inHub = true;
			dissolving = true;
			hubActiveFaster = 0f;
		}
		// - Dissolve All Seasons - //
		if (dissolving && inHub) {
			if (dissAmnt < 1.01f && matsToDissolve.Count > 0) {
				dissAmnt += Time.deltaTime * dissSpeed;
				foreach(Material matToDiss in matsToDissolve)
				{
					matToDiss.SetFloat ("_Threshold", dissAmnt);
				}
			}
			else {
				dissolveSeasonsScript.SaveSeasonDissolves();
				//EnableSeasonObjs();
				matsToDissolve.Clear();
				dissolving = false;
				dissolveDone = true;
				EnableHubObjects();
			}
		}
	}

	void EnableHubObjects() { // Enable general Village objects (UI, etc) 
		//backToMenuScript.backToMenuFadeScript.FadeIn();
		//backToMenuScript.backToMenuIconFadeScript.FadeIn();
		hubCGInteractFadeScript.FadeIn();
		hubCGUninteractFadeScript.FadeIn();
		// foreach (FadeInOutCanvasGroup hubCanvasGroupFadeScript in hubCanvasGroupFadeScripts)
		// {
		// 	hubCanvasGroupFadeScript.FadeIn();
		// }
	}

	public void EnableSeasonObjs() { // Enable Season objects (Scene buttons) 
		backToMenuScript.backToMenuBtn.enabled = true;
		// foreach true...
		if (GlobalVariables.globVarScript.dissSeasonsBools[0]) {
			foreach(GameObject summerObj in summerButtons)
			{
				if (!summerObj.activeSelf) { summerObj.SetActive(true); }
			}
			seasonGlowsScripts[0].StartLevelGlows();
			//edgeFirefliesScript.StartFireflyFX();
		}
		// ...
	}
	
	void DecideDissolve () { // Decide which seasons to dissolve or have already colored
		// If a season hasnt fully dissolved once and the player has enough eggs dissSeason[i] will be true and its corresponding material will be added to the dissolve list.
		for (int i = 0; i < dissolveSeasonsScript.dissSeasonsTemp.Count; i++)
		{
			if (dissolveSeasonsScript.dissSeasonsTemp[i]) {
				dissolveMats[i].SetFloat ("_Threshold", 0f);
				matsToDissolve.Add(dissolveMats[i]);
			}
			else { // Else make it dissolved already.
				dissolveMats[i].SetFloat ("_Treshold", 1.011f);
			}
		}
	}

	public void ResetHubSeasons() { // Run this to either reset the dissolves or not. 
		inHub = false;
		dissAmnt = 0f;
		matsToDissolve.Clear();
		startHubActive = false;
		hubActiveWaitTimer = hubActiveWait;
		dissolveDone = false;
		// Turn off all the Glows
		foreach(GameObject summerGlow in summerGlows)
		{
			if (summerGlow.activeSelf) { summerGlow.SetActive(false); }
		}
		// Reset all dissolve materials.
		foreach(Material dissolveMat in dissolveMats)
		{
			dissolveMat.SetFloat ("_Threshold", 0f);
		}
		// If it already dissolved make it colored.
		for (int i = 0; i < dissolveMats.Count; i++)
		{
			if (GlobalVariables.globVarScript.dissSeasonsBools[i]) {
				dissolveMats[i].SetFloat ("_Threshold", 1.01f);
			}
		}
	}
}
