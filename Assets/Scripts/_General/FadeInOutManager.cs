using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOutManager : MonoBehaviour {

	// Use this for initialization
	[Header("Start Level Variables")]
	public GameObject[] FIParents;
	public FadeInOutSprite[] FIFadeScripts;
		
	/// <summary>Activate all the fadein functions in the Starts level Variables.</summary>
	public void StartLvlFadeIn(){
		FadeInOutSprite[] MyFadeInOut;
		foreach (GameObject MyParent in FIParents)
		{
			if(!MyParent.activeSelf) MyParent.SetActive(true);
			MyFadeInOut =  MyParent.GetComponentsInChildren<FadeInOutSprite>();
			foreach (FadeInOutSprite myFade in MyFadeInOut)
			{
				myFade.FadeIn();
			}
		}
		foreach (FadeInOutSprite myFade in FIFadeScripts)
		{
			myFade.FadeIn();
		}
		
	}
		
	/// <summary>Activate all the fadeout functions in the Exit level Variables.</summary>
	public void ExitFadeOutLvl(){
		FadeInOutSprite[] MyFadeInOut;
		foreach (GameObject MyParent in FIParents)
		{
			MyFadeInOut =  MyParent.GetComponentsInChildren<FadeInOutSprite>();
			foreach (FadeInOutSprite myFade in MyFadeInOut)
			{
				myFade.FadeOut();
			}
		}
		foreach (FadeInOutSprite myFade in FIFadeScripts)
		{
			myFade.FadeOut();
		}
		
	}
}
