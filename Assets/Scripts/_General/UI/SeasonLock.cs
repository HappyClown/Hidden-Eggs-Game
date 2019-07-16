using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SeasonLock : MonoBehaviour {
	[Header ("Settings")]
	public bool comingSoonTit;
	public float eggsRequired, whiteSpeed, iterations, setUpTime;
	[Header ("References")]
	public Hub myHub;
	public HubEggcounts myCount;
	public Animator unlockAnim;
	public FadeInOutImage backColorFadeScript;
	public FadeInOutTMP amntReqFadeScript;
	public TextMeshProUGUI myEggCounter;
	public RectTransform  whiteTarget, whiteImage, whiteStartPos;
	public Image closedLock, openLock;
	public GameObject bannerTitle, comingSoonTitle, lockMask;
	[Header ("Info")]
	public bool locked;
	public bool settingUp;
	private bool checkSeason, unlocking;
	private int iterCounter;
	private float timer;

	void Start () {
		unlocking = false;
		timer = 0;
		iterCounter = 0;
		lockMask.SetActive(false);
		checkSeason = false;
	}
	
	void Update () {
		float eggsLeft;
		eggsLeft = eggsRequired - myCount.totEgg;
		if(eggsLeft <= 0){
			eggsLeft = 0;
		}
		if(myHub.inHub){
			if(!checkSeason){
				CheckUnlock();
				if(locked){
					SetUpTitle();
				}
				checkSeason = true;
			}
			if(settingUp){
				timer += Time.deltaTime;
				if(timer >= setUpTime){
					settingUp = false;
					timer = 0;
					Debug.Log(myCount.totEgg);
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
			else if (unlocking)
			{
				unlockAnim.SetTrigger("UnlockSeason");
				RemoveLock();
				unlocking = false;
			}
		}
		myEggCounter.text = eggsLeft.ToString();
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
}
