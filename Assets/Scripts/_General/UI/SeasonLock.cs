using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeasonLock : MonoBehaviour {

	public float eggsRequired, whiteSpeed, iterations, setUpTime;
	private float timer,iterCounter;
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
			else if(myCount.totEgg >= eggsRequired && !unlocking && locked){
				UnlockSeason();
			}
			else if(unlocking){
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
						unlocking = false;
						UnlockSequence();
					}
					else{
						timer = 0;
						whiteImage.position = whiteStartPos.position;
					}
				}
			}
		}
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
	}
}
