using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HubEggcounts : MonoBehaviour 
{
	public TextMeshProUGUI totEggCount;

	public int totEgg;
	public int TotEgg { get { return totEgg; } }

	[Header("Market")]
	public TextMeshProUGUI marketPopUpRegCount;
	public TextMeshProUGUI marketPopUpSilCount;
	public TextMeshProUGUI marketPopUpGolCount;

	[Header("Park")]
	public TextMeshProUGUI parkPopUpRegCount;
	public TextMeshProUGUI parkPopUpSilCount;
	public TextMeshProUGUI parkPopUpGolCount;

	void Start (){
		AdjustTotEggCount();
	}

	public void AdjustTotEggCount()
	{
		GlobalVariables.globVarScript.LoadCorrectEggs();
		// NEED TO MAKE SOEMTHIGN ELSE THAT GETS ALL THE SCENE TOTAL EGGS, or sumtin simila
		totEgg = GlobalVariables.globVarScript.hubTotalEggsFound;

		if (totEgg <= 0)
		{ totEggCount.text = "000"; return; }
		else if (totEgg < 10)
		{ totEggCount.text = "00" + totEgg; return; }
		else if (totEgg < 100)
		{ totEggCount.text = "0" + totEgg; return; }
		else
		{ totEggCount.text = "" + totEgg; return; }
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
