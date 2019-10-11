using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeachClamLevel : MonoBehaviour {

	public ClamSpot[] clamSpots;
	public BeachClam[] myClams;
	public bool levelLoading, levelComplete;

/// test for sounds ////
	public AudioSceneBeachPuzzle audioBeachPuzzleScript;

	void Start() {
		audioBeachPuzzleScript = GameObject.Find("Audio").GetComponent<AudioSceneBeachPuzzle>();
	}

	public void SetUpLevel(){
		foreach (BeachClam clam in myClams)
		{
			List<ClamSpot> availableSpots = new List<ClamSpot>();
			foreach (ClamSpot spot in clamSpots)
			{
				if(!spot.occupied){
					availableSpots.Add(spot);
				}
			}
			int rand = Random.Range(0,availableSpots.Count);
			clam.gameObject.transform.position = availableSpots[rand].gameObject.transform.position;
			//clam.clamAnim.SetTrigger("ShowClam");
			// Start anim trigger delay.
			//clam.myClosedClam.FadeIn();
			clam.ShowClams();
			availableSpots[rand].occupied = true;
			availableSpots.Clear();
		}
		foreach (ClamSpot clamSpot in clamSpots)
		{
			clamSpot.occupied = false;
		}
		foreach (BeachClam clam in myClams)
		{
			List<ClamSpot> availableSpots = new List<ClamSpot>();
			foreach (ClamSpot spot in clamSpots)
			{
				if(!spot.occupied){
					availableSpots.Add(spot);
				}
			}
			int rand = Random.Range(0,availableSpots.Count);
			Debug.Log(availableSpots.Count);
			clam.ClamSpriteParent.transform.localRotation = availableSpots[rand].gameObject.transform.localRotation;
			availableSpots[rand].occupied = true;
			availableSpots.Clear();
		}
		//test sounds
		setUpSounds();
		audioBeachPuzzleScript.newLevel();
	}

////////  TEST FOR SOUNDS  ///////////
	public void setUpSounds(){

		int iterator = 1;
		string randomOceanSound = audioBeachPuzzleScript.chooseRandomSound();
		foreach (BeachClam clam in myClams)
		{	
			if(iterator%2==0){
				clam.clamSound = randomOceanSound;
				Debug.Log("Clam # :"+iterator+" SFX : "+clam.clamSound);
			}
			else{
				randomOceanSound = audioBeachPuzzleScript.chooseRandomSound();
				clam.clamSound = randomOceanSound;
				Debug.Log("Clam # :"+iterator+" SFX : "+clam.clamSound);
			}
			iterator++;
		}
	} 

	public void ResetLevel(){
		foreach (BeachClam clam in myClams)
		{
			clam.ResetClams();
		}
		foreach (ClamSpot clamSpot in clamSpots)
		{
			clamSpot.occupied = false;
		}
		levelComplete = false;
	}
	public void CheckClams(){
		bool check = true;
		foreach (BeachClam clam in myClams)
		{
			if(!clam.matched){
				check = false;
			}
		}
		if(check){
			levelComplete = true;
		}
	}
	public void CleanClamBubbles(){
		foreach (BeachClam clam in myClams)
		{
			clam.CleanBubbles();
		}
	}
}
