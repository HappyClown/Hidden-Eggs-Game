using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggsSaveLoad : MonoBehaviour
{
    public ClickOnEggs clickOnEggs;
    public SceneEggMovement sceneEggMovement;

	public void SetEggStates() {
		// myEggIndex = clickOnEggsScript.eggs.IndexOf(this.gameObject);
		// myFoundSpotInPanel = GlobalVariables.globVarScript.eggsFoundOrder[myEggIndex];
		// if (!eggAnim) { eggAnim = this.GetComponent<Animator>(); }
		// LoadEggFromCorrectScene();
		// // If the egg has already been found previously (if it has been loaded as true)
		// if (eggFound) {
		// 	eggAnim.enabled = false;
		// 	if (!mySpotInPanel) {
		// 		mySpotInPanel = clickOnEggsScript.eggSpots[myFoundSpotInPanel];
		// 	}

		// 	this.transform.position = new Vector3(mySpotInPanel.transform.position.x, mySpotInPanel.transform.position.y, mySpotInPanel.transform.position.z - 0.24f + (myFoundSpotInPanel * 0.01f) - 4);
		// 	this.transform.eulerAngles = cornerRot;
		// 	this.transform.localScale = cornerEggScale;

		// 	this.GetComponent<Collider2D>().enabled = false;
		// 	if (!amIGolden) { 
		// 		clickOnEggsScript.eggsFound++; 
		// 		clickOnEggsScript.eggsInPanel++;
		// 		if (clickOnEggsScript.eggsInPanel == puzzUnlockScript.puzzUnlockAmnt) {
		// 			puzzUnlockScript.ActivatePuzzle();
		// 		}
		// 	}
		// 	else {
		// 		goldenEgg.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
		// 		goldenEgg.transform.localPosition = Vector3.zero;
		// 	}
		// 	this.transform.parent = clickOnEggsScript.eggPanel.transform;
		// 	clickOnEggsScript.UpdateEggsString();
		// 	clickOnEggsScript.AddEggsFound();
		// 	if (!amIGolden) {
		// 		eggShadowFade = clickOnEggsScript.eggShadowsFades[myFoundSpotInPanel];
		// 	}
		// 	eggShadowFade.FadeIn();
		// }

        // Load in the scene eggs that have already been found.
        for (int i = 0; i < clickOnEggs.eggs.Count; i++)
        {
            if (GlobalVariables.globVarScript.eggsFoundBools[i]) {
                GameObject egg = clickOnEggs.eggs[i];
                // Get the eggs spot in the egg panel.
                int spotInPanel = GlobalVariables.globVarScript.eggsFoundOrder[i];
                GameObject eggSpot = clickOnEggs.eggSpots[spotInPanel];
                // Turn the egg's animator component off. (likely not needed since the animator should already be off in the scene)
                // clickOnEggs.eggs[i].GetComponent<Animator>().enabled = false;
                // Set the egg at the right position, rotation and scale in the egg panel.
                egg.transform.position = new Vector3(eggSpot.transform.position.x, eggSpot.transform.position.y, eggSpot.transform.position.z - 0.24f + (spotInPanel * 0.01f) - 4);
                egg.transform.eulerAngles = sceneEggMovement.cornerRotation;
                egg.transform.localScale = sceneEggMovement.cornerScale;
                // Set the egg's parent to egg panel.
                egg.transform.parent = clickOnEggs.eggPanel.transform;
                // Disable the egg's collider.
                egg.GetComponent<Collider2D>().enabled = false;
                // Add to the eggsFound and eggsInPanel totals.
                clickOnEggs.eggsFound++;
                clickOnEggs.eggsInPanel++;
                // Fade in the egg panel egg shadow or set its alpha to 1.
            }
        }
        // After all the found eggs have been put in the egg panel, adjust other scene variables.
        // Check if the puzzle should be unlocked.
        clickOnEggs.puzzUnlockScript.PuzzleUnlockCheck(clickOnEggs.eggsInPanel);
        // Update the egg panel's egg counts.
        clickOnEggs.UpdateEggsString();
        //add up total eggs found AddEggsFound (clickoneggs) which also checks for level complete
	}
    public void SaveEgg(int _eggIndex) {
		GlobalVariables.globVarScript.totalEggsFound = clickOnEggs.totalEggsFound;
		GlobalVariables.globVarScript.eggsFoundBools[_eggIndex] = true;
		GlobalVariables.globVarScript.eggsFoundOrder[_eggIndex] = clickOnEggs.eggsFound;
		GlobalVariables.globVarScript.SaveEggState();
	}
}
