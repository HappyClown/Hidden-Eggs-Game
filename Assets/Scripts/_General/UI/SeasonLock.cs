using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SeasonLock : MonoBehaviour {

	public float eggsRequired, whiteSpeed, iterations, setUpTime;
	private float timer;
	public TextMeshProUGUI myEggCounter;
	private int iterCounter;
	public RectTransform  whiteTarget, whiteImage, whiteStartPos;
	public Image closedLock, openLock;
	public GameObject bannerTitle,comingSoonTitle,lockMask;
	public HubEggcounts myCount;
	public Hub myHub;
	public bool locked, comingSoonTit, settingUp;
	private bool checkSeason, unlocking;
	// Use this for initialization
	void Start () {
		unlocking = false;
		timer = 0;
		iterCounter = 0;
		lockMask.SetActive(false);
		checkSeason = false;
	}
	
	// Update is called once per frame
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
				timer += Time.deltaTime * whiteSpeed;
				if(timer <= 1){
					whiteImage.position = Vector3.Lerp(whiteStartPos.position,whiteTarget.position,timer);
				}
				else{
					iterCounter ++;
					if(iterCounter == iterations)
					{
						timer = 0;
						locked = false;
						UnlockSequence();
					}
					else{
						timer = 0;
						whiteImage.position = whiteStartPos.position;
					}
				}
			}
			else if (unlocking)
			{
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
		closedLock.gameObject.GetComponent<FadeInOutImage>().FadeIn();
		settingUp = true;
				Debug.Log("I'm here!!");
	}
	void UnlockSeason(){
		unlocking = true;
		lockMask.SetActive(true);
	}
	void UnlockSequence(){
		closedLock.gameObject.SetActive(false);
		openLock.gameObject.SetActive(true);
		lockMask.SetActive(false);
	}
	void RemoveLock(){
		openLock.gameObject.GetComponent<FadeInOutImage>().FadeOut();
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
			//activate next season!!!
		}
	}
}
