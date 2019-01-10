using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeachClamLevel : MonoBehaviour {

	public ClamSpot[] clamSpots;
	public BeachClam[] myClams;
	public bool levelLoading, levelComplete;
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
			availableSpots[rand].occupied = true;
			availableSpots.Clear();			
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
}
