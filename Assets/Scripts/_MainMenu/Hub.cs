using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hub : MonoBehaviour 
{
	public float hubActiveWait;
	private float hubActiveWaitTimer;

	[Header("Dissolve")]
	//public bool dissolveEverytime;
	public float dissAmnt;
	public float dissSpeed;
	public List<Material> dissolveMats;
	public List<Material> matsToDissolve;
	public List<bool> seasonsToDiss;

	[Header("Season Objects")]
	//public bool dissolveSummer;
	public List<GameObject> summerButtons;
	public List<GameObject> summerGlows;
	public List<GameObject> fallObjs;

	[Header("What To Do Bools")]
	public bool dissolving;
	public bool inHub;
	public bool startHubActive;

	[Header("References")]
	public DissolveSeasons dissolveSeasonsScript;
	public BackToMenu backToMenuScript;
	

	void Start ()
	{
		ResetHubSeasons();

		// if (GlobalVariables.globVarScript.toHub) { 
		// 	EnableHubObjects(); 
		// 	dissolveSeasonsScript.SeasonDissolveCheck();
		// 	DecideDissolve(); 
		// 	inHub = true; 
		// 	dissolving = true; 
		// }
		hubActiveWaitTimer = hubActiveWait;
	}


	void Update () 
	{
		// - Start Countdown Timer before Dissolves - //
		if (startHubActive) {
			if (hubActiveWaitTimer == hubActiveWait) {
				dissolveSeasonsScript.SeasonDissolveCheck(); 
				DecideDissolve();
			}
			hubActiveWaitTimer -= Time.deltaTime;
		}

		// - Start Dissolving Unlocked Seasons - //
		if(hubActiveWaitTimer <= 0f && !inHub) {
			startHubActive = false;
			hubActiveWaitTimer = hubActiveWait;
			EnableHubObjects();
			
			inHub = true;
			dissolving = true;
		}

		// - Dissolve All Seasons - //
		if (dissolving && inHub) 
		{
			if (dissAmnt < 1.01f && matsToDissolve.Count > 0) 
			{
				dissAmnt += Time.deltaTime * dissSpeed;
				foreach(Material matToDiss in matsToDissolve)
				{
					matToDiss.SetFloat ("_Threshold", dissAmnt);
				}
			}
			else
			{

				dissolveSeasonsScript.SaveSeasonDissolves();
				EnableSeasonObjs();
				matsToDissolve.Clear();
				dissolving = false;
			}
		}
	}


	void EnableHubObjects() // Enable general Village objects (UI, etc) 
	{
		backToMenuScript.backToMenuFadeScript.FadeIn();

		backToMenuScript.backToMenuIconFadeScript.FadeIn();
	}

	void EnableSeasonObjs() // Enable Season objects (Scene buttons)
	{
		backToMenuScript.backToMenuBtn.enabled = true;

		if (GlobalVariables.globVarScript.dissSeasonsBools[0])
		{
			foreach(GameObject summerObj in summerButtons)
			{
				if (!summerObj.activeSelf) { summerObj.SetActive(true); }
			}
			foreach(GameObject summerGlow in summerGlows)
			{
				if (!summerGlow.activeSelf) { summerGlow.SetActive(true); }
			}
		}

		// ...
	}
	
	void DecideDissolve () // Decide which seasons to dissolve or have already colored
	{
		// If a season hasnt fully dissolved once and the player has enough eggs dissSeason[i] will be true and its corresponding material will be added to the dissolve list.
		for (int i = 0; i < dissolveSeasonsScript.dissSeasonsTemp.Count; i++)
		{
			if (dissolveSeasonsScript.dissSeasonsTemp[i])
			{
				dissolveMats[i].SetFloat ("_Threshold", 0f);
				matsToDissolve.Add(dissolveMats[i]);
			}
			else // Else make it dissolved already.
			{
				dissolveMats[i].SetFloat ("_Treshold", 1.011f);
			}
		}
	}

	public void ResetHubSeasons() // Run this to either reset the dissolves or not.
	{
		inHub = false;
		dissAmnt = 0f;
		matsToDissolve.Clear();
		startHubActive = false;
		hubActiveWaitTimer = hubActiveWait;

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



	// public void PrepareHub() // Run this to
	// {
	// 	dissAmnt = 0f;
	// 	// To dissolve or not to dissolve.
	// 	if (!dissolveEverytime) {
	// 		for (int i = 0; i < dissolveMats.Count; i++)
	// 		{
	// 			if (GlobalVariables.globVarScript.dissSeasonsBools[i]) {
	// 				dissolveMats[i].SetFloat ("_Threshold", 1.011f);
	// 				EnableHubObjects();
	// 			}
	// 			else {

	// 			}
	// 		}
			
	// 	}
	// 	else {
	// 		foreach(Material dissolveMat in dissolveMats)
	// 		{
	// 			dissolveMat.SetFloat ("_Threshold", 0f);
	// 		}
	// 		dissolving = true;
	// 	}
	// }

}
