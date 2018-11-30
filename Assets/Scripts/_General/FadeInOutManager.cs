using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOutManager : MonoBehaviour {

	// Use this for initialization
	[Header("Start Level Variables")]
	public GameObject[] startLvlParents;
	public FadeInOutSprite[] startLvlFadeScripts;
	[Header("Level Complete Variables")]
	public GameObject[] lvlCompleteParents;
	public FadeInOutSprite[] lvlCompleteFadeScripts;
	[Header("Exit Level Variables")]
	public GameObject[] exitLvlParents;
	public FadeInOutSprite[] exitLvlFadeScripts;
	
	
	/// <summary>Activate all the fadein functions in the Starts level Variables.</summary>
	public void StartLvlFadeIn(){
		FadeInOutSprite[] MyFadeInOut;
		foreach (GameObject MyParent in startLvlParents)
		{
			MyFadeInOut =  MyParent.GetComponentsInChildren<FadeInOutSprite>();
			foreach (FadeInOutSprite myFade in MyFadeInOut)
			{
				myFade.FadeIn();
			}
		}
		foreach (FadeInOutSprite myFade in startLvlFadeScripts)
		{
			myFade.FadeIn();
		}
		
	}
	
	/// <summary>Activate all the fadeout functions in the Complete level Variables.</summary>
	public void lvlCompleteFadeOut(){
		FadeInOutSprite[] MyFadeInOut;
		foreach (GameObject MyParent in lvlCompleteParents)
		{
			MyFadeInOut =  MyParent.GetComponentsInChildren<FadeInOutSprite>();
			foreach (FadeInOutSprite myFade in MyFadeInOut)
			{
				myFade.FadeOut();
			}
		}
		foreach (FadeInOutSprite myFade in lvlCompleteFadeScripts)
		{
			myFade.FadeOut();
		}
		
	}
	
	/// <summary>Activate all the fadeout functions in the Exit level Variables.</summary>
	public void ExitFadeOutLvl(){
		FadeInOutSprite[] MyFadeInOut;
		foreach (GameObject MyParent in exitLvlParents)
		{
			MyFadeInOut =  MyParent.GetComponentsInChildren<FadeInOutSprite>();
			foreach (FadeInOutSprite myFade in MyFadeInOut)
			{
				myFade.FadeOut();
			}
		}
		foreach (FadeInOutSprite myFade in exitLvlFadeScripts)
		{
			myFade.FadeOut();
		}
		
	}
}
