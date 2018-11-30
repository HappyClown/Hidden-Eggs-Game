using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOutManager : MonoBehaviour {

	// Use this for initialization
	[Header("Start Level Variables")]
	public GameObject[] startLvlFIParents;
	public FadeInOutSprite[] startLvlFIFadeScripts;
	[Header("Level Complete Variables")]
	public GameObject[] lvlCompleteFIParents;
	public FadeInOutSprite[] lvlCompleteFIFadeScripts;
	public GameObject[] lvlCompleteFOParents;
	public FadeInOutSprite[] lvlCompleteFOFadeScripts;
	[Header("Exit Level Variables")]
	public GameObject[] exitLvlFOParents;
	public FadeInOutSprite[] exitLvlFOFadeScripts;
	
	
	/// <summary>Activate all the fadein functions in the Starts level Variables.</summary>
	public void StartLvlFadeIn(){
		FadeInOutSprite[] MyFadeInOut;
		foreach (GameObject MyParent in startLvlFIParents)
		{
			MyFadeInOut =  MyParent.GetComponentsInChildren<FadeInOutSprite>();
			foreach (FadeInOutSprite myFade in MyFadeInOut)
			{
				myFade.FadeIn();
			}
		}
		foreach (FadeInOutSprite myFade in startLvlFIFadeScripts)
		{
			myFade.FadeIn();
		}
		
	}
	
	/// <summary>Activate all the fadeIn functions in the Complete level Variables.</summary>
	public void lvlCompleteFadeIn(){
		FadeInOutSprite[] MyFadeInOut;
		foreach (GameObject MyParent in lvlCompleteFIParents)
		{
			MyFadeInOut =  MyParent.GetComponentsInChildren<FadeInOutSprite>();
			foreach (FadeInOutSprite myFade in MyFadeInOut)
			{
				myFade.FadeOut();
			}
		}
		foreach (FadeInOutSprite myFade in lvlCompleteFIFadeScripts)
		{
			myFade.FadeOut();
		}
		
	}
	/// <summary>Activate all the fadeout functions in the Complete level Variables.</summary>
	public void lvlCompleteFadeOut(){
		FadeInOutSprite[] MyFadeInOut;
		foreach (GameObject MyParent in lvlCompleteFOParents)
		{
			MyFadeInOut =  MyParent.GetComponentsInChildren<FadeInOutSprite>();
			foreach (FadeInOutSprite myFade in MyFadeInOut)
			{
				myFade.FadeOut();
			}
		}
		foreach (FadeInOutSprite myFade in lvlCompleteFOFadeScripts)
		{
			myFade.FadeOut();
		}
		
	}
	
	/// <summary>Activate all the fadeout functions in the Exit level Variables.</summary>
	public void ExitFadeOutLvl(){
		FadeInOutSprite[] MyFadeInOut;
		foreach (GameObject MyParent in exitLvlFOParents)
		{
			MyFadeInOut =  MyParent.GetComponentsInChildren<FadeInOutSprite>();
			foreach (FadeInOutSprite myFade in MyFadeInOut)
			{
				myFade.FadeOut();
			}
		}
		foreach (FadeInOutSprite myFade in exitLvlFOFadeScripts)
		{
			myFade.FadeOut();
		}
		
	}
}
