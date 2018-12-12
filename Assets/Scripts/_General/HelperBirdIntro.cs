using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperBirdIntro : MonoBehaviour {
	private Vector3 mousePos;
	private Vector2 mousePos2D;
	private RaycastHit2D hit;
	public float shakeTime;
	private float timer;
	public Animator anim;
	public bool birdTapped;
	public Material dissMat;
	public float dissAmnt;

	public bool isDissolving, isDissolved;
	public inputDetector inputDetScript;
	public SlideInHelpBird slideInScript;
	public SceneTapEnabler scenTapEnabScript;

	void Start () {
		dissMat.SetFloat("_Threshold", 0f);
	}
	
	void Update () {
		if (!birdTapped && scenTapEnabScript.canTapHelpBird) {
			if(inputDetScript.Tapped){
				mousePos = Camera.main.ScreenToWorldPoint(inputDetScript.TapPosition);
				mousePos2D = new Vector2 (mousePos.x, mousePos.y);
				hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f);
			}

			if (hit && hit.collider.CompareTag("Helper")) {
				birdTapped = true;
				isDissolving = true;
				inputDetScript.cancelDoubleTap = true;
			}

			timer += Time.deltaTime;
			if (timer > shakeTime) {
				anim.SetTrigger("Shake");
				// Shake FX
				timer = 0f;
			}
		}
		
		if (isDissolving) {
			dissAmnt += Time.deltaTime;
			dissMat.SetFloat("_Threshold", dissAmnt);
			if (dissAmnt > 1f) {
				isDissolving = false;
				isDissolved = true;
			}
		}

		if (isDissolved) {
			slideInScript.MoveBirdUpDown();
			isDissolved = false;
		}
		
	}
}
