using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelTitleVillage : MonoBehaviour {

	public float duration, maxLenght;
	//public bool openTitle, closeTitle;
	public bool  openingTitle, closingTitle, closed, open;
	private float lerpValue, currentLenght;
	public RectTransform myRectTransform;
	public LevelTitleVillage[] otherTitles;
	public Image NormalEgg, silverEgg, goldenEgg, NShadow, SShadow, GShadow;
	public Sprite spriteNormalEgg, spriteSilverEgg, spriteGoldenEgg, spriteEmptyEgg;
	public string myLevel;
	private Coroutine currentCoroutine;
	public AudioManagerHubMenu audioManHubMenuScript;

	void Start () {
		UpdateEggs();
		if (!audioManHubMenuScript) {
			audioManHubMenuScript = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManagerHubMenu>();
		}
	}
	
	IEnumerator OpeningTitle() {
		while (lerpValue < 1) {
			lerpValue += Time.deltaTime / duration;
			myRectTransform.sizeDelta = new Vector2(Mathf.Lerp(0,maxLenght,lerpValue),myRectTransform.sizeDelta.y);
			yield return null;
		}
		openingTitle = false;
		open = true;
		currentCoroutine = null;
	}
	IEnumerator ClosingTitle() {
		while (lerpValue < 1) {
			lerpValue += Time.deltaTime / duration;
			myRectTransform.sizeDelta = new Vector2(Mathf.Lerp(currentLenght,0,lerpValue),myRectTransform.sizeDelta.y);
			yield return null;
		}
		closingTitle = false;
		closed = true;
		currentCoroutine = null;
		this.gameObject.SetActive(false);
	}

	public void OpenTitle(){
		this.gameObject.SetActive(true);
		audioManHubMenuScript.StatPaperSound_on();
		foreach (LevelTitleVillage titles in otherTitles) {	
			if (titles.open || titles.openingTitle) {
				titles.CloseTitle();
				audioManHubMenuScript.StatPaperSound_off();
			}
		}
		// Setup opening variables.
		ResetTitle();
		closingTitle = false;
		closed = false;
		openingTitle = true;
		open = false;
		if (currentCoroutine != null) {
			StopCoroutine(currentCoroutine);
			currentCoroutine = null;
		}
		currentCoroutine = StartCoroutine(OpeningTitle());
	}
	public void CloseTitle(){
		closingTitle = true;
		closed = false;
		openingTitle = false;
		open = false;
		currentLenght = myRectTransform.sizeDelta.x;
		lerpValue = 0;
		if (currentCoroutine != null) {
			StopCoroutine(currentCoroutine);
			currentCoroutine = null;
		}
		currentCoroutine = StartCoroutine(ClosingTitle());
	}
	public void ForceOpen(){
		myRectTransform.sizeDelta = new Vector2(maxLenght,myRectTransform.sizeDelta.y);
		open = closed = closingTitle = openingTitle = false;
		lerpValue = 0;
		open = true;
	}
	
	public void ResetTitle () {
		lerpValue = 0;
		open = closingTitle = openingTitle = closed = false;
		myRectTransform.sizeDelta = new Vector2(0,myRectTransform.sizeDelta.y);
		closed = true;
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
