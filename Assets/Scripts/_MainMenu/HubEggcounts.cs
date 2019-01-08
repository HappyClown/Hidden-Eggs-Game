using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HubEggcounts : MonoBehaviour 
{
	public TextMeshProUGUI totEggCount;

	[Header("Market")]
	public TextMeshProUGUI marketPopUpRegCount;
	public TextMeshProUGUI marketPopUpSilCount;
	public TextMeshProUGUI marketPopUpGolCount;

	[Header("Park")]
	public TextMeshProUGUI parkPopUpRegCount;
	public TextMeshProUGUI parkPopUpSilCount;
	public TextMeshProUGUI parkPopUpGolCount;

	public void AdjustTotEggCount()
	{
		GlobalVariables.globVarScript.LoadCorrectEggs();
		// NEED TO MAKE SOEMTHIGN ELSE THAT GETS ALL THE SCENE TOTAL EGGS, or sumtin simila
		int totEggTemp = GlobalVariables.globVarScript.hubTotalEggsFound;

		if (totEggTemp <= 0)
		{ totEggCount.text = "000"; return; }
		else if (totEggTemp < 10)
		{ totEggCount.text = "00" + totEggTemp; return; }
		else if (totEggTemp < 100)
		{ totEggCount.text = "0" + totEggTemp; return; }
		else
		{ totEggCount.text = "" + totEggTemp; return; }
	}

	public void AdjustPopUpCounts()
	{
		int marketGolEgg;
		if(GlobalVariables.globVarScript.riddleSolved) { marketGolEgg = 1; } else { marketGolEgg = 0; }
		marketPopUpRegCount.text = (GlobalVariables.globVarScript.totalEggsFound - (GlobalVariables.globVarScript.silverEggsCount + marketGolEgg)) + "/23";

		marketPopUpSilCount.text = (GlobalVariables.globVarScript.silverEggsCount) + "/6";

		marketPopUpGolCount.text = marketGolEgg + "/1";

		int parkGolEgg;
		if(GlobalVariables.globVarScript.riddleSolved) { parkGolEgg = 1; } else { parkGolEgg = 0; }
		parkPopUpRegCount.text = (GlobalVariables.globVarScript.totalEggsFound - (GlobalVariables.globVarScript.silverEggsCount + parkGolEgg)) + "/23";

		parkPopUpSilCount.text = (GlobalVariables.globVarScript.silverEggsCount) + "/6";

		parkPopUpGolCount.text = parkGolEgg + "/1";

		
	}

}
