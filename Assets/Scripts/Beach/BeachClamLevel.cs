using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeachClamLevel : MonoBehaviour {

	public ClamSpot[] clamSpots;
	public BeachClam[] myClams;
	public bool levelLoading, levelComplete, tutorialLevel;
	public Shuffle_Shell_Bubble_Library[] bubbleLibrary;

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
			// foreach (ClamSpot item in availableSpots)
			// {	
			// 	Debug.Log(item.gameObject.name);
			// }
			int rand = Random.Range(0,availableSpots.Count);
			if(tutorialLevel){
				rand = 0;
			}
			clam.gameObject.transform.position = availableSpots[rand].gameObject.transform.position;
			availableSpots[rand].occupied = true;
			
			//clam.clamAnim.SetTrigger("ShowClam");
			// Start anim trigger delay.
			//clam.myClosedClam.FadeIn();
			clam.ShowClams();
			
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
			//Debug.Log(availableSpots.Count);
			clam.ClamSpriteParent.transform.localRotation = availableSpots[rand].gameObject.transform.localRotation;
			availableSpots[rand].occupied = true;
			availableSpots.Clear();
			clam.canTap = true;
			if(tutorialLevel){
				clam.canTap = false;
			}
		}
		SetUpBubbles();
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
				//Debug.Log("Clam # :"+iterator+" SFX : "+clam.clamSound);
			}
			else{
				randomOceanSound = audioBeachPuzzleScript.chooseRandomSound();
				clam.clamSound = randomOceanSound;
				//Debug.Log("Clam # :"+iterator+" SFX : "+clam.clamSound);
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
		for (int i = 0; i < bubbleLibrary.Length; i++)
		{
			bubbleLibrary[i].used = false;
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
	private void SetUpBubbles(){
		for (int i = 0; i < myClams.Length; i++)
		{
			if(!myClams[i].bubblesAssigned){
				List<Shuffle_Shell_Bubble_Library> availableBubbles = new List<Shuffle_Shell_Bubble_Library>();
				for (int j = 0; j < bubbleLibrary.Length; j++)
				{
					if(!bubbleLibrary[j].used){
						availableBubbles.Add(bubbleLibrary[j]);
					}
				}
				int bubbletoTake = Random.Range(0,availableBubbles.Count);
				for (int k = 0; k < myClams[i].myBubbles.Length; k++)
				{
					Color newColor = availableBubbles[bubbletoTake].libBubbles[k].gameObject.GetComponent<SpriteRenderer>().color;
					myClams[i].myBubbles[k].lifeTimedelay = availableBubbles[bubbletoTake].libBubbles[k].gameObject.GetComponent<BeachBubbles>().lifeTimedelay;
					myClams[i].myBubbles[k].bubbleSize = availableBubbles[bubbletoTake].libBubbles[k].gameObject.GetComponent<BeachBubbles>().bubbleSize;
					myClams[i].myBubbles[k].curveMultiplier = availableBubbles[bubbletoTake].libBubbles[k].gameObject.GetComponent<BeachBubbles>().curveMultiplier;
					myClams[i].myBubbles[k].Speed = availableBubbles[bubbletoTake].libBubbles[k].gameObject.GetComponent<BeachBubbles>().Speed;
					myClams[i].myBubbles[k].gameObject.GetComponent<SpriteRenderer>().sprite = availableBubbles[bubbletoTake].libBubbles[k].gameObject.GetComponent<SpriteRenderer>().sprite;
					myClams[i].myBubbles[k].gameObject.GetComponent<SpriteRenderer>().color = newColor;
					myClams[i].myBubbles[k].ResetBubble();

					myClams[i].myMatch.myBubbles[k].lifeTimedelay = availableBubbles[bubbletoTake].libBubbles[k].gameObject.GetComponent<BeachBubbles>().lifeTimedelay;
					myClams[i].myMatch.myBubbles[k].bubbleSize = availableBubbles[bubbletoTake].libBubbles[k].gameObject.GetComponent<BeachBubbles>().bubbleSize;
					myClams[i].myMatch.myBubbles[k].curveMultiplier = availableBubbles[bubbletoTake].libBubbles[k].gameObject.GetComponent<BeachBubbles>().curveMultiplier;
					myClams[i].myMatch.myBubbles[k].Speed = availableBubbles[bubbletoTake].libBubbles[k].gameObject.GetComponent<BeachBubbles>().Speed;
					myClams[i].myMatch.myBubbles[k].gameObject.GetComponent<SpriteRenderer>().sprite = availableBubbles[bubbletoTake].libBubbles[k].gameObject.GetComponent<SpriteRenderer>().sprite;
					myClams[i].myMatch.myBubbles[k].gameObject.GetComponent<SpriteRenderer>().color = newColor;
					myClams[i].myMatch.myBubbles[k].ResetBubble();

					availableBubbles[bubbletoTake].used = true;
				}
				availableBubbles.Clear();
				myClams[i].bubblesAssigned = true;
				myClams[i].myMatch.bubblesAssigned = true;
			}
			
		}
	}
}
