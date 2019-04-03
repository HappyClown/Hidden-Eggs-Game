using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTitleVillage : MonoBehaviour {

	public float speed, maxLenght;
	public bool openTitle, closeTitle;
	private bool  openingTitle, closingTitle, closed, open;
	private float lerpValue, currentLenght;
	private RectTransform myRectTransform;
	public LevelTitleVillage[] AllTitles;
	public AudioManagerHubMenu audioManHubMenuScript;

	// Use this for initialization
	
	void Start () {
		myRectTransform = this.GetComponent<RectTransform>();
		ResetTittle ();

		if (!audioManHubMenuScript) {
			audioManHubMenuScript = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManagerHubMenu>();
		}
	}

	// Update is called once per frame
	void Update () {
		if(openTitle){
			ResetTittle();
			openingTitle = true;
			closed = false;
		}
		if(closeTitle){
			closingTitle = true;
			openingTitle = false;
			currentLenght = myRectTransform.sizeDelta.x;
			closeTitle = false;
			lerpValue = 0;
			open = false;
		}
		if(openingTitle){
			lerpValue += Time.deltaTime*speed;
			myRectTransform.sizeDelta = new Vector2(Mathf.Lerp(0,maxLenght,lerpValue),myRectTransform.sizeDelta.y);
			if(lerpValue >= 1){
				openingTitle = false;
				open = true;
			}
		}else if(closingTitle){
			lerpValue += Time.deltaTime*speed;
			myRectTransform.sizeDelta = new Vector2(Mathf.Lerp(currentLenght,0,lerpValue),myRectTransform.sizeDelta.y);
			if(lerpValue >= 1){
				closingTitle = false;
				closed = true;
			}
		}
	}
	void ResetTittle () {
		lerpValue = 0;
		openTitle = closeTitle = closingTitle = openingTitle = open = false;
		myRectTransform.sizeDelta = new Vector2(0,myRectTransform.sizeDelta.y);
		closed = true;
	}
	public void OpenTitle(){
		if(!openingTitle && !open){
			openTitle = true;
			foreach (LevelTitleVillage titles in AllTitles)
			{	
				if(titles.open || titles.openingTitle){
					titles.CloseTitle();
				}
			}
		//Sound
		audioManHubMenuScript.StatPaperSound_on();
		}
	}
	public void ForceOpen(){
		myRectTransform.sizeDelta = new Vector2(maxLenght,myRectTransform.sizeDelta.y);
		openTitle = closeTitle = closingTitle = openingTitle = false;
		lerpValue = 0;
		open = true;
	}
	public void CloseTitle(){
		if(!openTitle){
			closeTitle = true;
		}

	//Sound
	audioManHubMenuScript.StatPaperSound_off();
	}
	

}
