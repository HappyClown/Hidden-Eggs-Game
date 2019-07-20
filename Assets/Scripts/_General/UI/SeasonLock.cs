using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SeasonLock : MonoBehaviour {
	[Header ("Settings")]
	public bool comingSoonTit;
	public float eggsRequired, whiteSpeed, iterations, setUpTime;
	public float maxEggsPerSec, eggReqSpeedDamp;
	[Header ("References")]
	public Hub myHub;
	public HubEggcounts myCount;
	public Animator unlockAnim, eggReqAnim;
	public FadeInOutImage backColorFadeScript;
	public FadeInOutTMP amntReqFadeScript;
	public TextMeshProUGUI myEggCounter;
	public RectTransform  whiteTarget, whiteImage, whiteStartPos;
	public Image closedLock, openLock;
	public GameObject bannerTitle, comingSoonTitle, lockMask;
	public ParticleSystem oneReqSparkFX, multiReqSparkFX;
	[Header ("Info")]
	public bool locked;
	public bool settingUp;
	private bool checkSeason, unlocking, lerpEggAmnt;
	private int iterCounter;
	private float timer, lastEggVal, newEggVal, eggLerpTimer;
	private int eggAmntForAnim;
	private float eggsLeft, curEggReqSpeed;

	void Start () {
		unlocking = false;
		timer = 0;
		iterCounter = 0;
		lockMask.SetActive(false);
		checkSeason = false;
		maxEggsPerSec *= Time.deltaTime;
	}
	
	void Update () {
		if(myHub.inHub){
			if(!checkSeason){
				// Check if the season is already unlocked.
				CheckUnlock();
				if(locked){
					// Set up the banner and the closed lock. 
					SetUpTitle();
					CalculateEggReqs();
				}
				checkSeason = true;
			}
			if(settingUp) {
				timer += Time.deltaTime;
				if(timer >= setUpTime){
					settingUp = false;
					if (lastEggVal != newEggVal) {
						lerpEggAmnt = true;
					}
					timer = 0;
				}
			}
			if (lerpEggAmnt) {
				if (curEggReqSpeed < maxEggsPerSec) {
					curEggReqSpeed += Time.deltaTime / eggReqSpeedDamp;
				}
				lastEggVal -= curEggReqSpeed;
				if (eggAmntForAnim != Mathf.RoundToInt(lastEggVal)) {
					eggReqAnim.SetTrigger("ScaleCounter");
					oneReqSparkFX.Play();
				}
				eggAmntForAnim = Mathf.RoundToInt(lastEggVal);
				myEggCounter.text = eggAmntForAnim.ToString();
				if (lastEggVal <= newEggVal) {
					lerpEggAmnt = false;
					lastEggVal = newEggVal;
					SaveNewLastEggVal();
					multiReqSparkFX.Play();
				}
			}
			else if(eggsLeft <= 0 && !unlocking && locked){
				UnlockSeason();
			}
			else if(unlocking && locked){
				// Unlock sequence starts here.
				// timer += Time.deltaTime * whiteSpeed;
				// if(timer <= 1){
				// 	whiteImage.position = Vector3.Lerp(whiteStartPos.position,whiteTarget.position,timer);
				// }
				// else{
				// 	iterCounter ++;
				// 	if(iterCounter == iterations)
				// 	{
				// 		timer = 0;
				 		locked = false;
				// 		UnlockSequence();
				// 	}
				// 	else{
				// 		timer = 0;
				// 		whiteImage.position = whiteStartPos.position;
				// 	}
				// }
			}
			else if (unlocking) {
				unlockAnim.SetTrigger("UnlockSeason");
				RemoveLock();
				unlocking = false;
			}
		}
	}
	void CheckUnlock(){
		//Check if the season is already unlocked
		if(!locked && comingSoonTitle){
			bannerTitle.SetActive(false);
			// To be terminated once all the seasons are implemented.
			comingSoonTitle.SetActive(true);
			comingSoonTitle.GetComponent<FadeInOutImage>().FadeIn();
			comingSoonTitle.GetComponentInChildren<FadeInOutTMP>().FadeIn();
		}
	}
	void SetUpTitle(){
		bannerTitle.SetActive(true);
		closedLock.gameObject.SetActive(true);
		comingSoonTitle.SetActive(false);
		bannerTitle.GetComponent<FadeInOutImage>().FadeIn();
		FadeInOutTMP[] myFade =  bannerTitle.GetComponentsInChildren<FadeInOutTMP>();
		foreach (FadeInOutTMP fade in myFade)
		{
			fade.FadeIn();
		}
		amntReqFadeScript.FadeIn();
		closedLock.gameObject.GetComponent<FadeInOutImage>().FadeIn();
		backColorFadeScript.gameObject.SetActive(true);
		backColorFadeScript.FadeIn();
		settingUp = true;
		// Debug.Log("I'm here!!");
	}
	void UnlockSeason(){
		unlocking = true;
		// lockMask.SetActive(true);
	}
	void UnlockSequence(){
		closedLock.gameObject.SetActive(false);
		openLock.gameObject.SetActive(true);
		//lockMask.SetActive(false);
	}
	void RemoveLock(){
		// openLock.gameObject.GetComponent<FadeInOutImage>().FadeOut();
		bannerTitle.GetComponent<FadeInOutImage>().FadeOut();
		FadeInOutTMP[] myFade =  bannerTitle.GetComponentsInChildren<FadeInOutTMP>();
		foreach (FadeInOutTMP fade in myFade)
		{
			fade.FadeOut();
		}
		if(comingSoonTit){
			float delay = openLock.gameObject.GetComponent<FadeInOutImage>().fadeDuration;
			comingSoonTitle.SetActive(true);
			FadeInOutTMP textFade = comingSoonTitle.GetComponentInChildren<FadeInOutTMP>();
			FadeInOutImage imgFade = comingSoonTitle.GetComponent<FadeInOutImage>();
			textFade.FadeIn();
			imgFade.FadeIn();
		}
		else{
			// Activate next season!!!
		}
	}

	public void CalculateEggReqs() {
		lastEggVal = GlobalVariables.globVarScript.lastEggTotVal;
		// Should only happen the first time a season unlock appears.
		if (lastEggVal <= 0) {
			lastEggVal = eggsRequired;
		}
		newEggVal = eggsRequired - GlobalVariables.globVarScript.hubTotalEggsFound;
		if (newEggVal < 0) {
			newEggVal = 0;
		}
		eggAmntForAnim = (int)lastEggVal;
		eggsLeft = lastEggVal;
		myEggCounter.text = lastEggVal.ToString();
	}

	void SaveNewLastEggVal() {
		GlobalVariables.globVarScript.lastEggTotVal = newEggVal;
		GeneralSaveLoadManager.SaveGeneralData(GlobalVariables.globVarScript);
	}
}
