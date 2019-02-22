using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperBirdIntro : MonoBehaviour {
	private Vector3 mousePos;
	private Vector2 mousePos2D;
	private RaycastHit2D hit;
	public float shakeCD, shakeFXCD;
	private float timer;
	private bool fxPlayed;
	[Header("Anim Shake")]
	public bool animShake;
	public Animator anim;
	public List<ParticleSystem> shakeParSys;
	[Header("Dissolve")]
	public Material dissMat;
	public float dissDuration;
	private bool birdTapped;
	private float dissAmnt;
	private bool isDissolving, isDissolved;
	public GameObject inSceneBirdBtnObj, birdObj;
	private Vector3 ogBirdPos;
	public float minShapeSize, maxShapeSize;
	private float curShapeSize;
	public ParticleSystem dissParSys;
	[Header("Script References")]
	public inputDetector inputDetScript;
	public SlideInHelpBird slideInScript;
	public SceneTapEnabler scenTapEnabScript;
	public BirdIntroSave birdIntroSaveScript;

	void Start () {
		birdIntroSaveScript.LoadBirdIntro();
		if (slideInScript.introDone) {
			inSceneBirdBtnObj.SetActive(true);
			dissMat.SetFloat("_DissolveAmount", 1.01f);
			birdTapped = true;
		}
		else {
			dissMat.SetFloat("_DissolveAmount", 0f);
		}
		ogBirdPos = birdObj.transform.position;
	}
	
	void Update () {
		if (!birdTapped && scenTapEnabScript.canTapHelpBird) {
			if(inputDetScript.Tapped){ // If frozen bird has not been tapped yep cast a ray on tap
				mousePos = Camera.main.ScreenToWorldPoint(inputDetScript.TapPosition);
				mousePos2D = new Vector2 (mousePos.x, mousePos.y);
				hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f);
			}
			// If frozen bird is tapped
			if (hit && hit.collider.CompareTag("Helper")) {
				birdTapped = true;
				isDissolving = true;
				inputDetScript.cancelDoubleTap = true;
				birdObj.transform.position = ogBirdPos;
				dissParSys.Play();
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
					timer = 0f;
					fxPlayed = false;
				} 
			}
		}
		// After being tapped dissolve the black and white bird's material
		if (isDissolving) {
			dissAmnt += Time.deltaTime / dissDuration;
			dissMat.SetFloat("_DissolveAmount", dissAmnt);

			curShapeSize = Mathf.Lerp(minShapeSize, maxShapeSize, dissAmnt);
			var shapeMod = dissParSys.shape;
			shapeMod.radius = curShapeSize;
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
