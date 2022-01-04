using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperBirdIntro : MonoBehaviour {
	[Header("Anim Shake")]
	public bool animShake;
	public Animator anim;
	public List<ParticleSystem> shakeParSys;
	[Header("Dissolve")]
	public float dissDuration;
	public Material dissMat;
	public bool waitToStartSeq;
	private float dissAmnt;
	private bool isDissolving, isDissolved, birdTapped;
	public GameObject inSceneBirdBtnObj, birdObj;
	private Vector3 ogBirdPos;
	public float minShapeSize, maxShapeSize;
	private float curShapeSize;
	public ParticleSystem dissParSys;
	[Header("Script References")]
	public inputDetector inputDetScript;
	public SlideInHelpBird slideInScript;
	public SceneTapEnabler sceneTapEnabScript;
	public BirdIntroSave birdIntroSaveScript;
	public AudioSceneGeneral audioSceneGenScript;
	[Header ("Info")]
	private Vector3 mousePos;
	private Vector2 mousePos2D;
	private RaycastHit2D hit;
	public float shakeCD, shakeFXCD;
	private float timer;
	private bool fxPlayed;

	void Start () {
		birdIntroSaveScript.LoadBirdIntro();
		if (slideInScript.introDone) {
			inSceneBirdBtnObj.SetActive(true);
			dissMat.SetFloat("_Threshold", 1.01f);
			birdTapped = true;
			this.enabled = false;
		}
		else {
			dissMat.SetFloat("_Threshold", 0f);
		}
		//ogBirdPos = birdObj.transform.position;
		if (!audioSceneGenScript) {
			audioSceneGenScript = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSceneGeneral>();
		}
	}
	
	void Update () {
		if (!sceneTapEnabScript.canTapHelpBird) {
			inSceneBirdBtnObj.SetActive(false);
		} else {
			inSceneBirdBtnObj.SetActive(true);
		}
		if (!birdTapped && sceneTapEnabScript.canTapHelpBird) {
			// If frozen bird has not been tapped yep cast a ray on tap.
			if (inputDetScript.Tapped){ 
				mousePos = Camera.main.ScreenToWorldPoint(inputDetScript.TapPosition);
				mousePos2D = new Vector2 (mousePos.x, mousePos.y);
				hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f);
			}
			// If frozen bird is tapped.
			if (hit && hit.collider.CompareTag("Helper")) {
				birdTapped = true;
				//waitToStartSeq = true;
				QueueSequenceManager.AddSequenceToQueue(StartBirdIntroSequence);
				inputDetScript.cancelDoubleTap = true;
				//birdObj.transform.position = ogBirdPos;
				dissParSys.Play();
				sceneTapEnabScript.canTapPauseBtn = false;
				audioSceneGenScript.unfrozenBirdSnd();
			}
			// Periodically shake the bird to attract the player's attention
			timer += Time.deltaTime;
			if (timer > shakeFXCD && !fxPlayed) {
				foreach (ParticleSystem ps in shakeParSys)
				{
					ps.Play();
				}
				fxPlayed = true;
			}
			if (timer > shakeCD) {
				if (animShake) {
					anim.SetTrigger("Shake");
					audioSceneGenScript.frozenBirdShake();
					timer = 0f;
					fxPlayed = false;
				} 
			}
		}
	}
	// Method(Action) called by the sequence queue manager script to start the bird intro sequence.
	void StartBirdIntroSequence() {
		StartCoroutine(DissolveHelpBird());
	}
	// Dissolve the bird's B&W material.
	IEnumerator DissolveHelpBird() {
		while (dissAmnt <= 1) {
			dissAmnt += Time.deltaTime / dissDuration;
			dissMat.SetFloat("_Threshold", dissAmnt);
			curShapeSize = Mathf.Lerp(minShapeSize, maxShapeSize, dissAmnt);
			var shapeMod = dissParSys.shape;
			shapeMod.radius = curShapeSize;
			yield return null;
		}
		// Once dissolved move up the bird for its intro.
		isDissolving = false;
		isDissolved = true;
		dissParSys.Stop();
		inSceneBirdBtnObj.SetActive(true);
		slideInScript.MoveBirdUpDown();
		this.enabled = false;
	}
}
