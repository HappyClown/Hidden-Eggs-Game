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
		}
		else {
			dissMat.SetFloat("_Threshold", 0f);
		}
		ogBirdPos = birdObj.transform.position;
		if (!audioSceneGenScript) {
			audioSceneGenScript = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSceneGeneral>();
		}
	}
	
	void Update () {
		if(!sceneTapEnabScript.canTapHelpBird){
			inSceneBirdBtnObj.SetActive(false);
		}else{
			inSceneBirdBtnObj.SetActive(true);
		}
		if (!birdTapped && sceneTapEnabScript.canTapHelpBird) {
			if(inputDetScript.Tapped){ // If frozen bird has not been tapped yep cast a ray on tap
				mousePos = Camera.main.ScreenToWorldPoint(inputDetScript.TapPosition);
				mousePos2D = new Vector2 (mousePos.x, mousePos.y);
				hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f);
			}
			// If frozen bird is tapped
			if (hit && hit.collider.CompareTag("Helper")) {
				birdTapped = true;
				waitToStartSeq = true;
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
		// Wait until no sequence is playing to start the Bird Intro sequence.
		if (waitToStartSeq && !ClickOnEggs.inASequence) {
			isDissolving = true;
			// In a sequence.
			ClickOnEggs.inASequence = true;
			waitToStartSeq = false;
			Debug.Log("Trying to start dissolving");
		}
		// After being tapped dissolve the black and white bird's material
		if (isDissolving) {
			dissAmnt += Time.deltaTime / dissDuration;
			dissMat.SetFloat("_Threshold", dissAmnt);

			curShapeSize = Mathf.Lerp(minShapeSize, maxShapeSize, dissAmnt);
			var shapeMod = dissParSys.shape;
			shapeMod.radius = curShapeSize;
			// // In a sequence.
			// if (!ClickOnEggs.inASequence) {
			// 	ClickOnEggs.inASequence = true;
			// }
			if (dissAmnt > 1f) {
				isDissolving = false;
				isDissolved = true;
				dissParSys.Stop();
			}
		}
		// Once dissolved move up the bird for its intro
		if (isDissolved) {
			inSceneBirdBtnObj.SetActive(true);
			slideInScript.MoveBirdUpDown();
			isDissolved = false;
		}
		
	}
}
