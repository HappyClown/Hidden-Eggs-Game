using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HubEggcounts : MonoBehaviour 
{
	public TextMeshProUGUI totEggCount;

	public void AdjustTotEggCount()
	{
		GlobalVariables.globVarScript.LoadCorrectEggs();
		// NEED TO MAKE SOEMTHIGN ELSE THAT GETS ALL THE SCENE TOTAL EGGS, or sumtin simila
		int totEggTemp = GlobalVariables.globVarScript.marketTotalEggsFound + GlobalVariables.globVarScript.parkTotalEggsFound; 

		if (totEggTemp <= 0)
		{ totEggCount.text = "000"; return; }
		else if (totEggTemp < 10)
		{ totEggCount.text = "00" + totEggTemp; return; }
		else if (totEggTemp < 100)
		{ totEggCount.text = "0" + totEggTemp; return; }
		else
		{ totEggCount.text = "" + totEggTemp; return; }
	}
}
