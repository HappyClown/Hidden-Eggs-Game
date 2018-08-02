using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CafePuzzleLevel : MonoBehaviour {

	// Use this for initialization
	public CafePuzzleCup[] myCups;
	public int levelNumber;
	public bool starting, finishing, loaded,finished, movigCups, setOrder;
	public int currentCup;
	public enum cupColor{
		Red,
		blue,
		green
	}
	public cupColor[] cupsOrder;
	public int requiredCups;
	public float cupsToMove, order;
	public string colorToMove, direction;
	void Start () {
		starting = finishing = false;
		currentCup = 0;
		cupsToMove = 0;
		setOrder = false;
		movigCups = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(cupsToMove > 0){
			setOrder = false;
			for (int i = 0; i < myCups.Length; i++)
			{
				if(myCups[i].myColor.ToString() == colorToMove){
					if(direction == "left" || direction == "down")
					{
						order ++;
						if(order == cupsToMove){
							cupsToMove = 0;
						}
					}
					else if(direction == "right" || direction == "up")
					{
						order --;
						if(order == 1){
							cupsToMove = 0;
						}
					}
					if(direction == "left" || direction == "right")
					{	
						for (int j = 0; j < myCups.Length; j++)
						{					
							if(myCups[j].hMovingOrder == order && myCups[j].myColor.ToString() == colorToMove)
							{
								myCups[j].MoveCup(direction);
							}
						}
					}
					else if(direction == "down" || direction == "up")
					{	
						for (int j = 0; j < myCups.Length; j++)
						{
							if(myCups[j].vMovingOrder == order && myCups[j].myColor.ToString() == colorToMove)
							{
								myCups[j].MoveCup(direction);
							}
						}
					}
				}
			}
		}
		movigCups = false;
		for (int i = 0; i < myCups.Length; i++)
		{	
			if(myCups[i].moving){
				 movigCups = true;
			}
		}
		if(!movigCups && !setOrder){
			for (int i = 0; i < myCups.Length; i++)
			{	
				myCups[i].SetOrder();
			}
			setOrder = true;
		}
	}
	public void Swipe(string color, string dir){
		cupsToMove = 0;
		colorToMove = color;
		for (int i = 0; i < myCups.Length; i++)
		{	
			if(myCups[i].myColor.ToString() == color){
				cupsToMove ++;
			}
		}
		if(dir == "left" || dir == "down")
		{
			order = 0;
		}
		else if(dir == "right" || dir == "up")
		{
			order = cupsToMove + 1;
		}
		direction = dir;
	}
}
