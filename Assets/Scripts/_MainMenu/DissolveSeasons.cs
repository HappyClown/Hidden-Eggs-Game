using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveSeasons : MonoBehaviour {

	public List<bool> dissSeasonsTemp;
	public float summerAmnt, fallAmnt, winterAmnt, springAmnt;

	[Header("References")]
	public HubEggcounts hubEggcountsScript;

	void Start () {
		GlobalVariables.globVarScript.dissSeasonsScript = this.gameObject.GetComponent<DissolveSeasons>();
	}
	
	public void SeasonDissolveCheck () {

		bool change = false;
		if (!GlobalVariables.globVarScript.dissSeasonsBools[0] && hubEggcountsScript.TotEgg >= summerAmnt) {
			dissSeasonsTemp[0] = true; change = true;
		}
		if (!GlobalVariables.globVarScript.dissSeasonsBools[1] && hubEggcountsScript.TotEgg >= fallAmnt) {
			dissSeasonsTemp[1] = true; change = true;
		}
		if (!GlobalVariables.globVarScript.dissSeasonsBools[2] && hubEggcountsScript.TotEgg >= winterAmnt) {
			dissSeasonsTemp[2] = true; change = true;
		}
		if (!GlobalVariables.globVarScript.dissSeasonsBools[3] && hubEggcountsScript.TotEgg >= springAmnt) {
			dissSeasonsTemp[3] = true; change = true;
		}

		// Debug.Log("Was a new season unlocked? " + change);
	}

	public void SaveSeasonDissolves ()
	{
		for (int i = 0; i < GlobalVariables.globVarScript.dissSeasonsBools.Count; i++)
		{
			if (!GlobalVariables.globVarScript.dissSeasonsBools[i] && dissSeasonsTemp[i])
			{
				GlobalVariables.globVarScript.dissSeasonsBools[i] = true;
				// Debug.Log("Assigned newly dissolved season: " + i + " to GlobalVarScript");
			}
		}
		GlobalVariables.globVarScript.SaveVillageState();

		for (int i = 0; i < dissSeasonsTemp.Count; i++)
		{
			dissSeasonsTemp[i] = false;
		}
	}
}
