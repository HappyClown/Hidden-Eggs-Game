using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSilverEgg : MonoBehaviour {
	public bool sendToPanel;
	public ClickOnEggs clickOnEggsScript;
	public int posInPanel;
	private float spawnDelay;
	private Vector3 iniPos, iniScale;
	public float lerpTimer;
	public AnimationCurve animCurve;
	public float moveDuration;
	public ParticleSystem trailFX, burstFX;
	private bool silEggAdded = false;
	public bool lastSpawned = false;
	public MoveWithCamera moveWithCamScript;
	public FadeInOutSprite myEggPanelShadow;
	public AudioSceneGeneral audioSceneGenScript;

	void Start() {
		if (!audioSceneGenScript) {
			audioSceneGenScript = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSceneGeneral>();
		}
	}

	void Update () 
	{
		if (sendToPanel) {
			spawnDelay -= Time.deltaTime;
			if (spawnDelay <= 0) {
				if (!silEggAdded) { 
					//Debug.Log("Pssst, over here!" + posInPanel);
					AddToSceneSilEgg();
					GlobalVariables.globVarScript.SaveEggState();
					//clickOnEggsScript.AdjustSilverEggCount();
					clickOnEggsScript.AddEggsFound();
					silEggAdded = true;
				}
				if (!trailFX.isPlaying) {
					audioSceneGenScript.silverEggsPanel(this.gameObject);
					trailFX.Play(true);
					burstFX.Play(true);
				}
				lerpTimer += Time.deltaTime / moveDuration;
				this.transform.localScale = Vector3.Lerp(iniScale, iniScale * moveWithCamScript.newScale, lerpTimer);
				this.transform.position = Vector3.Lerp(iniPos, clickOnEggsScript.silverEggsInPanel[posInPanel].transform.position, animCurve.Evaluate(lerpTimer));
				//Debug.Log("A Silver egg moves from puzz to pan! Dun dun duuuunnnn");
				if (lerpTimer >= 1) {
					this.transform.position = clickOnEggsScript.silverEggsInPanel[posInPanel].transform.position;
					clickOnEggsScript.eggMoving--; // if egg pos = panel pos
					sendToPanel = false;
					this.transform.parent = clickOnEggsScript.silverEggsInPanel[posInPanel].transform.parent;
					this.transform.localScale = iniScale;
					clickOnEggsScript.silverEggsFound++;
					clickOnEggsScript.AddEggsFound();
					clickOnEggsScript.UpdateEggsString();
					myEggPanelShadow = clickOnEggsScript.silEggsShadFades[posInPanel];
					myEggPanelShadow.FadeIn();
					trailFX.Stop(true);
					if (lastSpawned) {	
						// Sequence finished.
						ClickOnEggs.inASequence = false;
						lastSpawned = false;
					}
				}
			}
		}
	}
	
	public void SendToPanel (int numInPanel, float myDelay) {
		sendToPanel = true;
		posInPanel = numInPanel;
		spawnDelay = myDelay;
		clickOnEggsScript.eggMoving++;
		iniPos = this.transform.position;
		iniScale = this.transform.localScale;
	}

	public void AddToSceneSilEgg() {
			GlobalVariables.globVarScript.sceneSilEggsCount.Add(posInPanel); 
	}
}
