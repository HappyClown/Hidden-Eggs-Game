using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GoldenEgg : MonoBehaviour {
	[HideInInspector]
	public bool waitingToStartSeq;
	private float seqTimer;
	public PolygonCollider2D goldenEggCollider;

	[Header ("Sequence")]
	public float startCover;
	public float startText, startEgg, startFireWorks;
	private bool coverB, textB, eggB, fireWorksB;

	[Header("Egg Animation")]
	public Animator anim;
	private bool eggAnimStarted;

	[Header("Screen Cover")]
	public FadeInOutImage fadeCoverImg;

	[Header("Congratulations")]
	public Animator titleAnim;
	public TMPTextColorFade textFadeScript;
	public List<FadeInOutSprite> starFadeScripts;
	public RotationBurst[] rotationBursts;
	private bool congratsTxtOn, congratsTxtOff;
	private float congratsA;

	[Header("Particles")]
	public GameObject fireworkHolder;
	public ParticleSystem firework01, firework02;
	public ParticleSystem partShafts, partSparkles, partPop;
	public GameObject eggGlow, eggGlowTwo;
	private float partShaftsA;
	public float partShaftsShrinkTime;

	[Header("After Tap Sequence")]
	[TooltipAttribute("The time it takes in seconds before starting this sequence after the GoldenEgg has been tapped.")]
	public float eggToCornerTime;
	[TooltipAttribute("The time it takes in seconds before starting this sequence after the GoldenEgg has been tapped.")]
	public float congratsOffTime, coverOffTime;
	public string inPanelSortingLayer;
	public int inPanelOrderInLayer;
	public SpriteRenderer goldEggSpriteRend;
	private float eggToCornerTimer;

	[Header("Script References")]
	public LevelTapMannager lvlTapManScript;
	public ClickOnEggs clickOnEggsScript;
	public SceneTapEnabler scenTapEnabScript;
	public AudioRiddleSolvedAnim audioRiddleSolvedAnimScript;

	void Start () {
		if (!audioRiddleSolvedAnimScript) audioRiddleSolvedAnimScript = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioRiddleSolvedAnim>();
	}

	public void StartGoldenEggSequence () {
		this.gameObject.SetActive(true);
		CannotTaps();
		lvlTapManScript.ZoomOutCameraReset();
		eggGlow.SetActive(true);
		eggGlowTwo.SetActive(true);
		StartCoroutine(GoldenEggInSequence());
	}

	IEnumerator GoldenEggInSequence() {
		while (seqTimer < startText) {
			seqTimer += Time.deltaTime;
			// Start things according to the sequence timer.
			if (seqTimer >= startCover && !coverB) { 
				fadeCoverImg.FadeIn();
				coverB = true;
			}
			if (seqTimer >= startText && !textB) {
				titleAnim.gameObject.SetActive(true);
				titleAnim.SetTrigger("PopIn");
				textFadeScript.startFadeIn = true;
				textB = true;
				audioRiddleSolvedAnimScript.riddleSolvedTextSnd();
			}
			if (seqTimer >= startFireWorks && !fireWorksB) {
				fireworkHolder.SetActive(true);
				firework01.Play(true);
				firework02.Play(true);
				fireWorksB = true;
				eggGlow.SetActive(false);
				eggGlowTwo.SetActive(false);
				audioRiddleSolvedAnimScript.fireworkTrailBurstSnd();
			}
			if (seqTimer >= startEgg && !eggB) {
				anim.SetTrigger("StartAnim");
				eggAnimStarted = true;
				eggB = true;
			}
			yield return null;
		}
	}

	IEnumerator GoldenEggToPanel() {
		while (eggToCornerTimer < coverOffTime) {
			eggToCornerTimer += Time.deltaTime;
			if (eggToCornerTimer >= coverOffTime) { 
				fadeCoverImg.FadeOut(); 
			}
			yield return null;
		}
		while (eggToCornerTimer < congratsOffTime) {
			eggToCornerTimer += Time.deltaTime;
			if (eggToCornerTimer >= congratsOffTime) { 
				textFadeScript.startFadeOut = true; 
				foreach( FadeInOutSprite starFadeScript in starFadeScripts)
				{
					starFadeScript.FadeOut();
				}
				foreach (RotationBurst rotBurst in rotationBursts) {
					rotBurst.StartCoroutine(rotBurst.ShrinkTipTrails());
				}
			}
			yield return null;
		}
		while(eggToCornerTimer < eggToCornerTime) {
			eggToCornerTimer += Time.deltaTime;
			yield return null;
		}
		goldEggSpriteRend.sortingLayerName = inPanelSortingLayer;
		goldEggSpriteRend.sortingOrder = inPanelOrderInLayer;
		//clickOnEggsScript.UpdateEggsString(false, true);
		QueueSequenceManager.SequenceComplete();
	}

	IEnumerator FadeShafts() {
		partShafts.Stop(true); 
		float shaftScale = partShafts.transform.localScale.x;
		while (shaftScale > 0) {
			shaftScale -= Time.deltaTime / partShaftsShrinkTime;
			partShafts.transform.localScale = new Vector3(shaftScale, shaftScale, 1);
			yield return null;
		}
		partShafts.gameObject.SetActive(false);
	}

	// - CALLED DURING ANIMATIONS - // (Animation Events)
	#region Animation Events
	void StartStopShafts() {
		StartCoroutine(FadeShafts());
	}
	void StartStopSparkles() {
		if (!partSparkles.gameObject.activeSelf && !partSparkles.isPlaying) 
		{ 
			partSparkles.gameObject.SetActive(true); 
			partSparkles.Play(true); 
		}
		else { 
			partSparkles.gameObject.SetActive(false); 
			partSparkles.Stop(true); 
		}
	}
	void StartPop() {
		if (!partPop.isPlaying) 
		{ 
			partPop.gameObject.SetActive(true);
			partPop.Play(true); 
		}
	}
	void ActivateCollider() {
		goldenEggCollider.enabled = true;
	}
	void SendEggToCornerSequence() {
		StartCoroutine(GoldenEggToPanel());
	}
	void StopAnim () {
		anim.enabled = false;
	}
	public void CanTapGold () {
		scenTapEnabScript.canTapGoldEgg = true;
	}
	public void GoldEggAnimSound() {
		audioRiddleSolvedAnimScript.goldenEggIdleSnd();
	}
	public void GoldEggShimmerPlaySound() {
		audioRiddleSolvedAnimScript.goldenEggIdleSnd();
	}
	void TextOnOff () {
		if (congratsA <= 0) congratsTxtOn = true;
		else if (congratsA >= 1) congratsTxtOff = true;
	}
	#endregion

	public void CanTaps() {
		scenTapEnabScript.canTapEggRidPanPuz = true;
		scenTapEnabScript.canTapPauseBtn = true;
		scenTapEnabScript.canTapHelpBird = true;
	}
	public void CannotTaps() {
		scenTapEnabScript.canTapEggRidPanPuz = false;
		scenTapEnabScript.canTapPauseBtn = false;
		scenTapEnabScript.canTapHelpBird = false;
	}
}