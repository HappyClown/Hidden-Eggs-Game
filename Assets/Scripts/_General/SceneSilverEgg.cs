using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSilverEgg : MonoBehaviour 
{
	public bool sendToPanel;
	public ClickOnEggs clickOnEggsScript;
	public int posInPanel;
	private float spawnDelay;

	private Vector3 iniPos;

	public float lerpTimer;
	public AnimationCurve animCurve;
	public float moveDuration;

	private bool silEggAdded = false;


	void Update () 
	{
		if (sendToPanel)
		{
			spawnDelay -= Time.deltaTime;
			if (spawnDelay <= 0)
			{
				if (!silEggAdded) 
				{ 
					Debug.Log("Pssst, over here!" + posInPanel);
					GlobalVariables.globVarScript.marketSceneSilEggsCount.Add(posInPanel); 
					GlobalVariables.globVarScript.SaveEggState();
					clickOnEggsScript.silverEggsFound++;
					clickOnEggsScript.AdjustSilverEggCount();
					clickOnEggsScript.AddEggsFound();
					silEggAdded = true;
				}
				lerpTimer += Time.deltaTime / moveDuration;
				this.transform.position = Vector3.Lerp(iniPos, clickOnEggsScript.silverEggsInPanel[posInPanel].transform.position, animCurve.Evaluate(lerpTimer));
				Debug.Log("A Silver egg moves from puzz to pan! Dun dun duuuunnnn");
				if (lerpTimer >= 1)
				{
					this.transform.position = clickOnEggsScript.silverEggsInPanel[posInPanel].transform.position;
					clickOnEggsScript.eggMoving--; // if egg pos = panel pos
					sendToPanel = false;
					this.transform.parent = clickOnEggsScript.silverEggsInPanel[posInPanel].transform.parent;
					clickOnEggsScript.AddEggsFound();
				}
				
			}
		}
	}
	
	public void SendToPanel (int numInPanel, float myDelay) 
	{
		sendToPanel = true;
		posInPanel = numInPanel;
		spawnDelay = myDelay;
		clickOnEggsScript.eggMoving++;
		iniPos = this.transform.position;
	}
}
