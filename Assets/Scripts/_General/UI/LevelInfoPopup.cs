using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelInfoPopup : MonoBehaviour
{
    
	public float duration, maxLenght;
	//public bool openTitle, closeTitle;
	public bool  openingTitle, closingTitle, closed, open;
	private float lerpValue, currentLenght;
	public RectTransform myRectTransform;
	//public LevelTitleVillage[] otherTitles;
	public Image NormalEgg, silverEgg, goldenEgg, NShadow, SShadow, GShadow;
	public Sprite spriteNormalEgg, spriteSilverEgg, spriteGoldenEgg, spriteEmptyEgg;
    public TMP_Text sceneTitle;
	public string myLevel;
	private Coroutine currentCoroutine;
	public AudioManagerHubMenu audioManHubMenuScript;
    public CustomButtonClick customButtonClick;

	IEnumerator OpeningTitle() {
		while (lerpValue < 1) {
			lerpValue += Time.deltaTime / duration;
			myRectTransform.sizeDelta = new Vector2(myRectTransform.sizeDelta.x, Mathf.Lerp(0,maxLenght,lerpValue));
			yield return null;
		}
		openingTitle = false;
		open = true;
		currentCoroutine = null;
	}
	IEnumerator ClosingTitle() {
		while (lerpValue < 1) {
			lerpValue += Time.deltaTime / duration;
			myRectTransform.sizeDelta = new Vector2(myRectTransform.sizeDelta.x, Mathf.Lerp(currentLenght,0,lerpValue));
			yield return null;
		}
		closingTitle = false;
		closed = true;
		currentCoroutine = null;
		this.gameObject.SetActive(false);
	}

	public void OpenTitle(CustomButtonClick _customButtonClick){
        customButtonClick = _customButtonClick;
		UpdateSceneInfo();
		this.gameObject.SetActive(true);
		audioManHubMenuScript.StatPaperSound_on();
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
		myRectTransform.sizeDelta = new Vector2(myRectTransform.sizeDelta.x,0);
		closed = true;
	}
	
	public void UpdateSceneInfo(){
        sceneTitle.text = customButtonClick.sceneName;
        // Normal egg.
		if (customButtonClick.NE) {
			NormalEgg.sprite = spriteNormalEgg;
			NShadow.gameObject.SetActive(true);}
		else{
			NormalEgg.sprite = spriteEmptyEgg;
			NShadow.gameObject.SetActive(false);}
        // Silver egg.
		if (customButtonClick.SE){
			silverEgg.sprite = spriteSilverEgg;
			SShadow.gameObject.SetActive(true);}
		else{
			silverEgg.sprite = spriteEmptyEgg;
			SShadow.gameObject.SetActive(false);}
        // Golden egg.
		if (customButtonClick.GE){
			goldenEgg.sprite = spriteGoldenEgg;
			GShadow.gameObject.SetActive(true);}
		else{
			goldenEgg.sprite = spriteEmptyEgg;
			GShadow.gameObject.SetActive(false);}
	}
}
