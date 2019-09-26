using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SeasonLock : MonoBehaviour {
	[Header ("Settings")]
	public bool comingSoonTit;
	public float eggsRequired, setUpTime, scaleUpTime;
	public float maxEggsPerSec, eggReqSpeedDamp, seasonObjsDelay, scaleDownDelay, removeLockAnimDelay;
	[Header ("References")]
	public Hub myHubScript;
	public FadeInOutImage backColorFadeScript;
	public List<FadeInOutImage> playCinematicFadeScripts;
	public FadeInOutTMP amntReqFadeScript;
	public TextMeshProUGUI myEggCounter;
	public Animator unlockAnim, eggReqAnim, groupAnim;
	public Image closedLock, openLock;
	public GameObject bannerTitle, comingSoonTitle, lockMask;
	public ParticleSystem oneReqSparkFX, multiReqSparkFX;
	public AudioSeasonUnlockAnim myAudio;
	[Header ("Info")]
	public bool locked;
	public bool settingUp, enableSeasonObjsDelay;
	private bool checkSeason, unlocking, lerpEggAmnt, scaledUp;
	private bool startLockAnimDelay;
	private int iterCounter;
	private float timer, lastEggVal, newEggVal, seasonObjsTimer;
	private int eggAmntForAnim;
	private float eggsLeft, curEggReqSpeed;

	void Start () {
		unlocking = false;
		timer = 0;
		iterCounter = 0;
		lockMask.SetActive(false);
		checkSeason = false;
		maxEggsPerSec *= Time.deltaTime;

		myAudio = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSeasonUnlockAnim>();
	}
	
	void Update () {
		if(myHubScript.inHub){
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
			// Check if new eggs were found.
			if(settingUp && myHubScript.dissolveDone) {
				// Debug.Log(lastEggVal);
				// Debug.Log(newEggVal);
				if (lastEggVal == newEggVal) {
					myHubScript.EnableSeasonObjs();
					settingUp = false;
				}
				timer += Time.deltaTime;
				if (timer >= scaleUpTime && !scaledUp) {
					// AUDIO - LOCK SCALES UP!
					//myAudio.lockScaleUpSnd();
					groupAnim.SetTrigger("ScaleUp");
					scaledUp = true;
				}
				if(timer >= setUpTime){
					settingUp = false;
					if (lastEggVal != newEggVal) {
						lerpEggAmnt = true;
					}
					timer = 0f;
				}
			}
			// Decrease the egg required counter.
			if (lerpEggAmnt) {
				// Gradually increase the count going down speed, up to a maximum speed amount.
				if (curEggReqSpeed < maxEggsPerSec) {
					curEggReqSpeed += Time.deltaTime / eggReqSpeedDamp;
				}
				lastEggVal -= curEggReqSpeed;
				// Trigger an animation every time the unlock counter amount changes.
				if (eggAmntForAnim != Mathf.RoundToInt(lastEggVal)) {
					// AUDIO - COUNTER GOES DOWN BY ONE!
					myAudio.eggCounterSnd();
					eggReqAnim.SetTrigger("ScaleCounter");
					oneReqSparkFX.Play();
				}
				eggAmntForAnim = Mathf.RoundToInt(lastEggVal);
				myEggCounter.text = eggAmntForAnim.ToString();
				if (lastEggVal <= newEggVal) {
					lerpEggAmnt = false;
					lastEggVal = newEggVal;
					multiReqSparkFX.Play();
					SaveNewLastEggVal();
					if (lastEggVal <= 0) {
						startLockAnimDelay = true;
					}
					else {
						enableSeasonObjsDelay = true;
					}
				}
			}
			// Start the season unlocked sequence.
			if (startLockAnimDelay) {
				removeLockAnimDelay -= Time.deltaTime;
				if (removeLockAnimDelay <= 0f) {
					unlockAnim.enabled = true;
					unlockAnim.SetTrigger("UnlockSeason");
					FadeOutBanner();
					startLockAnimDelay = false;
					locked = false;
					SaveNewLastEggVal();
					Debug.Log("this is where it saves really this time; " + Time.time);
				}
			}
			// Enable the level glows, buttons, etc.
			if (enableSeasonObjsDelay) {
				seasonObjsTimer += Time.deltaTime;
				if (seasonObjsTimer >= scaleDownDelay) {
					// AUDIO - LOCK SCALES DOWN!
					//myAudio.lockScaleDown();
					groupAnim.SetTrigger("ScaleDown");
				}
				if (seasonObjsTimer >= seasonObjsDelay) {
					myHubScript.EnableSeasonObjs();
					enableSeasonObjsDelay = false;
					seasonObjsTimer = 0f;
				}
			}
		}
	}
	void CheckUnlock(){
		locked = GlobalVariables.globVarScript.fallLocked;
		//Check if the season is already unlocked
		if(!locked && comingSoonTitle){
			bannerTitle.SetActive(false);
			closedLock.gameObject.SetActive(false);
			backColorFadeScript.gameObject.SetActive(true);
			backColorFadeScript.FadeIn();
			// To be terminated once all the seasons are implemented.
			comingSoonTitle.SetActive(true);
			comingSoonTitle.GetComponent<FadeInOutImage>().FadeIn();
			comingSoonTitle.GetComponentInChildren<FadeInOutTMP>().FadeIn();
			myHubScript.EnableSeasonObjs();
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
		// Debug.Log("I'm here  setup should be true");
	}
	void UnlockSequence(){
		closedLock.gameObject.SetActive(false);
		openLock.gameObject.SetActive(true);
		//TEST sounds
		myAudio.lockUnlockSnd();
		myAudio.lockFireworksTrailBurstSnd();
	}
	void FadeOutBanner(){
		bannerTitle.GetComponent<FadeInOutImage>().FadeOut();
		FadeInOutTMP[] myFade =  bannerTitle.GetComponentsInChildren<FadeInOutTMP>();
		foreach (FadeInOutTMP fade in myFade)
		{
			fade.FadeOut();
		}
	}
	public void CinematicButton() {
		if(comingSoonTit) {
			float delay = openLock.gameObject.GetComponent<FadeInOutImage>().fadeDuration;
			comingSoonTitle.SetActive(true);
			FadeInOutTMP textFade = comingSoonTitle.GetComponentInChildren<FadeInOutTMP>();
			FadeInOutImage imgFade = comingSoonTitle.GetComponent<FadeInOutImage>();
			textFade.FadeIn();
			imgFade.FadeIn();
		}
		else {
			foreach (FadeInOutImage fadeScript in playCinematicFadeScripts)
			{
				fadeScript.FadeIn();
			}
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
		
		Debug.Log(locked);
		GlobalVariables.globVarScript.lastEggTotVal = newEggVal;
		GlobalVariables.globVarScript.fallLocked = locked;
		Debug.Log(GlobalVariables.globVarScript.fallLocked);
		GeneralSaveLoadManager.SaveGeneralData(GlobalVariables.globVarScript);
		Debug.Log("after save cigaret");
	}

	public void ScaleDownGroup() {
		groupAnim.SetTrigger("ScaleDown");
	}

	public void BackToMenu() {
		checkSeason = false;
		scaledUp = false;
		eggAmntForAnim = 0;
		eggsLeft = 0f;
		curEggReqSpeed = 0f;
	}

	public void NewGame() {
		locked = true;
		//unlockAnim.enabled = false;
		unlockAnim.transform.localScale = new Vector3(1,1,1);
		unlockAnim.transform.localPosition = new Vector3(-9.53f, 4.82f, -0.3f);
		closedLock.transform.localScale = new Vector3(1,1,1);
		closedLock.GetComponent<Image>().color = new Color(1,1,1,1);
		closedLock.transform.eulerAngles = Vector3.zero;
		closedLock.transform.localPosition = new Vector3(9.53f, -4.82f, 0);
		openLock.transform.eulerAngles = Vector3.zero;
		openLock.transform.localScale = new Vector3(1,1,1);
		openLock.transform.localPosition = new Vector3(9.53f, -4.82f, 0);
		openLock.GetComponent<Image>().color = new Color(1,1,1,1);
		openLock.gameObject.SetActive(false);
	}
}
