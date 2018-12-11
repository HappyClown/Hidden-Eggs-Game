using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperBirdIntro : MonoBehaviour {
	private Vector3 mousePos;
	private Vector2 mousePos2D;
	private RaycastHit2D hit;
	public float introTime;
	private float timer;
	public Animator anim;

	public bool isDissolving, isDissolved;
	public inputDetector inputDetScript;

	void Start () {
		
	}
	
	void Update () {
		if(inputDetScript.Tapped){
			mousePos = Camera.main.ScreenToWorldPoint(inputDetScript.TapPosition);
			mousePos2D = new Vector2 (mousePos.x, mousePos.y);
			hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f);
		}
		timer += Time.deltaTime;
		if (timer > introTime) {
			
			// Shake anim

		}
	}
}
