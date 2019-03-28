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
	private float scrollValue;
	private int bgInFront = 0;
	[Header ("Blurred Sideways")]
	public float blurSideScrollSpeed;
	public List<GameObject> blurredSidewaysBGs;
	[Header ("Vertical")]
	public float verticalScrollSpeed;
	public float yLimit;
	public List<GameObject> verticalBGs;
	private bool verticalScroll;
	[Header ("Other")]
	public List<GameObject> currentBGs;
	private float scrollSpeed;

	void Start () {

	}
	
	void Update () {
		if (sidewaysScroll) {
			SidewaysScroll();
		}
		if (verticalScroll) {
			VerticalScroll();
		}
	}

	public void SetUpClouds(List<GameObject> backGrounds, float myScrollSpeed, bool doISpeedUp) {
		//scrollValue;
		foreach (GameObject bg in currentBGs)
		{
			bg.SetActive(false);
		}
		currentBGs = backGrounds;
		foreach (GameObject bg in currentBGs)
		{
			bg.SetActive(true);
		}
		speedUp = doISpeedUp;
		scrollSpeed = myScrollSpeed;
		if (!doISpeedUp) {
			scrollValue = myScrollSpeed;
		}
		// if (currentBGs[0] == regularSidewaysBGs[0] || currentBGs[0] == blurredSidewaysBGs[0]) {
		// 	sidewaysScroll = true;
		// }
		// else {
		// 	sidewaysScroll = false;
		// }
		if (currentBGs[0] == verticalBGs[0]) {
			verticalScroll = true;
		}
		else {
			verticalScroll = false;
		}
	}

	void SidewaysScroll() {
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
			if (currentBGs[i].transform.position.x <= xLimit) {
				bgInFront++;
				if (bgInFront >= currentBGs.Count) {
					bgInFront = 0;
				}
				currentBGs[i].transform.position = new Vector3(currentBGs[bgInFront].transform.position.x - xLimit, currentBGs[i].transform.position.y, currentBGs[i].transform.position.z);
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
			if (currentBGs[i].transform.position.y <= yLimit) {
				bgInFront++;
				if (bgInFront >= currentBGs.Count) {
					bgInFront = 0;
				}
				currentBGs[i].transform.position = new Vector3(currentBGs[bgInFront].transform.position.x, currentBGs[i].transform.position.y - yLimit, currentBGs[i].transform.position.z);
			}
			currentBGs[i].transform.Translate(transform.up * Time.deltaTime * scrollValue * -1);
		}
	}
}
