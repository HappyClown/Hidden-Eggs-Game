using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryScrollingBG : MonoBehaviour {
	public bool scroll;
	[Header ("Speed Up")]
	public float speedUpDuration;
	public AnimationCurve scrollAnimCurve;
	private float scrollSpeedLerpValue;
	private bool speedUp;
	private bool sidewaysScroll;
	[Header ("Regular Sideways")]
	public float regSideScrollSpeed;
	public float xLimit;
	public List<GameObject> regularSidewaysBGs;
	public List<FadeInOutSprite> regSidewaysBGFadeScripts;
	private float scrollValue;
	public float ScrollValue
	{ get{ return scrollValue; } }
	private int bgHInFront = 0;
	[Header ("Blurred Sideways")]
	public float blurSideScrollSpeed;
	public List<GameObject> blurredSidewaysBGs;
	[Header ("Vertical")]
	public float verticalScrollSpeed; public float verticalScrollSpeedTwo;
	public float yLimit;
	public List<GameObject> verticalBGs;
	private bool verticalScroll;
	private int bgVInFront = 0;
	[Header ("Other")]
	public List<GameObject> currentBGs;
	public bool slowDownClouds;
	private float scrollSpeed;
	public float ScrollSpeed
	{ get{ return scrollSpeed; } }

	void Start () {

	}
	
	void Update () {
		if (sidewaysScroll) {
			SidewaysScroll();
		}
		if (verticalScroll) {
			VerticalScroll();
		}
		if (slowDownClouds) {
			SlowDownClouds();
		}
	}

	public void SetUpClouds(List<GameObject> backGrounds, float myScrollSpeed, bool doISpeedUp = false, bool doIFadeIn = false) {
		//scrollValue;
		// Set the current background inactive, activate the chosen backgrounds.
		foreach (GameObject bg in currentBGs)
		{
			bg.SetActive(false);
		}
		currentBGs = backGrounds;
		foreach (GameObject bg in currentBGs)
		{
			bg.SetActive(true);
		}
		// Gradually speed up the clouds.
		speedUp = doISpeedUp;
		scrollSpeed = myScrollSpeed;
		if (!doISpeedUp) {
			scrollValue = myScrollSpeed;
		}
		// Set the correct bool true according to which background was chosen when the method was called.
		sidewaysScroll = false;
		verticalScroll = false;
		if (regularSidewaysBGs.Count > 0 && currentBGs[0] == regularSidewaysBGs[0] || blurredSidewaysBGs.Count > 0 && currentBGs[0] == blurredSidewaysBGs[0]) {
			sidewaysScroll = true;
		}
		if (verticalBGs.Count > 0 && currentBGs[0] == verticalBGs[0]) {
			verticalScroll = true;
		}
		//bgHInFront = 0;
		// Fade in the clouds.
		if (doIFadeIn) {
			foreach (FadeInOutSprite regSidewaysBGFadeScript in regSidewaysBGFadeScripts)
			{
				regSidewaysBGFadeScript.FadeIn();
			}
		}
	}

	void SidewaysScroll() {
		// Speed up.
		if (scrollSpeedLerpValue < 1 && speedUp) {
			scrollSpeedLerpValue += Time.deltaTime / speedUpDuration;
			scrollValue = Mathf.Lerp(0f, scrollSpeed, scrollAnimCurve.Evaluate(scrollSpeedLerpValue));
			if (scrollSpeedLerpValue >= 1) {
				speedUp = false;
				scrollSpeedLerpValue = 0f;
			}
		}

		for (int i = 0; i < currentBGs.Count; i++)
		{
			if (currentBGs[i].transform.position.x <= xLimit) {
				int otherBG = 1;
				//bgHInFront++;
				if (i+1 >= currentBGs.Count) {
					//bgHInFront = 0;
					otherBG = 0;
				}
				currentBGs[i].transform.position = new Vector3(currentBGs[otherBG].transform.position.x - xLimit, currentBGs[i].transform.position.y, currentBGs[i].transform.position.z);
			}
			currentBGs[i].transform.Translate(transform.right * Time.deltaTime * scrollValue * -1);
		}
	}

	void VerticalScroll() {
		if (scrollSpeedLerpValue < 1 && speedUp) {
			scrollSpeedLerpValue += Time.deltaTime / speedUpDuration;
			scrollValue = Mathf.Lerp(0, scrollSpeed, scrollAnimCurve.Evaluate(scrollSpeedLerpValue));
			if (scrollSpeedLerpValue >= 1) {
				speedUp = false;
				scrollSpeedLerpValue = 0f;
			}
		}
		for (int i = 0; i < currentBGs.Count; i++)
		{
			if (currentBGs[i].transform.position.y >= yLimit) {
				bgVInFront++;
				if (bgVInFront >= currentBGs.Count) {
					bgVInFront = 0;
				}
				currentBGs[i].transform.position = new Vector3(currentBGs[bgVInFront].transform.position.x, currentBGs[i].transform.position.y - (yLimit*2), currentBGs[i].transform.position.z);
			}
			currentBGs[i].transform.Translate(transform.up * Time.deltaTime * scrollValue);
		}
	}

	public void SlowDownClouds() {
		scrollSpeedLerpValue += Time.deltaTime / speedUpDuration;
		scrollValue = Mathf.Lerp(scrollSpeed, 0f, scrollAnimCurve.Evaluate(scrollSpeedLerpValue));
		if (scrollSpeedLerpValue >= 1) {
			slowDownClouds = false;
			scrollSpeedLerpValue = 0f;
		}
	}

	public void SetCloudSpeed(float myScrollSpeed) {
		scrollSpeed = myScrollSpeed;
		scrollValue = myScrollSpeed;
	}

	public void TurnOffScrollClouds() {
		foreach (GameObject blurredSidewaysBG in blurredSidewaysBGs)
		{
			blurredSidewaysBG.SetActive(false);
		}
		foreach (GameObject regularSidewaysBG in regularSidewaysBGs)
		{
			regularSidewaysBG.SetActive(false);
		}
		foreach (GameObject verticalBG in verticalBGs)
		{
			verticalBG.SetActive(false);
		}
	}
}
