using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KitePuzzEngine : MainPuzzleEngine 
{
	[Header("Kite Engine Values")]
	public int connections;
	public List<float> bGSizes;
	public BackgroundScale bgScleScript;
	public ClickToRotateTile clickToRotTileScript;
 
	//private 
	[Header("Scripts")]
	public ResetTiles resetTilesScript;
	public KiteLevelChangeEvent kiteLevelChangeScript;

	void Start () {
		canPlay = false;
		initialSetupOn = true;
		maxLvl = GlobalVariables.globVarScript.puzzMaxLvl;
		tutorialDone = GlobalVariables.globVarScript.puzzIntroDone;
	}

	void Update () {
		if(resetLevel){
			ResetLevel();	
			resetLevel = false;
		}
		if(setupLevel){
			SetUpLevel();					
			setupLevel = false;
		}
		if (canPlay) {
			RunBasics(canPlay);
			if (connections == clickToRotTileScript.connectionsNeeded) {
				SilverEggsSetup();
				EndOfLevelEvent();
			}
		}
		else {
			RunBasics(canPlay);	
		}		
	}
	

	#region level Methods
	public void EndOfLevelEvent() {
		kiteLevelChangeScript.LevelChangeEvent();
	}	
	private void ResetLevel(){
		resetTilesScript.FillTileResetArray();	
		connections = 0;
		bgScleScript.ScaleBG();
		//resetTilesScript.EndOfLevelReset();
	}
	private void SetUpLevel(){
		clickToRotTileScript.CalculateConnectionsNeeded();
		bgScleScript.ScaleBG();
		clickToRotTileScript.connectionsNeeded = clickToRotTileScript.CalculateConnectionsNeeded(); // For fun, to try giving a method a return type for the first time.
		resetTilesScript.FillTileResetArray();
	}
	#endregion
}