using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSilverEgg : MonoBehaviour {
	public bool sendToPanel;
	public ClickOnEggs clickOnEggsScript;
	public int posInPanel;
	public FadeInOutSprite myEggPanelShadow;
	private float spawnDelay;
	private Vector3 iniPos;
	public float lerpTimer;
	public AnimationCurve animCurve;
	public float moveDuration;
	public ParticleSystem trailFX, burstFX;
	private bool silEggAdded = false;

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
					trailFX.Play(true);
					burstFX.Play(true);
				}
				lerpTimer += Time.deltaTime / moveDuration;
				this.transform.position = Vector3.Lerp(iniPos, clickOnEggsScript.silverEggsInPanel[posInPanel].transform.position, animCurve.Evaluate(lerpTimer));
				//Debug.Log("A Silver egg moves from puzz to pan! Dun dun duuuunnnn");
				if (lerpTimer >= 1) {
					this.transform.position = clickOnEggsScript.silverEggsInPanel[posInPanel].transform.position;
					clickOnEggsScript.eggMoving--; // if egg pos = panel pos
					sendToPanel = false;
					this.transform.parent = clickOnEggsScript.silverEggsInPanel[posInPanel].transform.parent;
					clickOnEggsScript.silverEggsFound++;
					clickOnEggsScript.AddEggsFound();
					clickOnEggsScript.UpdateEggsString();
					myEggPanelShadow.FadeIn();
					trailFX.Stop(true);
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
	}

	public void AddToSceneSilEgg() {
		// if (SceneManager.GetActiveScene().name == GlobalVariables.globVarScript.marketName)
		// {
			GlobalVariables.globVarScript.sceneSilEggsCount.Add(posInPanel); 
		// }

		// if (SceneManager.GetActiveScene().name == GlobalVariables.globVarScript.parkName)
		// {
		// 	GlobalVariables.globVarScript.sceneSilEggsCount.Add(posInPanel); 
		// }

		// if (SceneManager.GetActiveScene().name == GlobalVariables.globVarScript.beachName)
		// {

		// }
	}
}
