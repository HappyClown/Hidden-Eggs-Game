using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelTitleVillage : MonoBehaviour {

	public float duration, maxLenght;
	public bool openTitle, closeTitle;
	private bool  openingTitle, closingTitle, closed, open;
	private float lerpValue, currentLenght;
	private RectTransform myRectTransform;
	public LevelTitleVillage[] AllTitles;
	public Image NormalEgg, silverEgg, goldenEgg, NShadow, SShadow, GShadow;
	public Sprite spriteNormalEgg, spriteSilverEgg, spriteGoldenEgg, spriteEmptyEgg;
	public string myLevel;

	public AudioManagerHubMenu audioManHubMenuScript;
	void Start () {
		myRectTransform = this.GetComponent<RectTransform>();
		ResetTittle ();
		UpdateEggs();

		if (!audioManHubMenuScript) {
			audioManHubMenuScript = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManagerHubMenu>();
		}
	}
	
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
			lerpValue += Time.deltaTime / duration;
			myRectTransform.sizeDelta = new Vector2(Mathf.Lerp(0,maxLenght,lerpValue),myRectTransform.sizeDelta.y);
			if(lerpValue >= 1){
				openingTitle = false;
				open = true;
			}
		}else if(closingTitle){
			lerpValue += Time.deltaTime / duration;
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
			//sound
			audioManHubMenuScript.StatPaperSound_on();

			foreach (LevelTitleVillage titles in AllTitles)
			{	
				if(titles.open || titles.openingTitle){
					titles.CloseTitle();
					//sound
					audioManHubMenuScript.StatPaperSound_off();
				}
			}
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
	}
	public void UpdateEggs(){
		if(myLevel == GlobalVariables.globVarScript.marketName){
			if(GlobalVariables.globVarScript.marketNE){
			NormalEgg.sprite = spriteNormalEgg;
			NShadow.gameObject.SetActive(true);}
			else{
			NormalEgg.sprite = spriteEmptyEgg;
			NShadow.gameObject.SetActive(false);}
			if(GlobalVariables.globVarScript.marketSE){
			silverEgg.sprite = spriteSilverEgg;
			SShadow.gameObject.SetActive(true);}
			else{
			silverEgg.sprite = spriteEmptyEgg;
			SShadow.gameObject.SetActive(false);}
			if(GlobalVariables.globVarScript.marketGE){
			goldenEgg.sprite = spriteGoldenEgg;
			GShadow.gameObject.SetActive(true);}
			else{
			goldenEgg.sprite = spriteEmptyEgg;
			GShadow.gameObject.SetActive(false);}
		}else if(myLevel == GlobalVariables.globVarScript.parkName){
			if(GlobalVariables.globVarScript.parkNE){
			NormalEgg.sprite = spriteNormalEgg;
			NShadow.gameObject.SetActive(true);}
			else{
			NormalEgg.sprite = spriteEmptyEgg;
			NShadow.gameObject.SetActive(false);}
			if(GlobalVariables.globVarScript.parkSE){
			silverEgg.sprite = spriteSilverEgg;
			SShadow.gameObject.SetActive(true);}
			else{
			silverEgg.sprite = spriteEmptyEgg;
			SShadow.gameObject.SetActive(false);}
			if(GlobalVariables.globVarScript.parkGE){
			goldenEgg.sprite = spriteGoldenEgg;
			GShadow.gameObject.SetActive(true);}
			else{
			goldenEgg.sprite = spriteEmptyEgg;
			GShadow.gameObject.SetActive(false);}

		}else if(myLevel == GlobalVariables.globVarScript.beachName){
			if(GlobalVariables.globVarScript.beachNE){
			NormalEgg.sprite = spriteNormalEgg;
			NShadow.gameObject.SetActive(true);}
			else{
			NormalEgg.sprite = spriteEmptyEgg;
			NShadow.gameObject.SetActive(false);}
			if(GlobalVariables.globVarScript.beachSE){
			silverEgg.sprite = spriteSilverEgg;
			SShadow.gameObject.SetActive(true);}
			else{
			silverEgg.sprite = spriteEmptyEgg;
			SShadow.gameObject.SetActive(false);}
			if(GlobalVariables.globVarScript.beachGE){
			goldenEgg.sprite = spriteGoldenEgg;
			GShadow.gameObject.SetActive(true);}
			else{
			goldenEgg.sprite = spriteEmptyEgg;
			GShadow.gameObject.SetActive(false);}

		}
	}

}
