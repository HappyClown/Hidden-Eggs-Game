using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSilverEgg : MonoBehaviour 
{
	public bool sendToPanel;
	public ClickOnEggs clickOnEggsScript;
	public int posInPanel;

	void Update () 
	{
		if (sendToPanel)
		{
			// add animation & movement
			this.transform.position = clickOnEggsScript.silverEggsInPanel[posInPanel].transform.position;
			this.transform.parent = clickOnEggsScript.silverEggsInPanel[posInPanel].transform.parent;
			sendToPanel = false;
			clickOnEggsScript.eggMoving--;
		}
	}
	
	public void SendToPanel (int numInPanel) 
	{
		sendToPanel = true;
		posInPanel = numInPanel;
		clickOnEggsScript.eggMoving++;
	}
}
